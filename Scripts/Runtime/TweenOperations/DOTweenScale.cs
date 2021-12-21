using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace niscolas.UnityUtils.Extras
{
    public class DOTweenScale : BaseDOTweenTransformOperation<Vector3, Vector3, VectorOptions>
    {
        protected override void AfterSetDefaultOptions(TweenerCore<Vector3, Vector3, VectorOptions> tweener)
        {
            if (_setFrom)
            {
                tweener.From(_from, _fromIsRelative);
            }
        }

        protected override TweenerCore<Vector3, Vector3, VectorOptions> GetTweener()
        {
            return Target.DOScale(_to, _duration.Value);
        }
    }
}