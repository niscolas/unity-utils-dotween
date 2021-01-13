using System.Collections.Generic;
using System.Threading.Tasks;
using __Utils._ClassExtensions.LanguageExtensions;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.DOTweenUtils {
	public class FadeTweenAction : TweenActionBase {
		[Header("Fade Settings")]
		[SerializeField]
		private bool useCurrentAlpha;

		[HideIf(nameof(useCurrentAlpha))]
		[SerializeField]
		private float fadeFrom;

		[SerializeField]
		private float toAlpha;

		protected override async Task Inner_PlayTweenOn(GameObject target) {
			Graphic[] graphics = target.GetComponentsInChildren<Graphic>();
			Renderer[] renderers = target.GetComponentsInChildren<Renderer>();

			if (graphics.IsNullOrEmpty() && renderers.IsNullOrEmpty()) {
				return;
			}

			List<Tween> tweens = new List<Tween>();

			foreach (Graphic graphic in graphics) {
				Tween graphicTween =
					graphic
						.DOFade(toAlpha, duration)
						.From(useCurrentAlpha ? graphic.color.a : fadeFrom);

				tweens.Add(graphicTween);
			}

			foreach (Renderer renderer in renderers) {
				Tween rendererTween =
					renderer.material
						.DOFade(toAlpha, duration)
						.From(useCurrentAlpha ? renderer.material.color.a : fadeFrom);

				tweens.Add(rendererTween);
			}

			List<Task> tweenTasks = new List<Task>();

			foreach (Tween tween in tweens) {
				tweenTasks.Add(tween.AsyncWaitForCompletion());
				ApplyDefaultOptions(tween);
			}

			await Task.WhenAll(tweenTasks);
		}
	}
}