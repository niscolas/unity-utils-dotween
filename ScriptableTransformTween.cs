using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using static Plugins.DOTweenUtils.ScriptableTweenSequence;

namespace Plugins.DOTweenUtils {
	[EditorIcon("atom-icon-purple")]
	[CreateAssetMenu(menuName = BaseAssetMenuPath + "Scriptable Transform Tween", order = AssetMenuOrder)]
	public class ScriptableTransformTween : BaseScriptableTween {
		[Title("Transform Tween")]
		[SerializeField]
		private TransformOperation operation;

		[TabGroup("From To", "From"), BoxGroup("From To/From/Current From")]
		[HorizontalGroup("From To/From/Current From/XYZ", LabelWidth = 10)]
		[VerticalGroup("From To/From/Current From/XYZ/X")]
		[LabelText("X")]
		[SerializeField]
		private bool useCurrentX;

		[TabGroup("From To", "From"), BoxGroup("From To/From/Current From")]
		[VerticalGroup("From To/From/Current From/XYZ/Y")]
		[LabelText("Y")]
		[SerializeField]
		private bool useCurrentY;

		[TabGroup("From To", "From"), BoxGroup("From To/From/Current From")]
		[VerticalGroup("From To/From/Current From/XYZ/Z")]
		[LabelText("Z")]
		[SerializeField]
		private bool useCurrentZ;

		[TabGroup("From To", "From"), BoxGroup("From To/From/Current From")]
		[LabelText("With Offset")]
		[ShowIf(nameof(UseCurrentXYZ))]
		[Title("Offset From Current")]
		[HideLabel]
		[SerializeField]
		private Vector3Reference offsetFromCurrent;

		[TabGroup("From To", "From")]
		[BoxGroup("From To/From/Fixed From")]
		[DisableIf(nameof(UseCurrentXYZ))]
		[HideLabel]
		[SerializeField]
		private Vector3Reference fromVector;

		[TabGroup("From To", "To")]
		[SerializeField]
		private bool incrementalTo;

		[TabGroup("From To", "To")]
		[SerializeField]
		private Vector3Reference toVector;

		private bool UseCurrentXYZ => useCurrentX && useCurrentY && useCurrentZ;

		public ScriptableTransformTween WithDynamicTo(Vector3 to) {
			ScriptableTransformTween dynamicToVectorTween = Instantiate(this);
			dynamicToVectorTween.toVector.Value = to;
			return dynamicToVectorTween;
		}

		public override IEnumerable<Tween> GetTweens(GameObject target) {
			Tween transformTween;
			switch (operation) {
				case TransformOperation.Position:
					transformTween = PerformTranslation(target);
					break;
				
				case TransformOperation.Rotation:
					transformTween = PerformRotation(target);
					break;
				
				case TransformOperation.Scale:
					transformTween = PerformScaling(target);
					break;
				
				default:
					transformTween = PerformTranslation(target);
					break;
			}

			return new[] {transformTween};
		}

		protected override Tween ApplyDefaultOptions(Tween tween, GameObject target) {
			return base.ApplyDefaultOptions(tween, target).SetRelative(incrementalTo);
		}

		private Tween PerformTranslation(GameObject target) {
			target.transform.position = GetFromPosition(target);

			Tween translationTween = target.transform.DOMove(toVector, duration);

			return ApplyDefaultOptions(translationTween, target);
		}

		private Tween PerformRotation(GameObject target) {
			target.transform.rotation = Quaternion.Euler(GetFromRotation(target));

			Tween rotationTween = target.transform.DORotate(toVector, duration);

			return ApplyDefaultOptions(rotationTween, target);
		}

		private Tween PerformScaling(GameObject target) {
			target.transform.localScale = GetFromScale(target);

			Tween scalingTween = target.transform.DOScale(toVector, duration);

			return ApplyDefaultOptions(scalingTween, target);
		}

		private Vector3 GetFromPosition(GameObject target) {
			Vector3 currentPosition = target.transform.position;

			return GetFromVector(currentPosition);
		}

		private Vector3 GetFromRotation(GameObject target) {
			Quaternion currentRotation = target.transform.rotation;

			return GetFromVector(currentRotation.eulerAngles);
		}

		private Vector3 GetFromScale(GameObject target) {
			Vector3 currentScale = target.transform.localScale;

			return GetFromVector(currentScale);
		}

		private Vector3 GetFromVector(Vector3 currentFrom) {
			float x = useCurrentX ? currentFrom.x + offsetFromCurrent.Value.x : fromVector.Value.x;
			float y = useCurrentY ? currentFrom.y + offsetFromCurrent.Value.y : fromVector.Value.y;
			float z = useCurrentZ ? currentFrom.z + offsetFromCurrent.Value.z : fromVector.Value.z;

			return new Vector3(x, y, z);
		}
	}
}