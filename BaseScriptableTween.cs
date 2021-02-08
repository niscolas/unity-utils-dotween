using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

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
		private LinkBehaviour linkBehaviour = LinkBehaviour.CompleteAndKillOnDisable;

		[TabGroup("Main Settings", "Life Time")]
		[SerializeField]
		private bool autoKill = true;
		
		[TabGroup("Main Settings", "Life Time")]
		[FormerlySerializedAs("enableOnStart")]
		[SerializeField]
		private BoolReference enableTargetOnStart;

		[TabGroup("Main Settings", "Life Time")]
		[FormerlySerializedAs("disableOnFinish")]
		[SerializeField]
		private BoolReference disableTargetOnFinish;

		[TabGroup("Main Settings", "Life Time")]
		[FormerlySerializedAs("destroyOnFinish")]
		[SerializeField]
		private BoolReference destroyTargetOnFinish;

		public abstract IEnumerable<Tween> GetTweens(GameObject target);

		public override async void Do(GameObject target) {
			await DoAsync(target);
		}

		public async UniTask DoAsync(GameObject target) {
			if (enableTargetOnStart) {
				target.SetActive(true);
			}

			IEnumerable<Tween> tweens = GetTweens(target);
			IEnumerable<UniTask> tweenTasks = tweens.Select(tween => tween.AsyncWaitForCompletion().AsUniTask());

			await UniTask.WhenAll(tweenTasks);

			if (destroyTargetOnFinish) {
				Destroy(target);
			}
			else if (disableTargetOnFinish) {
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
				.SetAutoKill(autoKill)
				.SetDelay(delay)
				.SetLink(target, linkBehaviour)
				.SetEase(easeType)
				.SetUpdate(useUnscaledTime);
		}
	}
}