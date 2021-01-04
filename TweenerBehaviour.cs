using System;
using __Utils._ClassExtensions.LanguageExtensions;
using __Utils._ClassExtensions.UnityExtensions;
using __Utils.UnityUtils;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TweenType = __Utils.UnityUtils.TweenType;

namespace __Utils._Packages__PluginsUtils.DOTweenUtils {
	public class TweenerBehaviour : MonoBehaviour {
		[Header("Entry Settings")]
		[FormerlySerializedAs("tweeningPreset")]
		[SerializeField]
		private TweeningPreset entryTweeningPreset;

		[SerializeField]
		private bool executeOnAwake;

		[SerializeField]
		private bool executeOnEnable;

		[Header("Exit Settings")]
		[SerializeField]
		private TweeningPreset exitTweenPreset;

		[SerializeField]
		private bool executeOnDisable;

		public TweeningPreset EntryTweeningPreset {
			set => entryTweeningPreset = value;
		}

		public bool ExecuteOnAwake {
			get => executeOnAwake;
			set => executeOnAwake = value;
		}

		public bool ExecuteOnEnable {
			get => executeOnEnable;
			set => executeOnEnable = value;
		}

		public TweeningPreset ExitTweenPreset => exitTweenPreset;

		public bool ExecuteOnDisable => executeOnDisable;

		private void Awake() {
			if (executeOnAwake) {
				Play();
			}
		}

		private void OnEnable() {
			if (executeOnEnable) {
				Play();
			}
		}

		private void OnDisable() {
			transform.DOKill(true);
		}

		[Button]
		public void Play() {
			Play(null);
		}

		public void Play(Action callback) {
			PlayTweenPreset(entryTweeningPreset, callback);
		}

		public void SetTweenerActive(bool enable) {
			if (enable) {
				Play();
			}
			else {
				Disable();
			}
		}

		public void Disable() {
			if (exitTweenPreset) {
				PlayTweenPreset(exitTweenPreset);

				CoroutineRunner.Instance.DoAfterSeconds(
					() => {
						if (gameObject) {
							gameObject.SetActive(false);
						}
					},
					exitTweenPreset.Duration,
					exitTweenPreset.UnscaledTime
				);
			}
			else {
				gameObject.SetActive(false);
			}
		}

		public void PlayTweenPreset(TweeningPreset preset) {
			PlayTweenPreset(preset, null);
		}

		public void PlayTweenPreset(TweeningPreset preset, Action callback) {
			if (!preset) {
				return;
			}

			CoroutineRunner.Instance.DoAfterSeconds(
				() => {
					if (preset.DisableOnFinish) {
						gameObject.SetActive(false);
					}
					else if (preset.DestroyOnFinish) {
						Destroy(gameObject);
					}

					callback?.Invoke();
				},
				preset.Duration,
				preset.UnscaledTime
			);

			foreach (TweenAction tweenAction in preset) {
				switch (tweenAction.TweenType) {
					case TweenType.Transform:
						PlayTransformTween(tweenAction);
						break;

					case TweenType.Fade:
						PlayFadeTween(tweenAction);
						break;
				}
			}
		}

		private void PlayTransformTween(TweenAction tweenAction) {
			if (tweenAction.SetActiveAtStart) {
				gameObject.SetActive(true);
			}

			switch (tweenAction.Action) {
				case TransformAction.Position:
					PerformTranslation(tweenAction);
					break;

				case TransformAction.Rotation:
					PerformRotation(tweenAction);
					break;

				case TransformAction.Scale:
					PerformScaling(tweenAction);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void PlayFadeTween(TweenAction tweenAction) {
			Graphic[] graphics = GetComponentsInChildren<Graphic>();
			SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

			if (graphics.IsNullOrEmpty() && spriteRenderers.IsNullOrEmpty()) {
				return;
			}

			if (!tweenAction.AffectAllChildren) {
				graphics = new[] {graphics[0]};

				spriteRenderers = new[] {spriteRenderers[0]};
			}

			if (tweenAction.SetActiveAtStart) {
				gameObject.SetActive(true);
			}

			foreach (Graphic graphic in graphics) {
				if (!tweenAction.CurrentFrom) {
					Color tempColor = graphic.color;
					tempColor.a = tweenAction.FadeFrom;
					graphic.color = tempColor;
				}

				ApplyDefaultOptions(
					graphic.DOFade(tweenAction.FadeTo, tweenAction.Duration),
					tweenAction
				);
			}

			foreach (SpriteRenderer currentRenderer in spriteRenderers) {
				if (!tweenAction.CurrentFrom) {
					Color tempColor = currentRenderer.color;
					tempColor.a = tweenAction.FadeFrom;
					currentRenderer.color = tempColor;
				}

				ApplyDefaultOptions(
					currentRenderer.DOFade(tweenAction.FadeTo, tweenAction.Duration),
					tweenAction
				);
			}
		}

		private void PerformTranslation(TweenAction tweenAction) {
			if (!tweenAction.CurrentFrom) {
				transform.position = GetFromVector(tweenAction);
			}

			ApplyDefaultOptions(
				transform.DOLocalMove(GetFinalTo(tweenAction), tweenAction.Duration),
				tweenAction
			);
		}

		private void PerformRotation(TweenAction tweenAction) {
			if (!tweenAction.CurrentFrom) {
				transform.rotation = Quaternion.Euler(tweenAction.FromVector);
			}

			ApplyDefaultOptions(
				transform.DORotate(GetFinalTo(tweenAction), tweenAction.Duration),
				tweenAction
			);
		}

		private void PerformScaling(TweenAction tweenAction) {
			if (!tweenAction.CurrentFrom) {
				transform.localScale = tweenAction.FromVector;
			}

			ApplyDefaultOptions(
				transform.DOScale(GetFinalTo(tweenAction), tweenAction.Duration),
				tweenAction);
		}

		private Vector3 GetFromVector(TweenAction tweenAction) {
			Vector3 currentPosition = transform.position;

			float x =
				tweenAction.CurrentFrom || tweenAction.CurrentFromX
					? currentPosition.x
					: tweenAction.FromVector.x;

			float y =
				tweenAction.CurrentFrom || tweenAction.CurrentFromY
					? currentPosition.y
					: tweenAction.FromVector.y;

			float z =
				tweenAction.CurrentFrom || tweenAction.CurrentFromZ
					? currentPosition.z
					: tweenAction.FromVector.z;

			return new Vector3(x, y, z);
		}

		private Vector3 GetFinalTo(TweenAction tweenAction) {
			if (!tweenAction.IncrementalTo) {
				return tweenAction.ToVector;
			}

			Vector3 currentPos;

			switch (tweenAction.Action) {
				case TransformAction.Position:
					currentPos = tweenAction.IncrementalTo
						? transform.localPosition
						: transform.position;

					break;

				case TransformAction.Rotation:
					currentPos = transform.rotation.eulerAngles;
					break;

				case TransformAction.Scale:
					currentPos = transform.localScale;
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			return currentPos + tweenAction.ToVector;
		}

		private void ApplyDefaultOptions<T, T2, TOpts>(TweenerCore<T, T2, TOpts> tweener, TweenAction tweenAction)
			where TOpts : struct, IPlugOptions {
			tweener
				.SetDelay(tweenAction.Delay)
				.SetEase(tweenAction.EaseType)
				.SetLoops(tweenAction.Loops, tweenAction.PingPong ? LoopType.Yoyo : LoopType.Incremental)
				.SetUpdate(tweenAction.UnscaledTime);
		}
	}
}