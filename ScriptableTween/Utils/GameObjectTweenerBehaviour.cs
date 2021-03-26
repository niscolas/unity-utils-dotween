using UnityEngine;

namespace Plugins.DOTweenUtils.ScriptableTween.Utils
{
	public class GameObjectTweenerBehaviour : BaseTweenerBehaviour<GameObject>
	{
		protected override GameObject Target
		{
			get
			{
				if (!target)
				{
					return gameObject;
				}

				return target;
			}
		}
	}
}