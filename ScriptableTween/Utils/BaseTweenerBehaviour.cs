using Plugins.DOTweenUtils.ScriptableTween.Sequences;
using UnityEngine;

namespace Plugins.DOTweenUtils.ScriptableTween.Utils
{
	public class BaseTweenerBehaviour<T> : MonoBehaviour
	{
		[SerializeField]
		protected BaseScriptableTweenSequence<T> entryTweenSequence;

		[SerializeField]
		private T target;

		[SerializeField]
		private bool executeOnAwake;

		[SerializeField]
		private bool executeOnEnable;

		private void Awake()
		{
			if (executeOnAwake)
			{
				PlayEntryTween();
			}
		}

		private void OnEnable()
		{
			if (executeOnEnable)
			{
				PlayEntryTween();
			}
		}

		private async void PlayEntryTween()
		{
			await entryTweenSequence.DoAsync(target);
		}
	}
}