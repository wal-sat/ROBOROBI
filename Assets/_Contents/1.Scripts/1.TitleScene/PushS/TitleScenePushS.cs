using System;
using UnityEngine;

public class TitleScenePushS : MonoBehaviour
{
    [SerializeField] private TitleSceneUIToolkit _titleSceneUIToolkit;

    public Action<TitleState> OnChangeTitleState;

    // ----- Public Methods -----

    public void Submit()
    {
        _titleSceneUIToolkit.SetPushingSTextAnimation(false);
        OnChangeTitleState?.Invoke(TitleState.Menu);
    }

    public void Initialize()
    {
        _titleSceneUIToolkit.SetPushingSTextAnimation(true);
    }
}
