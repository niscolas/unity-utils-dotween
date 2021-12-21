using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using niscolas.UnityUtils.Core;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityUtils;

namespace niscolas.UnityUtils.Extras
{
    // TODO move this logic to new ScriptableTween
    public abstract class BaseDOTweenOperation<TFrom, TTo, TOptions> : CachedMonoBehaviour
        where TOptions : struct, IPlugOptions
    {
        [FoldoutGroup("General Settings")]
        [SerializeField]
        private bool _autoStart;

        [FoldoutGroup("General Settings")]
        [ShowIf(nameof(_autoStart))]
        [SerializeField]
        private MonoCallbackType _autoStartMoment;

        [FoldoutGroup("To")]
        [FormerlySerializedAs("_endPositionIsRelative"), SerializeField]
        private bool _toIsRelative;

        [FoldoutGroup("Tween Settings")]
        [SerializeField]
        protected FloatReference _duration;

        [FoldoutGroup("Tween Settings")]
        [SerializeField]
        protected FloatReference _delay;

        [FoldoutGroup("Tween Settings")]
        [SerializeField]
        private bool _useEaseCurve;

        [FoldoutGroup("Tween Settings")]
        [DisableIf(nameof(_useEaseCurve)), SerializeField]
        private Ease _ease;

        [FoldoutGroup("Tween Settings")]
        [EnableIf(nameof(_useEaseCurve)), SerializeField]
        private AnimationCurve _easeCurve;

        [FoldoutGroup("Tween Settings")]
        [SerializeField]
        private LinkBehaviour _linkBehaviour;

        [FoldoutGroup("Tween Settings")]
        [SerializeField]
        private UpdateType _updateType = UpdateType.Normal;

        [FoldoutGroup("Tween Settings")]
        [SerializeField]
        private bool _isIndependentUpdate;

        [FoldoutGroup("Tween Settings")]
        [SerializeField]
        private bool _autoKill;

        [FoldoutGroup("Tween Settings")]
        [SerializeField]
        private IntReference _loopCount;

        [FoldoutGroup("Tween Settings")]
        [SerializeField]
        private LoopType _loopType;

        [FoldoutGroup("Tween Settings")]
        [SerializeField]
        private bool _canReplayWhileIncomplete;

        [FoldoutGroup("Events")]
        [SerializeField]
        private UnityEvent _onComplete;

        [FoldoutGroup("Events")]
        [SerializeField]
        private UnityEvent _onKill;

        [FoldoutGroup("Events")]
        [SerializeField]
        private UnityEvent _onPause;

        [FoldoutGroup("Events")]
        [SerializeField]
        private UnityEvent _onPlay;

        [FoldoutGroup("Events")]
        [SerializeField]
        private UnityEvent _onRewind;

        [FoldoutGroup("Events")]
        [SerializeField]
        private UnityEvent _onStart;

        [FoldoutGroup("Events")]
        [SerializeField]
        private UnityEvent _onUpdate;

        [FoldoutGroup("Events")]
        [SerializeField]
        private UnityEvent _onStepComplete;

        protected abstract TweenerCore<TFrom, TTo, TOptions> GetTweener();

        protected abstract GameObject GetLinkTarget();

        protected override void Awake()
        {
            base.Awake();

            if (_autoStart)
            {
                MonoLifecycleHooksManager.AutoTrigger(_gameObject, DoTween, _autoStartMoment);
            }
        }

        private bool _isPlaying;

        [Button]
        public void DoTween()
        {
            if (_isPlaying && !_canReplayWhileIncomplete)
            {
                return;
            }

            SetDefaultOptions(GetTweener());
        }

        protected virtual void AfterSetDefaultOptions(TweenerCore<TFrom, TTo, TOptions> tweener) { }

        private void SetDefaultOptions(TweenerCore<TFrom, TTo, TOptions> tweener)
        {
            tweener
                .SetRelative(_toIsRelative)
                .SetDelay(_delay.Value)
                .SetUpdate(_updateType, _isIndependentUpdate)
                .SetLink(GetLinkTarget(), _linkBehaviour)
                .SetAutoKill(_autoKill)
                .SetLoops(_loopCount.Value, _loopType)
                .OnStart(() =>
                {
                    _isPlaying = true;
                    _onStart?.Invoke();
                })
                .OnKill(() => _onKill?.Invoke())
                .OnPause(() => _onPause?.Invoke())
                .OnPlay(() => _onPlay?.Invoke())
                .OnRewind(() => _onRewind?.Invoke())
                .OnUpdate(() => _onUpdate?.Invoke())
                .OnStepComplete(() => _onStepComplete?.Invoke())
                .OnComplete(() =>
                {
                    _onComplete?.Invoke();
                    _isPlaying = false;
                });

            if (_useEaseCurve)
            {
                tweener.SetEase(_easeCurve);
            }
            else
            {
                tweener.SetEase(_ease);
            }

            AfterSetDefaultOptions(tweener);
        }
    }
}