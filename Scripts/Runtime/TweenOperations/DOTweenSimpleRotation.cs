using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;

namespace niscolas.UnityUtils.Extras
{
    public class DOTweenSimpleRotation : BaseDOTweenTransformOperation<Quaternion, Vector3, QuaternionOptions>
    {
        [FoldoutGroup("Tween Settings")]
        [SerializeField]
        private RotateMode _rotateMode = RotateMode.Fast;

        [FoldoutGroup("Tween Settings")]
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