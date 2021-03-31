using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Plugins.ScriptableTween.Tweens
{
	[EditorIcon("atom-icon-purple")]
	[CreateAssetMenu(
		menuName = Constants.BaseAssetMenuPath + "Scriptable Material Tween",
		order = Constants.AssetMenuOrder)]
	public class ScriptableMaterialTween : BaseScriptableTween<Material>
	{
		[SerializeField]
		private StringReference propertyName;

		[SerializeField]
		private MaterialTweenType tweenType;

		[ShowIf(nameof(IsDOFloatTween))]
		[SerializeField]
		private FloatReference endFloatValue;

		[ShowIf(nameof(IsDOColorTween))]
		[SerializeField]
		private ColorReference endColorValue;

		private bool IsDOFloatTween => tweenType == MaterialTweenType.DOFloat;

		private bool IsDOColorTween => tweenType == MaterialTweenType.DOColor;
		// private bool IsDOGradientTween => tweenType == MaterialTweenType.DOGradient;

		public override IEnumerable<Tween> GetTweens(Material target)
		{
			Tween tween = null;

			switch (tweenType)
			{
				case MaterialTweenType.DOFloat:
					tween = target.DOFloat(endFloatValue, propertyName, CurrentDuration);
					break;

				case MaterialTweenType.DOColor:
					tween = target.DOColor(endColorValue, propertyName, CurrentDuration);
					break;
			}

			return new[] {tween};
		}

		private enum MaterialTweenType
		{
			DOFloat,

			DOColor
			// DOGradient
		}
	}
}