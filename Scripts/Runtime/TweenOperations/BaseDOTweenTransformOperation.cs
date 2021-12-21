using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

namespace niscolas.UnityUtils.Extras
{
    public abstract class BaseDOTweenTransformOperation<TFrom, TTo, TOptions> :
        BaseDOTweenOperation<TFrom, TTo, TOptions> where TOptions : struct, IPlugOptions
    {
        [FoldoutGroup("General Settings")]
        [SerializeField]
        private Transform _target;

        [FoldoutGroup("From")]
        [FormerlySerializedAs("_overwriteStartPosition"), SerializeField]
        protected bool _setFrom;

        [FoldoutGroup("From")]
        [EnableIf(nameof(_setFrom))]
        [FormerlySerializedAs("_startPositionIsRelative"), SerializeField]
        protected bool _fromIsRelative;

        [FoldoutGroup("From")]
        [EnableIf(nameof(_setFrom))]
        [FormerlySerializedAs("_fromPosition"), FormerlySerializedAs("_startPosition")]
        [SerializeField]
        protected Vector3Reference _from;

        [FoldoutGroup("To")]
        [FormerlySerializedAs("_endPosition"), SerializeField]
        protected Vector3Reference _to;

        protected Transform Target
        {
            get
            {
                if (!_target)
                {
                    _target = transform;
                }

                return _target;
            }
        }

        protected override GameObject GetLinkTarget()
        {
            return Target.gameObject;
        }
    }
}