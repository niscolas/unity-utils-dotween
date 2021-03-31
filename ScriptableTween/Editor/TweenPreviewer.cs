#if ODIN_INSPECTOR
using System.Collections.Generic;
using DG.DOTweenEditor;
using DG.Tweening;
using Plugins.ScriptableTween.Tweens.GameObject;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Plugins.ScriptableTween.Editor
{
	public class TweenPreviewer : OdinEditorWindow
	{
		[SceneObjectsOnly]
		[SerializeField]
		private GameObject target;

		[Space(15)]
		[SerializeField]
		private ScriptableGameObjectTween scriptableTween;

		[MenuItem("Tools/Tween Previewer")]
		private static void OpenWindow()
		{
			GetWindow<TweenPreviewer>().Show();
		}

		[HorizontalGroup]
		[Button(ButtonSizes.Large, ButtonStyle.Box)]
		[GUIColor("@UnityEngine.Color.green")]
		public void Play()
		{
			IEnumerable<Tween> tweens = scriptableTween.GetTweens(target);
			foreach (Tween tween in tweens)
			{
				DOTweenEditorPreview.PrepareTweenForPreview(tween);
			}

			DOTweenEditorPreview.Start();
		}

		[HorizontalGroup]
		[Button(ButtonSizes.Large, ButtonStyle.Box)]
		[GUIColor("@UnityEngine.Color.red")]
		public void Stop()
		{
			DOTweenEditorPreview.Stop(true);
		}
	}
}
#endif