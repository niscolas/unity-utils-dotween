using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Plugins.DOTweenUtils {
	[CreateAssetMenu(menuName = nameof(ScriptableTween), order = -15)]
	public class ScriptableTween : ScriptableObject {
		[SerializeField]
		private TweenActionBase[] content;

		public async Task PlayTweenOn(GameObject target) {
			if (!target) {
				return;
			}
			
			Task[] tweenTasks = new Task[content.Length];

			for (int i = 0; i < content.Length; i++) {
				tweenTasks[i] = content[i].PlayTweenOn(target);
			}

			await Task.WhenAll(tweenTasks);
		}

		public IEnumerator<TweenActionBase> GetEnumerator() {
			return ((IEnumerable<TweenActionBase>) content).GetEnumerator();
		}
	}
}