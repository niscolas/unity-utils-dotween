using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityExtensions;

namespace ScriptableTween.Tweens
{
	[EditorIcon("atom-icon-purple")]
	public abstract class BaseScriptableTween<T> : AtomAction<T>
	{
		[Title("Tween")]
		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		protected T fixedTarget;

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		protected IntReference loops;

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		protected LoopType loopType;

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		protected BoolReference useUnscaledTime;

		[TabGroup("Main Settings", "Appearance")]
		[SerializeField]
		private bool _useAnimationCurve;

		[TabGroup("Main Settings", "Appearance")]
		[HideIf(nameof(_useAnimationCurve))]
		[SerializeField]
		protected Ease[] possibleEaseTypes;

		[TabGroup("Main Settings", "Appearance")]
		[ShowIf(nameof(_useAnimationCurve))]
		[SerializeField]
		private AnimationCurve _curve;

		[TabGroup("Main Settings", "Duration")]
		[SerializeField]
		private FloatReference duration = new FloatReference(0.1f);

		[TabGroup("Main Settings", "Duration")]
		[SerializeField]
		private bool randomizeDuration;

		[TabGroup("Main Settings", "Duration")]
		[ShowIf(nameof(randomizeDuration))]
		[Range(0, 1)]
		[SerializeField]
		private float durationRandomization;

		[TabGroup("Main Settings", "Delay")]
		[SerializeField]
		private FloatReference delay;

		[TabGroup("Main Settings", "Delay")]
		[SerializeField]
		private bool randomizeDelay;

		[TabGroup("Main Settings", "Delay")]
		[ShowIf(nameof(randomizeDelay))]
		[Range(0, 1)]
		[SerializeField]
		private float delayRandomization;

		[TabGroup("Main Settings", "Life Time")]
		[SerializeField]
		private bool autoKill = true;

		[TabGroup("From To", "To")]
		[SerializeField]
		private bool isRelative;

		private Ease CurrentEaseType
		{
			get
			{
				if (!possibleEaseTypes.IsNullOrEmpty())
				{
					return possibleEaseTypes.RandomElement();
				}

				return default;
			}
		}

		protected float CurrentDuration => GetNewValueFor(duration.Value, durationRandomization, randomizeDuration);

		private float CurrentDelay => GetNewValueFor(delay.Value, delayRandomization, randomizeDelay);

		public abstract IEnumerable<Tween> GetTweens(T target);

		public override void Do()
		{
			Do(fixedTarget);
		}

		public override async void Do(T target)
		{
			await DoAsync(target);
		}

		public async UniTask DoAsync()
		{
			await DoAsync(fixedTarget);
		}

		public virtual async UniTask DoAsync(T target)
		{
			IList<Tween> tweens = GetTweens(target).ToList();
			
			if (tweens.IsNullOrEmpty()) return;

			for (int i = 0; i < tweens.Count; i++)
			{
				if (tweens[i] == null)
				{
					tweens.RemoveAt(i);
					continue;
				}

				ApplyDefaultOptions(tweens[i], target);
			}

			IEnumerable<UniTask> tweenTasks = tweens.Select(tween => tween.AsyncWaitForCompletion().AsUniTask());

			await UniTask.WhenAll(tweenTasks);
		}

		protected virtual Tween ApplyDefaultOptions(Tween tween, T target)
		{
			tween = tween.SetLoops(loops, loopType)
				.SetAutoKill(autoKill)
				.SetDelay(CurrentDelay)
				.SetUpdate(useUnscaledTime)
				.SetRelative(isRelative);

			if (_useAnimationCurve)
			{
				tween = tween.SetEase(_curve);
			}
			else
			{
				tween = tween.SetEase(CurrentEaseType);
			}

			return tween;
		}

		private float GetNewValueFor(float value, float randomization, bool randomize)
		{
			if (randomize)
			{
				return value.Randomize(randomization);
			}

			return value;
		}
	}
}