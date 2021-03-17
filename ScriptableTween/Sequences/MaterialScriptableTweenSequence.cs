using UnityEngine;

namespace Plugins.DOTweenUtils.ScriptableTween.Sequences
{
	[CreateAssetMenu(
		menuName = Constants.BaseAssetMenuPath + "(Material) => Scriptable Tween Sequence",
		order = Constants.AssetMenuOrder)]
	public class MaterialScriptableTweenSequence : BaseScriptableTweenSequence<Material> { }
}