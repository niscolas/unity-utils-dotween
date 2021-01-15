using System.Threading.Tasks;
using DG.Tweening;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Plugins.DOTweenUtils {
	[EditorIcon("atom-icon-purple")]
	public abstract class ScriptableTweenBase : AtomAction<GameObject> {
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

		protected abstract Task Inner_Do(GameObject target);

		public override async void Do(GameObject target) {
			await DoAsync(target);
		}

		public async Task DoAsync(GameObject target) {
			if (enableOnStart) {
				target.SetActive(true);
			}

			await Inner_Do(target);

			if (destroyOnFinish) {
				Destroy(target);
			}
			else if (disableOnFinish) {
				target.SetActive(false);
			}
		}

		protected Tween ApplyDefaultOptions(Tween tween) {
			return tween
				.SetDelay(delay)
				.SetEase(easeType)
				.SetLoops(loops, isPingPong ? LoopType.Yoyo : LoopType.Incremental)
				.SetUpdate(useUnscaledTime);
		}
	}
}