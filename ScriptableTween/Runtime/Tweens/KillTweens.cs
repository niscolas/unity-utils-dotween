using DG.Tweening;
using UnityAtoms;
using UnityEngine;

namespace Plugins.ScriptableTween.Tweens
{
	[EditorIcon("atom-icon-purple")]
	[CreateAssetMenu(
		menuName = Constants.BaseAssetMenuPath + "Destroy Tween",
		order = Constants.AssetMenuOrder)]
	public class KillTweens : AtomAction<UnityEngine.GameObject>
	{
		[SerializeField]
		private bool complete = true;

		public override void Do(UnityEngine.GameObject target)
		{
			if (!target)
			{
				return;
			}

			target.transform.DOKill(complete);
		}
	}
}