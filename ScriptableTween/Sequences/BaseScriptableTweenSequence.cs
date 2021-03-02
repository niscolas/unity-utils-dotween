using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Plugins.DOTweenUtils.ScriptableTween.Tweens;
using UnityAtoms;
using UnityEngine;

namespace Plugins.DOTweenUtils.ScriptableTween.Sequences {
	public abstract class BaseScriptableTweenSequence<T> : AtomAction<T> {
		[SerializeField]
		protected BaseScriptableTween<T>[] content;

		public sealed override async void Do(T target) {
			await DoAsync(target);
		}

		public async UniTask DoAsync(T target) {
			if (target == null) {
				return;
			}

			List<UniTask> tweenTasks = new List<UniTask>();

			foreach (BaseScriptableTween<T> scriptableTween in content) {
				tweenTasks.Add(
					scriptableTween.DoAsync(target)
				);
			}

			await UniTask.WhenAll(tweenTasks);
		}

		public IEnumerator<BaseScriptableTween<T>> GetEnumerator() {
			return ((IEnumerable<BaseScriptableTween<T>>) content).GetEnumerator();
		}
	}
}