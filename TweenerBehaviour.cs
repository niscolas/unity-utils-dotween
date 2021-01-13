using DG.Tweening;
using UnityEngine;

namespace Plugins.DOTweenUtils {
	public class TweenerBehaviour : MonoBehaviour {
		[Header("Entry Settings")]
		[SerializeField]
		private ScriptableTween entryScriptableTween;

		[SerializeField]
		private bool executeOnAwake;

		[SerializeField]
		private bool executeOnEnable;

		private void Awake() {
			if (executeOnAwake) {
				PlayEntryTween();
			}
		}

		private void OnEnable() {
			if (executeOnEnable) {
				PlayEntryTween();
			}
		}

		private void OnDisable() {
			transform.DOKill(true);
		}

		private async void PlayEntryTween() {
			await entryScriptableTween.PlayTweenOn(gameObject);
		}
	}
}