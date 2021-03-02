using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.DOTweenUtils.ScriptableTween.Tweens.GameObject {
	[EditorIcon("atom-icon-purple")]
	[CreateAssetMenu(
		menuName = Constants.BaseAssetMenuPath + "Scriptable Fade Tween",
		order = Constants.AssetMenuOrder)]
	public class ScriptableFadeTween : GameObjectScriptableTween {
		[Header("Fade Settings")]
		[SerializeField]
		private bool recursive = true;

		[SerializeField]
		private bool useCurrentAlpha;

		[HideIf(nameof(useCurrentAlpha))]
		[SerializeField]
		private float fadeFrom;

		[SerializeField]
		private float fadeTo;

		[Header("Fade Settings / Targets")]
		[SerializeField]
		private bool affectGraphics = true;

		[SerializeField]
		private bool affectRenderers = true;

		[SerializeField]
		private bool affectSpriteRenderers = true;

		protected override IEnumerable<Tween> GetTweens(UnityEngine.GameObject target) {
			List<Tween> tweens = new List<Tween>();
			if (affectGraphics) {
				tweens.AddRange(GetTweens<Graphic>(target, GetGraphicsFadeTweens));
			}

			if (affectRenderers) {
				tweens.AddRange(GetTweens<Renderer>(target, GetRenderersFadeTweens));
			}

			if (affectSpriteRenderers) {
				tweens.AddRange(GetTweens<SpriteRenderer>(target, GetSpriteRenderersFadeTweens));
			}

			return tweens;
		}

		private IEnumerable<Tween> GetTweens<T>(UnityEngine.GameObject target, Func<T, Tween> tweenFunc) {
			List<Tween> tweens = new List<Tween>();
			if (!recursive) {
				T component = target.GetComponent<T>();
				if (component != null) {
					tweens.Add(tweenFunc?.Invoke(component));
				}

				return tweens;
			}

			T[] components = target.GetComponentsInChildren<T>(target);

			if (components == null || components.Length == 0) {
				return tweens;
			}

			foreach (T component in components) {
				if (component == null) {
					continue;
				}

				Tween tween = tweenFunc?.Invoke(component);
				tweens.Add(tween);
			}

			return tweens;
		}

		private Tween GetGraphicsFadeTweens(Graphic graphic) {
			return graphic
				.DOFade(fadeTo, CurrentDuration)
				.From(useCurrentAlpha ? graphic.color.a : fadeFrom);
		}

		private Tween GetRenderersFadeTweens(Renderer renderer) {
			return renderer.material
				.DOFade(fadeTo, CurrentDuration)
				.From(useCurrentAlpha ? renderer.material.color.a : fadeFrom);
		}

		private Tween GetSpriteRenderersFadeTweens(SpriteRenderer spriteRenderer) {
			return spriteRenderer
				.DOFade(fadeTo, CurrentDuration)
				.From(useCurrentAlpha ? spriteRenderer.color.a : fadeFrom);
		}
	}
}