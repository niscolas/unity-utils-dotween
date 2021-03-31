using System.Collections.Generic;
using DG.Tweening;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Plugins.DOTweenUtils.ScriptableTween.Tweens.GameObject
{
	[EditorIcon("atom-icon-purple")]
	[CreateAssetMenu(
		menuName = Constants.BaseAssetMenuPath + "Scriptable Shake Tween",
		order = Constants.AssetMenuOrder)]
	public class ScriptableShakeTween : ScriptableGameObjectTween
	{
		[SerializeField]
		private TransformTweenType _shakeType;

		[SerializeField]
		private Vector3Reference _strength;

		[SerializeField]
		private IntReference _vibrato;

		[SerializeField]
		private FloatReference _randomness;

		[SerializeField]
		private BoolReference _fadeOut;

		public override IEnumerable<Tween> GetTweens(UnityEngine.GameObject target)
		{
			Tween tween;

			Transform targetTransform = target.transform;
			switch (_shakeType)
			{
				case TransformTweenType.Position:
					tween = PerformShakePosition(targetTransform);
					break;

				case TransformTweenType.Rotation:
					tween = PerformShakeRotation(targetTransform);
					break;

				case TransformTweenType.Scale:
					tween = PerformShakeScale(targetTransform);
					break;

				default:
					tween = PerformShakeRotation(targetTransform);
					break;
			}

			return new[] {tween};
		}

		private Tween PerformShakePosition(Transform transform)
		{
			Tween tween = transform.DOShakePosition(
				CurrentDuration,
				_strength.Value,
				_vibrato.Value,
				_randomness.Value,
				_fadeOut.Value);

			return tween;
		}

		private Tween PerformShakeRotation(Transform transform)
		{
			Tween tween = transform.DOShakeRotation(
				CurrentDuration,
				_strength.Value,
				_vibrato.Value,
				_randomness.Value,
				_fadeOut.Value);

			return tween;
		}

		private Tween PerformShakeScale(Transform transform)
		{
			Tween tween = transform.DOShakeScale(
				CurrentDuration,
				_strength.Value,
				_vibrato.Value,
				_randomness.Value,
				_fadeOut.Value);

			return tween;
		}
	}
}