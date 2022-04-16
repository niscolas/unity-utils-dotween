using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace niscolas.UnityUtils.Extras
{
    public class DOTweenSimpleRotation : BaseDOTweenTransformOperationMB<Quaternion, Vector3, QuaternionOptions>
    {
#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        private RotateMode _rotateMode = RotateMode.Fast;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        protected bool _isLocal;

        protected override void AfterSetDefaultOptions(
            TweenerCore<Quaternion, Vector3, QuaternionOptions> tweener)
        {
            if (_setFrom)
            {
                tweener.From(_from, _fromIsRelative);
            }
        }

        protected override TweenerCore<Quaternion, Vector3, QuaternionOptions> GetTweener()
        {
            if (!_isLocal)
            {
                return Target.DORotate(_to, _duration, _rotateMode);
            }
            else
            {
                return Target.DOLocalRotate(_to, _duration, _rotateMode);
            }
        }
    }
}