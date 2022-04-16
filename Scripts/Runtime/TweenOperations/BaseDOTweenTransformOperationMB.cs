using DG.Tweening.Plugins.Options;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#elif NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#endif

namespace niscolas.UnityUtils.Extras
{
    public abstract class BaseDOTweenTransformOperationMB<TFrom, TTo, TOptions> :
        BaseDOTweenOperationMB<TFrom, TTo, TOptions> where TOptions : struct, IPlugOptions
    {
#if ODIN_INSPECTOR
        [FoldoutGroup("General Settings")]
#endif
        [SerializeField]
        private Transform _target;

#if ODIN_INSPECTOR
        [FoldoutGroup("From")]
#endif
        [FormerlySerializedAs("_overwriteStartPosition"), SerializeField]
        protected bool _setFrom;

#if ODIN_INSPECTOR
        [FoldoutGroup("From")]
#endif
#if ODIN_INSPECTOR || NAUGHTY_ATTRIBUTES
        [EnableIf(nameof(_setFrom))]
#endif
        [FormerlySerializedAs("_startPositionIsRelative"), SerializeField]
        protected bool _fromIsRelative;

#if ODIN_INSPECTOR
        [FoldoutGroup("From")]
#endif
#if ODIN_INSPECTOR || NAUGHTY_ATTRIBUTES
        [EnableIf(nameof(_setFrom))]
#endif
        [FormerlySerializedAs("_fromPosition"), FormerlySerializedAs("_startPosition")]
        [SerializeField]
        protected Vector3Reference _from;

#if ODIN_INSPECTOR
        [FoldoutGroup("To")]
#endif
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