using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace niscolas.UnityUtils.Extras
{
    public class DOTweenSimpleMove : BaseDOTweenTransformOperation<Vector3, Vector3, VectorOptions>
    {
        [FoldoutGroup("From")]
        [EnableIf(nameof(_setFrom))]
        [FormerlySerializedAs("_fromPoint"), FormerlySerializedAs("_startPoint")]
        [SerializeField]
        private Transform _fromReferencePoint;

        [FoldoutGroup("To")]
        [FormerlySerializedAs("_endPoint"), SerializeField]
        private Transform _toReferencePoint;
        
        [FoldoutGroup("Tween Settings")]
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