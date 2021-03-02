using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Plugins.DOTweenUtils.ScriptableTween.Tweens {
	[EditorIcon("atom-icon-purple")]
	public abstract class BaseScriptableTween<T> : AtomAction<T> {
		[Title("Tween")]
		[SerializeField]
		protected Ease easeType;

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		protected T fixedTarget;
		
		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		protected FloatReference duration;

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		protected FloatReference delay;

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
				.SetDelay(delay)
				.SetEase(easeType)
				.SetUpdate(useUnscaledTime)
				.SetRelative(isRelative);
		}
	}
}