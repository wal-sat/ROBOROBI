using System;
using UnityEngine;

public class TitleScenePushS : MonoBehaviour
{
    [SerializeField] private TitleSceneUIToolkit _titleSceneUIToolkit;

    public Action<TitleSceneState> OnChangeTitleSceneState;

    // ----- Public Methods -----

    public void Submit()
    {
        _titleSceneUIToolkit.SetPushingSTextAnimation(false);
        OnChangeTitleSceneState?.Invoke(TitleSceneState.Menu);
    }

    public void Initialize()
    {
        _titleSceneUIToolkit.SetPushingSTextAnimation(true);
    }
}
