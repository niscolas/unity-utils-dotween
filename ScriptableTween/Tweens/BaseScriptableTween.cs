using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Plugins.ClassExtensions.CsharpExtensions;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Plugins.DOTweenUtils.ScriptableTween.Tweens
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

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		protected Ease[] possibleEaseTypes;

		[TabGroup("Main Settings", "Duration")]
		[SerializeField]
		private FloatReference duration;

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
		protected float CurrentDelay => GetNewValueFor(delay.Value, delayRandomization, randomizeDelay);

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
			Tween[] tweens = GetTweens(target).ToArray();

			foreach (Tween tween in tweens)
			{
				ApplyDefaultOptions(tween, target);
			}

			IEnumerable<UniTask> tweenTasks = tweens.Select(tween => tween.AsyncWaitForCompletion().AsUniTask());

			await UniTask.WhenAll(tweenTasks);
		}

		protected virtual Tween ApplyDefaultOptions(Tween tween, T target)
		{
			return tween
				.SetLoops(loops, loopType)
				.SetAutoKill(autoKill)
				.SetDelay(CurrentDelay)
				.SetEase(CurrentEaseType)
				.SetUpdate(useUnscaledTime)
				.SetRelative(isRelative);
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