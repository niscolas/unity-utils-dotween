using System;
using System.Collections.Generic;
using DG.Tweening;
using Plugins.ClassExtensions.CsharpExtensions;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.DOTweenUtils {
	[EditorIcon("atom-icon-purple")]
	[CreateAssetMenu(
		menuName = ScriptableTweenSequence.BaseAssetMenuPath + "Scriptable Fade Tween",
		order = ScriptableTweenSequence.AssetMenuOrder)]
	public class ScriptableFadeTween : BaseScriptableTween {
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

		public override IEnumerable<Tween> GetTweens(GameObject target) {
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

			foreach (Tween tween in tweens) {
				ApplyDefaultOptions(tween, target);
			}

			return tweens;
		}

		private IEnumerable<Tween> GetTweens<T>(GameObject target, Func<T, Tween> tweenFunc) {
			List<Tween> tweens = new List<Tween>();
			if (!recursive) {
				T component = target.GetComponent<T>();
				if (component != null) {
					tweens.Add(tweenFunc?.Invoke(component));
				}

				return tweens;
			}

			T[] components = target.GetComponentsInChildren<T>(target);

			if (components.IsNullOrEmpty()) {
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
				.DOFade(fadeTo, duration)
				.From(useCurrentAlpha ? graphic.color.a : fadeFrom);
		}

		private Tween GetRenderersFadeTweens(Renderer renderer) {
			return renderer.material
				.DOFade(fadeTo, duration)
				.From(useCurrentAlpha ? renderer.material.color.a : fadeFrom);
		}

		private Tween GetSpriteRenderersFadeTweens(SpriteRenderer spriteRenderer) {
			return spriteRenderer
				.DOFade(fadeTo, duration)
				.From(useCurrentAlpha ? spriteRenderer.color.a : fadeFrom);
		}
	}
}