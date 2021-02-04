using DG.Tweening;
using UnityAtoms;
using UnityEngine;
using static Plugins.DOTweenUtils.ScriptableTweenSequence;

namespace Plugins.DOTweenUtils {
	[EditorIcon("atom-icon-purple")]
	[CreateAssetMenu(menuName = BaseAssetMenuPath + "Destroy Tween", order = AssetMenuOrder)]
	public class KillTweens : AtomAction<GameObject> {
		[SerializeField]
		private bool complete = true;

		public override void Do(GameObject target) {
			if (!target) {
				return;
			}

			target.transform.DOKill(complete);
		}
	}
}