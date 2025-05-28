using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;

public class TitleSceneUIToolkit : MonoBehaviour
{
    private VisualElement _title;
    private VisualElement _titlePushS;
    private VisualElement _titlePushS__Text;
    private VisualElement _titleMenu;
    private VisualElement _titleMenu__startOption;
    private VisualElement _titleMenu__settingOption;
    private VisualElement _titleMenu__exitOption;
    private VisualElement _titleSaveSlots;
    private VisualElement _titleExit;
    private VisualElement _titleExit__backOption;
    private VisualElement _titleExit__exitOption;

    private Dictionary<VisualElement, CancellationTokenSource> _CTSDictionary = new Dictionary<VisualElement, CancellationTokenSource>();

    // ----- Life Methods -----

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _title = root.Q<VisualElement>("title");
        _titlePushS = root.Q<VisualElement>("title-push-s");
        _titlePushS__Text = _titlePushS.Q<VisualElement>("title-push-s__text");
        _titleMenu = root.Q<VisualElement>("title-menu");
        _titleMenu__startOption = _titleMenu.Q<VisualElement>("title-menu__start-option");
        _titleMenu__settingOption = _titleMenu.Q<VisualElement>("title-menu__setting-option");
        _titleMenu__exitOption = _titleMenu.Q<VisualElement>("title-menu__exit-option");
        _titleSaveSlots = root.Q<VisualElement>("title-save-slots");
        _titleExit = root.Q<VisualElement>("title-exit");
        _titleExit__backOption = _titleExit.Q<VisualElement>("title-exit__back-option");
        _titleExit__exitOption = _titleExit.Q<VisualElement>("title-exit__exit-option");

        _CTSDictionary.Add(_titlePushS__Text, null);
        _CTSDictionary.Add(_titleMenu__startOption, null);
        _CTSDictionary.Add(_titleMenu__settingOption, null);
        _CTSDictionary.Add(_titleMenu__exitOption, null);
        _CTSDictionary.Add(_titleExit__backOption, null);
        _CTSDictionary.Add(_titleExit__exitOption, null);
    }

    // ----- Public Methods -----

    public void ChangeTitleStateUI(TitleState titleState)
    {
        MakeInvisible(_titlePushS);
        MakeInvisible(_titleMenu);
        ClosePanel(_titleSaveSlots);
        ClosePanel(_titleExit);

        switch (titleState)
        {
            case TitleState.PushS:
                MakeVisible(_titlePushS);
                break;
            case TitleState.Menu:
                MakeVisible(_titleMenu);
                break;
            case TitleState.SaveSlots:
                OpenPanel(_titleSaveSlots);
                break;
            case TitleState.Settings:
                break;
            case TitleState.Exit:
                OpenPanel(_titleExit);
                break;
        }
    }

    public void SetPushingSTextAnimation(bool isStart)
    {
        if (isStart)
        {
            StartTextAnimation(_titlePushS__Text);
        }
        else
        {
            StopTextAnimation(_titlePushS__Text);
        }
    }

    public void SelectMenuOption(int index)
    {
        DeselectOption(_titleMenu__startOption);
        DeselectOption(_titleMenu__settingOption);
        DeselectOption(_titleMenu__exitOption);
        StopTextAnimation(_titleMenu__startOption);
        StopTextAnimation(_titleMenu__settingOption);
        StopTextAnimation(_titleMenu__exitOption);

        switch (index)
        {
            case 0:
                SelectOption(_titleMenu__startOption);
                StartTextAnimation(_titleMenu__startOption);
                break;
            case 1:
                SelectOption(_titleMenu__settingOption);
                StartTextAnimation(_titleMenu__settingOption);
                break;
            case 2:
                SelectOption(_titleMenu__exitOption);
                StartTextAnimation(_titleMenu__exitOption);
                break;
        }
    }

    public void SelectExitOption(int index)
    {
        DeselectOption(_titleExit__backOption);
        DeselectOption(_titleExit__exitOption);
        StopTextAnimation(_titleExit__backOption);
        StopTextAnimation(_titleExit__exitOption);

        switch (index)
        {
            case 0:
                SelectOption(_titleExit__backOption);
                StartTextAnimation(_titleExit__backOption);
                break;
            case 1:
                SelectOption(_titleExit__exitOption);
                StartTextAnimation(_titleExit__exitOption);
                break;
        }
    }

    // ----- Private Methods -----

    /// <summary>
    /// 透明度の変更
    /// </summary>
    private void MakeVisible(VisualElement visualElement)
    {
        if (!visualElement.ClassListContains("visible--disable")) return;

        visualElement.RemoveFromClassList("visible--disable");
    }
    private void MakeInvisible(VisualElement visualElement)
    {
        if (!visualElement.ClassListContains("visible")) return;

        visualElement.AddToClassList("visible--disable");
    }

    /// <summary>
    /// パネルの表示・非表示
    /// </summary>
    private void OpenPanel(VisualElement visualElement)
    {
        if (!visualElement.ClassListContains("panel")) return;

        visualElement.AddToClassList("panel--open");
    }
    private void ClosePanel(VisualElement visualElement)
    {
        if (!visualElement.ClassListContains("panel--open")) return;

        visualElement.RemoveFromClassList("panel--open");
    }

    /// <summary>
    /// オプションの選択状態
    /// </summary>
    private void SelectOption(VisualElement visualElement)
    {
        if (!visualElement.ClassListContains("text__option")) return;

        visualElement.AddToClassList("text__option--selected");
    }
    private void DeselectOption(VisualElement visualElement)
    {
        if (!visualElement.ClassListContains("text__option--selected")) return;

        visualElement.RemoveFromClassList("text__option--selected");
    }

    /// <summary>
    /// テキストのアニメーション
    /// </summary>
    private void StartTextAnimation(VisualElement visualElement)
    {
        if (!visualElement.ClassListContains("text")) return;

        if (_CTSDictionary[visualElement] != null) return;
        _CTSDictionary[visualElement] = new CancellationTokenSource();

        visualElement.AddToClassList("text--animate");
        ActionLoop(() => visualElement.ToggleInClassList("text--animate-toggle"), _CTSDictionary[visualElement].Token).Forget();
    }
    private void StopTextAnimation(VisualElement visualElement)
    {
        if (!visualElement.ClassListContains("text--animate")) return;

        _CTSDictionary[visualElement]?.Cancel();
        _CTSDictionary[visualElement] = null;

        visualElement.RemoveFromClassList("text--animate");
        visualElement.RemoveFromClassList("text--animate-toggle");
    }

    /// <summary>
    /// アクションをループで実行する
    /// </summary>
    private async UniTaskVoid ActionLoop(Action action, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(1f, cancellationToken: token);

            action();
        }
    }
}
