using System;
using UnityEditor;
using UnityEngine;

public class TitleSceneMenu : MonoBehaviour
{
    [SerializeField] private TitleSceneUIToolkit _titleSceneUIToolkit;

    public Action<TitleState> OnChangeTitleState;

    private int _menuIndex;
    int MenuIndex
    {
        get => _menuIndex;
        set
        {
            _menuIndex = Mathf.Clamp(value, 0, 2);
            _titleSceneUIToolkit.SelectMenuOption(_menuIndex);
        }
    }

    // ----- Public Methods -----

    public void Up()
    {
        MenuIndex--;
    }
    public void Down()
    {
        MenuIndex++;
    }
    public void Submit()
    {
        MenuSubmit(MenuIndex);
    }
    public void Cancel()
    {
        BackPushS();
    }

    public void Initialize(TitleState titleStatePast)
    {
        switch (titleStatePast)
        {
            case TitleState.PushS:
            case TitleState.SaveSlots:
                MenuIndex = 0;
                break;
            case TitleState.Settings:
                MenuIndex = 1;
                break;
            case TitleState.Exit:
                MenuIndex = 2;
                break;
        }
    }

    // ----- Private Methods -----

    private void MenuSubmit(int index)
    {
        switch (index)
        {
            case 0:
                _titleSceneUIToolkit.SelectMenuOption(-1);
                OnChangeTitleState?.Invoke(TitleState.SaveSlots);
                break;
            case 1:
                _titleSceneUIToolkit.SelectMenuOption(-1);
                OnChangeTitleState?.Invoke(TitleState.Settings);
                break;
            case 2:
                _titleSceneUIToolkit.SelectMenuOption(-1);
                OnChangeTitleState?.Invoke(TitleState.Exit);
                break;
        }
    }

    private void BackPushS()
    {
        _titleSceneUIToolkit.SelectMenuOption(-1);
        OnChangeTitleState?.Invoke(TitleState.PushS);
    }
}
