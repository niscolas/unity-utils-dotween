using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableTween.Tweens.GameObject
{
	[EditorIcon("atom-icon-purple")]
	[CreateAssetMenu(
		menuName = Constants.BaseAssetMenuPath + "Scriptable Fade Tween",
		order = Constants.AssetMenuOrder)]
	public class ScriptableFadeTween : ScriptableGameObjectTween
	{
		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		private bool recursive = true;

		[TabGroup("From To", "From")]
		[SerializeField]
		private bool useCurrentAlpha;

		[TabGroup("From To", "From")]
		[HideIf(nameof(useCurrentAlpha))]
		[LabelText("From")]
		[SerializeField]
		private float fadeFrom;

		[TabGroup("From To", "To")]
		[LabelText("To")]
		[SerializeField]
		private float fadeTo;

		[TabGroup("Main Settings", "Basic")]
		[Title("", "Targets")]
		[SerializeField]
		private bool affectGraphics = true;

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		private bool affectRenderers = true;

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		private bool affectSpriteRenderers = true;

		[TabGroup("Main Settings", "Basic")]
		[SerializeField]
		private bool _affectCanvasGroups = true;

		public override IEnumerable<Tween> GetTweens(UnityEngine.GameObject target)
		{
			List<Tween> tweens = new List<Tween>();
			if (affectGraphics)
			{
				tweens.AddRange(GetTweens<Graphic>(target, GetGraphicsFadeTweens));
			}

			if (affectRenderers)
			{
				tweens.AddRange(GetTweens<Renderer>(target, GetRenderersFadeTweens));
			}

			if (affectSpriteRenderers)
			{
				tweens.AddRange(GetTweens<SpriteRenderer>(target, GetSpriteRenderersFadeTweens));
			}

			if (_affectCanvasGroups)
			{
				tweens.AddRange(GetTweens<CanvasGroup>(target, GetCanvasGroupsFadeTweens));
			}

			return tweens;
		}

		private IEnumerable<Tween> GetTweens<T>(UnityEngine.GameObject target, Func<T, Tween> tweenProvider)
		{
			List<Tween> tweens = new List<Tween>();
			if (!recursive)
			{
				T component = target.GetComponent<T>();
				if (component != null)
				{
					tweens.Add(tweenProvider?.Invoke(component));
				}

				return tweens;
			}

			T[] components = target.GetComponentsInChildren<T>(target);

			if (components == null || components.Length == 0)
			{
				return tweens;
			}

			foreach (T component in components)
			{
				if (component == null) continue;

				Tween tween = tweenProvider?.Invoke(component);
				tweens.Add(tween);
			}

			return tweens;
		}

		private Tween GetGraphicsFadeTweens(Graphic graphic)
		{
			return graphic
				.DOFade(fadeTo, CurrentDuration)
				.From(useCurrentAlpha ? graphic.color.a : fadeFrom);
		}

		private Tween GetRenderersFadeTweens(Renderer renderer)
		{
			return renderer.material
				.DOFade(fadeTo, CurrentDuration)
				.From(useCurrentAlpha ? renderer.material.color.a : fadeFrom);
		}

		private Tween GetSpriteRenderersFadeTweens(SpriteRenderer spriteRenderer)
		{
			return spriteRenderer
				.DOFade(fadeTo, CurrentDuration)
				.From(useCurrentAlpha ? spriteRenderer.color.a : fadeFrom);
		}

		private Tween GetCanvasGroupsFadeTweens(CanvasGroup canvasGroup)
		{
			return canvasGroup
				.DOFade(fadeTo, CurrentDuration)
				.From(useCurrentAlpha ? canvasGroup.alpha : fadeFrom);
		}
	}
}