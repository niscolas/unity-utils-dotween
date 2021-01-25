using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Plugins.ClassExtensions.CsharpExtensions;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;
using static Plugins.DOTweenUtils.ScriptableTweenSequence;

namespace Plugins.DOTweenUtils {
	[EditorIcon("atom-icon-purple")]
	[CreateAssetMenu(menuName = BaseAssetMenuPath + "Scriptable Fade Tween", order = AssetMenuOrder)]
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

		protected override async UniTask Inner_DoAsync(GameObject target) {
			List<Tween> tweens = new List<Tween>();
			tweens.AddRange(GetGraphicsFadeTweens(target));
			tweens.AddRange(GetRenderersFadeTweens(target));
			tweens.AddRange(GetSpriteRenderersFadeTweens(target));

			List<Task> tweenTasks = new List<Task>();
			foreach (Tween tween in tweens) {
				ApplyDefaultOptions(tween, target);
				tweenTasks.Add(tween.AsyncWaitForCompletion());
			}

			await Task.WhenAll(tweenTasks);
		}

		private IEnumerable<Tween> GetGraphicsFadeTweens(GameObject target) {
			Graphic[] graphics = GetComponentsFiltered<Graphic>(target);

			List<Tween> graphicsTweens = new List<Tween>();
			if (graphics.IsNullOrEmpty()) {
				return graphicsTweens;
			}

			foreach (Graphic graphic in graphics) {
				if (!graphic) {
					continue;
				}

				Tween graphicTween =
					graphic
						.DOFade(fadeTo, duration)
						.From(useCurrentAlpha ? graphic.color.a : fadeFrom);

				graphicsTweens.Add(graphicTween);
			}

			return graphicsTweens;
		}

		private IEnumerable<Tween> GetRenderersFadeTweens(GameObject target) {
			Renderer[] renderers = GetComponentsFiltered<Renderer>(target);

			List<Tween> renderersTweens = new List<Tween>();
			if (renderers.IsNullOrEmpty()) {
				return renderersTweens;
			}

			foreach (Renderer renderer in renderers) {
				if (!renderer) {
					continue;
				}

				Tween rendererTween =
					renderer.material
						.DOFade(fadeTo, duration)
						.From(useCurrentAlpha ? renderer.material.color.a : fadeFrom);

				renderersTweens.Add(rendererTween);
			}

			return renderersTweens;
		}

		private IEnumerable<Tween> GetSpriteRenderersFadeTweens(GameObject target) {
			SpriteRenderer[] spriteRenderers = GetComponentsFiltered<SpriteRenderer>(target);

			List<Tween> spriteRenderersTweens = new List<Tween>();
			if (spriteRenderers.IsNullOrEmpty()) {
				return spriteRenderersTweens;
			}

			foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
				if (!spriteRenderer) {
					continue;
				}

				Tween rendererTween =
					spriteRenderer
						.DOFade(fadeTo, duration)
						.From(useCurrentAlpha ? spriteRenderer.color.a : fadeFrom);

				spriteRenderersTweens.Add(rendererTween);
			}

			return spriteRenderersTweens;
		}

		private T[] GetComponentsFiltered<T>(GameObject target) where T : Component {
			if (recursive) {
				return target.GetComponentsInChildren<T>();
			}

			T component = target.GetComponent<T>();
			return component ? new[] {component} : null;
		}
	}
}