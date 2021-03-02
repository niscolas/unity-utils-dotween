using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Plugins.ClassExtensions.CsharpExtensions;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using Random = System.Random;

namespace Plugins.DOTweenUtils.ScriptableTween.Tweens {
	[EditorIcon("atom-icon-purple")]
	public abstract class BaseScriptableTween<T> : AtomAction<T> {
		[Title("Tween")]
		[SerializeField]
		protected Ease[] possibleEaseTypes;

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		protected T fixedTarget;
		
		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		private FloatReference duration;

		[SerializeField]
		private bool randomizeDuration;
		
		[Range(0,1)]
		[SerializeField]
		private float durationRandomization;

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		private FloatReference delay;
		
		[SerializeField]
		private bool randomizeDelay;
		
		[Range(0,1)]
		[SerializeField]
		private float delayRandomization;

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		protected IntReference loops;

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		protected LoopType loopType;

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		protected BoolReference useUnscaledTime;

		[TabGroup("Main Settings", "Life Time")]
		[SerializeField]
		private bool autoKill = true;
		
		[TabGroup("From To", "To")]
		[SerializeField]
		private bool isRelative;

		private Ease CurrentEaseType => possibleEaseTypes.RandomElement();

		protected float CurrentDuration => GetNewValueFor(duration.Value, durationRandomization, randomizeDuration);
		protected  float CurrentDelay => GetNewValueFor(delay.Value, delayRandomization, randomizeDelay);


		protected abstract IEnumerable<Tween> GetTweens(T target);

		public override void Do() {
			Do(fixedTarget);
		}

		public override async void Do(T target) {
			await DoAsync(target);
		}

		public async UniTask DoAsync() {
			await DoAsync(fixedTarget);
		}

		public virtual async UniTask DoAsync(T target) {
			Tween[] tweens = GetTweens(target).ToArray();

			foreach (Tween tween in tweens) {
				ApplyDefaultOptions(tween, target);
			}

			IEnumerable<UniTask> tweenTasks = tweens.Select(tween => tween.AsyncWaitForCompletion().AsUniTask());

			await UniTask.WhenAll(tweenTasks);
		}

		protected virtual Tween ApplyDefaultOptions(Tween tween, T target) {
			return tween
				.SetLoops(loops, loopType)
				.SetAutoKill(autoKill)
				.SetDelay(CurrentDelay)
				.SetEase(possibleEaseTypes.RandomElement())
				.SetUpdate(useUnscaledTime)
				.SetRelative(isRelative);
		}

		private float GetNewValueFor(float value, float randomization, bool randomize) {	
			if (randomize) {
				return value.Randomize(randomization);
			}

			return value;
		}
	}
}