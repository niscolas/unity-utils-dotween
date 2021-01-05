using System;
using DG.Tweening;
using UnityAtoms.BaseAtoms;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#endif

namespace Plugins.DOTweenUtils {
	public enum TweenType {
		Transform,
		Fade
	}

	[Serializable]
	public class TweenAction {
		[SerializeField]
		private TweenType tweenType;

#if ODIN_INSPECTOR
		[ShowIf(nameof(IsTransformTween))]
#endif
		[SerializeField]
		private TransformOperation operation;

		[SerializeField]
		private Ease easeType;

		[SerializeField]
		private bool affectAllChildren;

		[Header("From / To Settings")]
		[SerializeField]
		private bool currentFrom;

		[SerializeField]
		private bool currentFromX;

		[SerializeField]
		private bool currentFromY;

		[SerializeField]
		private bool currentFromZ;

#if ODIN_INSPECTOR
		[HideIf(nameof(IsFadeTween))]
#endif
		[SerializeField]
		private bool incrementalTo;

#if ODIN_INSPECTOR
		[HideIf(nameof(currentFrom))]
		[HideIf(nameof(IsTransformTween))]
#endif
		[SerializeField]
		private float fadeFrom;

#if ODIN_INSPECTOR
		[HideIf(nameof(currentFrom))]
		[HideIf(nameof(IsFadeTween))]
#endif
		[SerializeField]
		private Vector3Reference fromVector;

#if ODIN_INSPECTOR
		[ShowIf(nameof(IsFadeTween))]
#endif
		[SerializeField]
		private float fadeTo;

#if ODIN_INSPECTOR
		[HideIf(nameof(IsFadeTween))]
#endif
		[SerializeField]
		private Vector3Reference toVector;

		[Header("Time Settings")]
		[SerializeField]
		private FloatReference duration;

		[SerializeField]
		private FloatReference delay;

		[SerializeField]
		private int loops;

		[SerializeField]
		private bool pingPong;

		[SerializeField]
		private bool unscaledTime;

		[Header("Other Settings")]
		[SerializeField]
		private bool setActiveAtStart;

		public bool AffectAllChildren => affectAllChildren;

		public bool SetActiveAtStart => setActiveAtStart;

		public float FadeFrom => fadeFrom;

		public float FadeTo => fadeTo;

		public TweenType TweenType => tweenType;

		public TransformOperation Operation => operation;

		public Ease EaseType => easeType;

		public Vector3 FromVector => fromVector;

		public bool CurrentFrom => currentFrom;

		public bool CurrentFromX => currentFromX;

		public bool CurrentFromY => currentFromY;

		public bool CurrentFromZ => currentFromZ;

		public Vector3 ToVector => toVector;

		public bool IncrementalTo => incrementalTo;

		public float Duration => duration;

		public float Delay => delay;

		public int Loops => loops;

		public bool PingPong => pingPong;

		public bool UnscaledTime => unscaledTime;

		private bool IsTransformTween => tweenType == TweenType.Transform;
		private bool IsFadeTween => tweenType == TweenType.Fade;
	}
}