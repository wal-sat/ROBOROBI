using UnityEngine;
using DG.Tweening;

public class S_FadeManager : Singleton<S_FadeManager>
{
    [SerializeField] CanvasGroup _blackPanelCanvasGroup;
    
    // ゲーム起動時にフェードインから始まる
    private void Start()
    {
        _blackPanelCanvasGroup.alpha = 1f;
        FadeIn(1);
    }

    public void FadeIn(float duration)
    {
        _blackPanelCanvasGroup.DOFade(0f, duration).SetEase(Ease.Linear);
    }
    public void FadeOut(float duration)
    {
        _blackPanelCanvasGroup.DOFade(1f, duration).SetEase(Ease.Linear);
    }
}