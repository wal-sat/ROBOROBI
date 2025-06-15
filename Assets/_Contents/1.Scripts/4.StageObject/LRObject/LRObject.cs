using UnityEngine;
using UnityEngine.Splines;
using DG.Tweening;

public class LRObject : MonoBehaviour
{
    [SerializeField] private LRObjectManager _lrObjectManager;
    [SerializeField] private LRObjectView _lrObjectView;
    [SerializeField] private SplineAnimate _splineAnimate;

    private Tweener _currentTweener;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _lrObjectManager.Register(this);
    }

    // ----- Public Methods -----

    public void PlayAnimation(LRObjectState lrObjectState, float animationDuration)
    {
        float currentTime = _splineAnimate.NormalizedTime;
        float terminalTime, duration;

        if (lrObjectState == LRObjectState.L) terminalTime = 0;
        else terminalTime = 1;

        duration = animationDuration * Mathf.Abs(terminalTime - currentTime);

        if (_currentTweener != null) _currentTweener.Kill();
        _currentTweener = DOVirtual.Float(currentTime, terminalTime, duration, v => _splineAnimate.NormalizedTime = v).SetEase(Ease.OutSine);

        _lrObjectView.SpriteChange(lrObjectState);
    }
    
    public void Initialize(LRObjectState lrObjectState)
    {
        _currentTweener.Kill();
        _splineAnimate.NormalizedTime = 0;

        _lrObjectView.SpriteChange(lrObjectState);
    }
}
