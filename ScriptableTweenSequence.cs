using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityAtoms;
using UnityEngine;
using UniTask = Cysharp.Threading.Tasks.UniTask;

namespace Plugins.DOTweenUtils {
	[CreateAssetMenu(menuName = BaseAssetMenuPath + "Scriptable Tween Sequence", order = AssetMenuOrder)]
	public class ScriptableTweenSequence : AtomAction<GameObject> {
		public const string BaseAssetMenuPath = "Scriptable Tween/";
		public const int AssetMenuOrder = -20;

		[SerializeField]
		private BaseScriptableTween[] content;

		public override async void Do(GameObject target) {
			await DoAsync(target);
		}

		public async UniTask DoAsync(GameObject target) {
			if (!target) {
				return;
			}

			List<UniTask> tweenTasks = new List<UniTask>();
			
			foreach (BaseScriptableTween scriptableTween in content) {
				tweenTasks.Add(
					scriptableTween.DoAsync(target)
				);
			}

			await UniTask.WhenAll(tweenTasks);
		}

		public IEnumerator<BaseScriptableTween> GetEnumerator() {
			return ((IEnumerable<BaseScriptableTween>) content).GetEnumerator();
		}
	}
}