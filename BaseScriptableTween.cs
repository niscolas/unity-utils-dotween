using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Plugins.DOTweenUtils {
	[EditorIcon("atom-icon-purple")]
	public abstract class BaseScriptableTween : AtomAction<GameObject> {
		[Title("Tween")]
		[SerializeField]
		protected Ease easeType;

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
		private BoolReference enableOnStart;

		[TabGroup("Main Settings", "Life Time")]
		[SerializeField]
		private BoolReference disableOnFinish;

		[TabGroup("Main Settings", "Life Time")]
		[SerializeField]
		private BoolReference destroyOnFinish;

		public abstract IEnumerable<Tween> GetTweens(GameObject target);

		public override async void Do(GameObject target) {
			await DoAsync(target);
		}

		public async UniTask DoAsync(GameObject target) {
			if (enableOnStart) {
				target.SetActive(true);
			}

			Sequence sequence = DOTween.Sequence();
			IEnumerable<Tween> tweens = GetTweens(target);
			foreach (Tween tween in tweens) {
				sequence.Append(tween);
			}

			await sequence.AsyncWaitForCompletion();

			if (destroyOnFinish) {
				Destroy(target);
			}
			else if (disableOnFinish) {
				if (LeanPool.Links.ContainsKey(target)) {
					LeanPool.Despawn(target);
				}
				else {
					target.SetActive(false);
				}
			}
		}

		protected virtual Tween ApplyDefaultOptions(Tween tween, GameObject target) {
			return tween
				.SetLoops(loops, loopType)
				.SetDelay(delay)
				.SetLink(target)
				.SetEase(easeType)
				.SetUpdate(useUnscaledTime);
		}
	}
}