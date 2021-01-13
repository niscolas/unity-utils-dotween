using System.Threading.Tasks;
using DG.Tweening;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Plugins.DOTweenUtils {
	public abstract class TweenActionBase : ScriptableObject {
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

		public async Task PlayTweenOn(GameObject target) {
			if (enableOnStart) {
				target.SetActive(true);
			}

			await Inner_PlayTweenOn(target);

			if (destroyOnFinish) {
				Destroy(target);
			}
			else if (disableOnFinish) {
				target.SetActive(false);
			}
		}

		protected abstract Task Inner_PlayTweenOn(GameObject target);

		protected Tween ApplyDefaultOptions(Tween tween) {
			return tween
				.SetDelay(delay)
				.SetEase(easeType)
				.SetLoops(loops, isPingPong ? LoopType.Yoyo : LoopType.Incremental)
				.SetUpdate(useUnscaledTime);
		}
	}
}