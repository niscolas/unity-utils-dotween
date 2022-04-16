using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using niscolas.UnityUtils.Core;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#elif NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#endif

namespace niscolas.UnityUtils.Extras
{
    public abstract class BaseDOTweenOperationMB<TFrom, TTo, TOptions> : CachedMB
        where TOptions : struct, IPlugOptions
    {
#if ODIN_INSPECTOR
        [FoldoutGroup("General Settings")]
#endif
        [SerializeField]
        private bool _autoStart;

#if ODIN_INSPECTOR
        [FoldoutGroup("General Settings")]
#endif
#if ODIN_INSPECTOR || NAUGHTY_ATTRIBUTES
        [ShowIf(nameof(_autoStart))]
#endif
        [SerializeField]
        private MonoBehaviourEventType _autoStartMoment;

#if ODIN_INSPECTOR
        [FoldoutGroup("To")]
#endif
        [FormerlySerializedAs("_endPositionIsRelative"), SerializeField]
        private bool _toIsRelative;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        protected FloatReference _duration;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        protected FloatReference _delay;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        private bool _useEaseCurve;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
#if ODIN_INSPECTOR || NAUGHTY_ATTRIBUTES
        [DisableIf(nameof(_useEaseCurve))]
#endif
        [SerializeField]
        private Ease _ease;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
#if ODIN_INSPECTOR || NAUGHTY_ATTRIBUTES
        [EnableIf(nameof(_useEaseCurve))]
#endif
        [SerializeField]
        private AnimationCurve _easeCurve;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        private LinkBehaviour _linkBehaviour;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        private UpdateType _updateType = UpdateType.Normal;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        private bool _isIndependentUpdate;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        private bool _autoKill;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        private IntReference _loopCount;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        private LoopType _loopType;

#if ODIN_INSPECTOR
        [FoldoutGroup("Tween Settings")]
#endif
        [SerializeField]
        private bool _canReplayWhileIncomplete;

#if ODIN_INSPECTOR
        [FoldoutGroup("Events")]
#endif
        [SerializeField]
        private UnityEvent _onComplete;

#if ODIN_INSPECTOR
        [FoldoutGroup("Events")]
#endif
        [SerializeField]
        private UnityEvent _onKill;

#if ODIN_INSPECTOR
        [FoldoutGroup("Events")]
#endif
        [SerializeField]
        private UnityEvent _onPause;

#if ODIN_INSPECTOR
        [FoldoutGroup("Events")]
#endif
        [SerializeField]
        private UnityEvent _onPlay;

#if ODIN_INSPECTOR
        [FoldoutGroup("Events")]
#endif
        [SerializeField]
        private UnityEvent _onRewind;

#if ODIN_INSPECTOR
        [FoldoutGroup("Events")]
#endif
        [SerializeField]
        private UnityEvent _onStart;

#if ODIN_INSPECTOR
        [FoldoutGroup("Events")]
#endif
        [SerializeField]
        private UnityEvent _onUpdate;

#if ODIN_INSPECTOR
        [FoldoutGroup("Events")]
#endif
        [SerializeField]
        private UnityEvent _onStepComplete;

        protected abstract TweenerCore<TFrom, TTo, TOptions> GetTweener();

        protected abstract GameObject GetLinkTarget();

        protected override void Awake()
        {
            base.Awake();

            if (_autoStart)
            {
                MonoHooksManagerMB.AutoTrigger(_gameObject, DoTween, _autoStartMoment);
            }
        }

        private bool _isPlaying;

#if ODIN_INSPECTOR || NAUGHTY_ATTRIBUTES
        [Button]
#endif
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