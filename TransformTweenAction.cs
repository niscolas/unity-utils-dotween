using System.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Plugins.DOTweenUtils {
	public class TransformTweenAction : TweenActionBase {
		[SerializeField]
		private TransformOperation operation;

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

		[SerializeField]
		private bool incrementalTo;

		[SerializeField]
		private Vector3Reference toVector;

		public Vector3Reference FromVector {
			get => fromVector;
			set => fromVector = value;
		}

		public Vector3Reference ToVector {
			get => toVector;
			set => toVector = value;
		}

		protected override async Task Inner_PlayTweenOn(GameObject target) {
			Tween transformTween = operation switch {
				TransformOperation.Position => PerformTranslation(target),
				TransformOperation.Rotation => PerformRotation(target),
				TransformOperation.Scale => PerformScaling(target),
				_ => PerformTranslation(target)
			};

			await transformTween.AsyncWaitForCompletion();
		}

		private Tween PerformTranslation(GameObject target) {
			target.transform.position = GetFromPosition(target);

			Tween translationTween = target.transform.DOLocalMove(GetFinalTo(target), duration);

			return ApplyDefaultOptions(translationTween);
		}

		private Tween PerformRotation(GameObject target) {
			target.transform.rotation = Quaternion.Euler(GetFromRotation(target));

			Tween rotationTween = target.transform.DORotate(GetFinalTo(target), duration);

			return ApplyDefaultOptions(rotationTween);
		}

		private Tween PerformScaling(GameObject target) {
			target.transform.localScale = GetFromScale(target);

			Tween scalingTween = target.transform.DOScale(GetFinalTo(target), duration);

			return ApplyDefaultOptions(scalingTween);
		}

		private Vector3 GetFromPosition(GameObject target) {
			Vector3 currentPosition = target.transform.position;

			return FilterFromVector(currentPosition);
		}

		private Vector3 GetFromRotation(GameObject target) {
			Quaternion currentRotation = target.transform.rotation;

			return FilterFromVector(currentRotation.eulerAngles);
		}

		private Vector3 GetFromScale(GameObject target) {
			Vector3 currentScale = target.transform.localScale;

			return FilterFromVector(currentScale);
		}

		private Vector3 FilterFromVector(Vector3 currentFrom) {
			float x = useCurrentX ? currentFrom.x : fromVector.Value.x;
			float y = useCurrentY ? currentFrom.y : fromVector.Value.y;
			float z = useCurrentZ ? currentFrom.z : fromVector.Value.z;

			return new Vector3(x, y, z);
		}

		private Vector3 GetFinalTo(GameObject target) {
			if (!incrementalTo) {
				return toVector;
			}

			Vector3 currentPos = target.transform.position;

			switch (operation) {
				case TransformOperation.Position:
					currentPos = incrementalTo ? target.transform.localPosition : target.transform.position;
					break;

				case TransformOperation.Rotation:
					currentPos = target.transform.rotation.eulerAngles;
					break;

				case TransformOperation.Scale:
					currentPos = target.transform.localScale;
					break;
			}

			return currentPos + toVector;
		}
	}
}