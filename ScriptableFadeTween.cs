using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;
using static Plugins.DOTweenUtils.ScriptableTweenSequence;

namespace Plugins.DOTweenUtils {
	[EditorIcon("atom-icon-purple")]
	[CreateAssetMenu(menuName = BaseAssetMenuPath + "Scriptable Fade Tween", order = AssetMenuOrder)]
	public class ScriptableFadeTween : ScriptableTweenBase {
		[Header("Fade Settings")]
		[SerializeField]
		private bool useCurrentAlpha;

		[HideIf(nameof(useCurrentAlpha))]
		[SerializeField]
		private float fadeFrom;

		[SerializeField]
		private float fadeTo;

		protected override async Task Inner_Do(GameObject target) {
			List<Tween> tweens = new List<Tween>();
			tweens.AddRange(GetGraphicsFadeTweens(target));
			tweens.AddRange(GetRenderersFadeTweens(target));
			tweens.AddRange(GetSpriteRenderersFadeTweens(target));

			List<Task> tweenTasks = new List<Task>();
			foreach (Tween tween in tweens) {
				ApplyDefaultOptions(tween);
				tweenTasks.Add(tween.AsyncWaitForCompletion());
			}

			await Task.WhenAll(tweenTasks);
		}

		private IEnumerable<Tween> GetGraphicsFadeTweens(GameObject target) {
			Graphic[] graphics = target.GetComponentsInChildren<Graphic>();

			List<Tween> graphicsTweens = new List<Tween>();

			foreach (Graphic graphic in graphics) {
				Tween graphicTween =
					graphic
						.DOFade(fadeTo, duration)
						.From(useCurrentAlpha ? graphic.color.a : fadeFrom);

				graphicsTweens.Add(graphicTween);
			}

			return graphicsTweens;
		}

		private IEnumerable<Tween> GetRenderersFadeTweens(GameObject target) {
			Renderer[] renderers = target.GetComponentsInChildren<Renderer>();

			List<Tween> renderersTweens = new List<Tween>();

			foreach (Renderer renderer in renderers) {
				Tween rendererTween =
					renderer.material
						.DOFade(fadeTo, duration)
						.From(useCurrentAlpha ? renderer.material.color.a : fadeFrom);

				renderersTweens.Add(rendererTween);
			}

			return renderersTweens;
		}

		private IEnumerable<Tween> GetSpriteRenderersFadeTweens(GameObject target) {
			SpriteRenderer[] spriteRenderers = target.GetComponentsInChildren<SpriteRenderer>();

			List<Tween> spriteRenderersTweens = new List<Tween>();

			foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
				Tween rendererTween =
					spriteRenderer
						.DOFade(fadeTo, duration)
						.From(useCurrentAlpha ? spriteRenderer.color.a : fadeFrom);

				spriteRenderersTweens.Add(rendererTween);
			}

			return spriteRenderersTweens;
		}
	}
}