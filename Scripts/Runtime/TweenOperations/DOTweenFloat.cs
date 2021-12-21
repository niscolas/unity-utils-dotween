using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace niscolas.UnityUtils.Extras
{
    public class DOTweenFloat : BaseDOTweenOperation<float, float, FloatOptions>
    {
        [FoldoutGroup("Tween Settings")]
        [SerializeField]
        private FloatReference _target;

        [FoldoutGroup("From")]
        [SerializeField]
        private FloatReference _from;

        [FoldoutGroup("To")]
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