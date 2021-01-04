using System.Collections.Generic;
using System.Linq;
using __Utils.UnityUtils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __Utils._Packages__PluginsUtils.DOTweenUtils {
	[CreateAssetMenu(menuName = nameof(TweeningPreset), order = -15)]
	public class TweeningPreset : ScriptableObject {
		[SerializeField]
		private TweenAction[] content;

		[SerializeField]
		private bool disableOnFinish;

		[DisableIf(nameof(disableOnFinish))]
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