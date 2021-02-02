using System.Collections.Generic;
using DG.DOTweenEditor;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Plugins.DOTweenUtils.Editor {
	public class TweenPreviewer : OdinEditorWindow {
		[MenuItem("Tools/Tween Previewer")]
		private static void OpenWindow() {
			GetWindow<TweenPreviewer>().Show();
		}

		[SceneObjectsOnly]
		[SerializeField]
		private GameObject target;

		[Space(15)]
		[SerializeField]
		private BaseScriptableTween scriptableTween;

		[HorizontalGroup]
		[Button(ButtonSizes.Large, ButtonStyle.Box)]
		[GUIColor("@UnityEngine.Color.green")]
		public void Play() {
			IEnumerable<Tween> tweens = scriptableTween.GetTweens(target);
			foreach (Tween tween in tweens) {
				DOTweenEditorPreview.PrepareTweenForPreview(tween);
			}

			DOTweenEditorPreview.Start();
		}

		[HorizontalGroup]
		[Button(ButtonSizes.Large, ButtonStyle.Box)]
		[GUIColor("@UnityEngine.Color.red")]
		public void Stop() {
			DOTweenEditorPreview.Stop(true);
		}
	}
}