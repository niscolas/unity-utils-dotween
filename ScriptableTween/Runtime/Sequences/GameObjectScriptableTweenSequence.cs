using UnityEngine;

namespace Plugins.ScriptableTween.Sequences
{
	[CreateAssetMenu(
		menuName = Constants.BaseAssetMenuPath + "(GameObject) => Scriptable Tween Sequence",
		order = Constants.AssetMenuOrder)]
	public class GameObjectScriptableTweenSequence : BaseScriptableTweenSequence<GameObject> { }
}