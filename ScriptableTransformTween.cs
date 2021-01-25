using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using static Plugins.DOTweenUtils.ScriptableTweenSequence;

namespace Plugins.DOTweenUtils {
	[EditorIcon("atom-icon-purple")]
	[CreateAssetMenu(menuName = BaseAssetMenuPath + "Scriptable Transform Tween", order = AssetMenuOrder)]
	public class ScriptableTransformTween : BaseScriptableTween {
		[Header("Transform")]
		[SerializeField]
		private TransformOperation operation;

		[Header("Transform / From")]
		[SerializeField]
		private bool useCurrentX;

		[SerializeField]
		private bool useCurrentY;

		[SerializeField]
		private bool useCurrentZ;

		[HideIf(
			EConditionOperator.And,
			nameof(useCurrentX),
			nameof(useCurrentY),
			nameof(useCurrentZ))]
		[SerializeField]
		private Vector3Reference fromVector;

		[ShowIf(EConditionOperator.Or,
			nameof(useCurrentX),
			nameof(useCurrentY),
			nameof(useCurrentZ))]
		[SerializeField]
		private Vector3Reference offsetFromCurrent;

		[Header("Transform / To")]
		[SerializeField]
		private bool incrementalTo;

		[SerializeField]
		private Vector3Reference toVector;

		public ScriptableTransformTween WithDynamicTo(Vector3 to) {
			ScriptableTransformTween dynamicToVectorTween = Instantiate(this);
			dynamicToVectorTween.toVector.Value = to;
			return dynamicToVectorTween;
		}

		protected override async UniTask Inner_DoAsync(GameObject target) {
			Tween transformTween = operation switch {
				TransformOperation.Position => PerformTranslation(target),
				TransformOperation.Rotation => PerformRotation(target),
				TransformOperation.Scale => PerformScaling(target),
				_ => PerformTranslation(target)
			};

			await transformTween.AsyncWaitForCompletion();
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