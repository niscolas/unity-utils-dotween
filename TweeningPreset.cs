using System.Collections.Generic;
using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace Plugins.DOTweenUtils {
	[CreateAssetMenu(menuName = nameof(TweeningPreset), order = -15)]
	public class TweeningPreset : ScriptableObject {
		[SerializeField]
		private TweenAction[] content;

		[SerializeField]
		private bool disableOnFinish;

#if ODIN_INSPECTOR
		[DisableIf(nameof(disableOnFinish))]
#endif
		[SerializeField]
		private bool destroyOnFinish;

		public TweenAction[] Content => content;

		public bool DestroyOnFinish => destroyOnFinish;

		public bool DisableOnFinish => disableOnFinish;

		public bool UnscaledTime => Content.Any(tweenAction => tweenAction.UnscaledTime);
		public float Duration => Content.Max(tweenAction => tweenAction.Duration);

		public IEnumerator<TweenAction> GetEnumerator() {
			return ((IEnumerable<TweenAction>) content).GetEnumerator();
		}
	}
}