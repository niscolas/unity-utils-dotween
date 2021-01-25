using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lean.Pool;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Plugins.DOTweenUtils {
	[EditorIcon("atom-icon-purple")]
	public abstract class BaseScriptableTween : AtomAction<GameObject> {
		[SerializeField]
		protected Ease easeType;

		[Header("Time Settings")]
		[SerializeField]
		protected FloatReference duration;

		[SerializeField]
		protected FloatReference delay;

		[SerializeField]
		protected IntReference loops;

		[SerializeField]
		protected BoolReference isPingPong;

		[SerializeField]
		protected BoolReference useUnscaledTime;

		[Header("Other Settings")]
		[SerializeField]
		private BoolReference enableOnStart;

		[SerializeField]
		private BoolReference disableOnFinish;

		[SerializeField]
		private BoolReference destroyOnFinish;

		protected abstract UniTask Inner_DoAsync(GameObject target);

		public override async void Do(GameObject target) {
			await DoAsync(target);
		}

		public async UniTask DoAsync(GameObject target) {
			if (enableOnStart) {
				target.SetActive(true);
			}

			await Inner_DoAsync(target);

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
				.SetLoops(loops, isPingPong ? LoopType.Yoyo : LoopType.Incremental)
				.SetDelay(delay)
				.SetLink(target)
				.SetEase(easeType)
				.SetUpdate(useUnscaledTime);
		}
	}
}