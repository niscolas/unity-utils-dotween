using UnityEngine;

namespace Plugins.DOTweenUtils.ScriptableTween.Sequences
{
	[CreateAssetMenu(
		menuName = Constants.BaseAssetMenuPath + "(GameObject) => Scriptable Tween Sequence",
		order = Constants.AssetMenuOrder)]
	public class GameObjectScriptableTweenSequence : BaseScriptableTweenSequence<GameObject> { }
}