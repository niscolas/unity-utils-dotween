using System.Collections.Generic;
using System.Threading.Tasks;
using UnityAtoms;
using UnityEngine;

namespace Plugins.DOTweenUtils {
	[CreateAssetMenu(menuName = BaseAssetMenuPath + "Scriptable Tween Sequence", order = AssetMenuOrder)]
	public class ScriptableTweenSequence : AtomAction<GameObject> {
		public const string BaseAssetMenuPath = "Scriptable Tween/";
		public const int AssetMenuOrder = -20;

		[SerializeField]
		private ScriptableTweenBase[] content;

		public override async void Do(GameObject target) {
			await DoAsync(target);
		}

		public async Task DoAsync(GameObject target) {
			if (!target) {
				return;
			}

			Task[] tweenTasks = new Task[content.Length];

			for (int i = 0; i < content.Length; i++) {
				tweenTasks[i] = content[i].DoAsync(target);
			}

			await Task.WhenAll(tweenTasks);
		}

		public IEnumerator<ScriptableTweenBase> GetEnumerator() {
			return ((IEnumerable<ScriptableTweenBase>) content).GetEnumerator();
		}
	}
}