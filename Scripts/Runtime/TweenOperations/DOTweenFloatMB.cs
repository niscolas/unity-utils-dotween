using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityAtoms.BaseAtoms;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace niscolas.UnityUtils.Extras
{
    public class DOTweenFloatMB : BaseDOTweenOperationMB<float, float, FloatOptions>
    {
#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        private FloatReference _target;

#if ODIN_INSPECTOR
        [FoldoutGroup("From")]
#endif
        [SerializeField]
        private FloatReference _from;

#if ODIN_INSPECTOR
        [FoldoutGroup("To")]
#endif
        [SerializeField]
        private FloatReference _to;

        protected override TweenerCore<float, float, FloatOptions> GetTweener()
        {
            return DOTween.To(
                () => _from.Value,
                value => _target.Value = value,
                _to.Value,
                _duration.Value);
        }

        protected override GameObject GetLinkTarget()
        {
            return _gameObject;
        }
    }
}