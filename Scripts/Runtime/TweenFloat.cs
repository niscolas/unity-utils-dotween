using DG.Tweening;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace niscolas.DOTweenUtils
{
    [CreateAssetMenu(menuName = Constants.CreateAssetMenuPrefix + "() => Tween Float")]
    public class TweenFloat : ScriptableObject
    {
        [SerializeField]
        private FloatVariable _target;

        [SerializeField]
        private BoolReference _fromCurrent;

        [SerializeField]
        private FloatReference _from;

        [SerializeField]
        private FloatReference _to;

        [SerializeField]
        private FloatReference _duration;

        [SerializeField]
        private Ease _ease;

        [SerializeField]
        private BoolReference _isIndependentUpdate;

        [Header("Events")]
        [SerializeField]
        private UnityEvent _onComplete;

        public void Do()
        {
            float from;
            if (_fromCurrent.Value)
            {
                from = _target.Value;
            }
            else
            {
                from = _to.Value;
            }

            DOTween
                .To(
                    () => _target.Value,
                    x => _target.Value = x,
                    _to.Value,
                    _duration.Value)
                .SetUpdate(_isIndependentUpdate.Value)
                .From(from)
                .SetEase(_ease)
                .OnComplete(
                    () => _onComplete?.Invoke());
        }
    }
}
