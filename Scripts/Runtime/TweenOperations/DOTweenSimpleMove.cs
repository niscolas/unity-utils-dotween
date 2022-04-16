using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#elif NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace niscolas.UnityUtils.Extras
{
    public class DOTweenSimpleMove : BaseDOTweenTransformOperationMB<Vector3, Vector3, VectorOptions>
    {
#if ODIN_INSPECTOR
        [FoldoutGroup("From")]
#endif
#if ODIN_INSPECTOR || NAUGHTY_ATTRIBUTES
        [EnableIf(nameof(_setFrom))]
#endif
        [FormerlySerializedAs("_fromPoint"), FormerlySerializedAs("_startPoint")]
        [SerializeField]
        private Transform _fromReferencePoint;

#if ODIN_INSPECTOR
        [FoldoutGroup("To")]
#endif
        [FormerlySerializedAs("_endPoint"), SerializeField]
        private Transform _toReferencePoint;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        protected bool _isLocal;

        protected override TweenerCore<Vector3, Vector3, VectorOptions> GetTweener()
        {
            TweenerCore<Vector3, Vector3, VectorOptions> tweener;

            if (!_isLocal)
            {
                tweener = Target.DOMove(GetTo(), _duration.Value);
            }
            else
            {
                tweener = Target.DOLocalMove(GetTo(), _duration.Value);
            }

            return tweener;
        }

        protected override void AfterSetDefaultOptions(
            TweenerCore<Vector3, Vector3, VectorOptions> tweener)
        {
            if (_setFrom)
            {
                tweener.From(GetFrom(), _fromIsRelative);
            }
        }

        protected Vector3 GetFrom()
        {
            if (_fromReferencePoint)
            {
                return _fromReferencePoint.position;
            }
            else
            {
                return _from.Value;
            }
        }

        private Vector3 GetTo()
        {
            if (_toReferencePoint)
            {
                return _toReferencePoint.position;
            }
            else
            {
                return _to.Value;
            }
        }
    }
}