using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

namespace Plugins.DOTweenUtils.ScriptableTween.Tweens.GameObject {
	public abstract class GameObjectScriptableTween : BaseScriptableTween<UnityEngine.GameObject> {
		[TabGroup("Main Settings", "Life Time")]
		[SerializeField]
		private LinkBehaviour linkBehaviour = LinkBehaviour.CompleteAndKillOnDisable;

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

		public override async UniTask DoAsync(UnityEngine.GameObject target) {
			if (enableTargetOnStart) {
				target.SetActive(true);
			}

			await base.DoAsync(target);

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

		protected override Tween ApplyDefaultOptions(Tween tween, UnityEngine.GameObject target) {
			return base
				.ApplyDefaultOptions(tween, target)
				.SetLink(target, linkBehaviour);
		}
	}
}