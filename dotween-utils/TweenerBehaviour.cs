using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Plugins.DOTweenUtils {
	public class TweenerBehaviour : MonoBehaviour {
		[FormerlySerializedAs("entryScriptableTweenGroup")]
		[Header("Entry Settings")]
		[SerializeField]
		private ScriptableTweenSequence entryScriptableTweenSequence;

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
			await entryScriptableTweenSequence.DoAsync(gameObject);
		}
	}
}