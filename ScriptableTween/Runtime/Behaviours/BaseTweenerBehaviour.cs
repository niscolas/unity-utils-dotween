using ScriptableTween.Sequences;
using UnityEngine;

namespace ScriptableTween.Behaviours
{
	public class BaseTweenerBehaviour<T> : MonoBehaviour
	{
		[SerializeField]
		protected BaseScriptableTweenSequence<T> entryTweenSequence;

		[SerializeField]
		protected T target;

		[SerializeField]
		private bool executeOnAwake;

		[SerializeField]
		private bool executeOnEnable;

		protected virtual T Target => target;

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
			await entryTweenSequence.DoAsync(Target);
		}
	}
}