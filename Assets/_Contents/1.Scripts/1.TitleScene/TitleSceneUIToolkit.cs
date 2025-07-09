using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;

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
    private VisualElement[] _titleSaveSlots__saveSlots = new VisualElement[3];
    private VisualElement _titleSaveSlots__backOption;
    private VisualElement _titleExit;
    private VisualElement _titleExit__backOption;
    private VisualElement _titleExit__exitOption;

    private Dictionary<VisualElement, CancellationTokenSource> _CTSDictionary = new Dictionary<VisualElement, CancellationTokenSource>();

    // ----- Life Cycle Methods -----

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
        for (int i = 0; i < _titleSaveSlots__saveSlots.Length; i++)
        {
            _titleSaveSlots__saveSlots[i] = _titleSaveSlots.Q<VisualElement>($"title-save-slots__save-slot-{i + 1}");
        }
        _titleSaveSlots__backOption = _titleSaveSlots.Q<VisualElement>("title-save-slots__back-option");
        _titleExit = root.Q<VisualElement>("title-exit");
        _titleExit__backOption = _titleExit.Q<VisualElement>("title-exit__back-option");
        _titleExit__exitOption = _titleExit.Q<VisualElement>("title-exit__exit-option");

        _CTSDictionary.Add(_titlePushS__Text, null);
        _CTSDictionary.Add(_titleMenu__startOption, null);
        _CTSDictionary.Add(_titleMenu__settingOption, null);
        _CTSDictionary.Add(_titleMenu__exitOption, null);
        for (int i = 0; i < _titleSaveSlots__saveSlots.Length; i++)
        {
            _CTSDictionary.Add(_titleSaveSlots__saveSlots[i], null);
        }
        _CTSDictionary.Add(_titleSaveSlots__backOption, null);
        _CTSDictionary.Add(_titleExit__backOption, null);
        _CTSDictionary.Add(_titleExit__exitOption, null);
    }

    // ----- Public Methods -----

    public void ChangeTitleSceneStateUI(TitleSceneState titleSceneState)
    {
        MakeInvisible(_titlePushS);
        MakeInvisible(_titleMenu);
        MakeInvisible(_titleSaveSlots);
        ClosePanel(_titleExit);

        switch (titleSceneState)
        {
            case TitleSceneState.PushS:
                MakeVisible(_titlePushS);
                break;
            case TitleSceneState.Menu:
                MakeVisible(_titleMenu);
                break;
            case TitleSceneState.SaveSlots:
                MakeVisible(_titleSaveSlots);
                break;
            case TitleSceneState.Settings:
                break;
            case TitleSceneState.Exit:
                OpenPanel(_titleExit);
                break;
        }
    }

    public void SetPushingSTextAnimation(bool isStart)
    {
        if (isStart)
        {
            StartAnimation(_titlePushS__Text);
        }
        else
        {
            StopAnimation(_titlePushS__Text);
        }
    }

    public void SelectMenuOption(int index)
    {
        DeselectOption(_titleMenu__startOption);
        DeselectOption(_titleMenu__settingOption);
        DeselectOption(_titleMenu__exitOption);
        StopAnimation(_titleMenu__startOption);
        StopAnimation(_titleMenu__settingOption);
        StopAnimation(_titleMenu__exitOption);

        switch (index)
        {
            case 0:
                SelectOption(_titleMenu__startOption);
                StartAnimation(_titleMenu__startOption);
                break;
            case 1:
                SelectOption(_titleMenu__settingOption);
                StartAnimation(_titleMenu__settingOption);
                break;
            case 2:
                SelectOption(_titleMenu__exitOption);
                StartAnimation(_titleMenu__exitOption);
                break;
        }
    }
    public void SelectSaveSlotOption(int index, bool isSelecttingBackOption = false)
    {
        for (int i = 0; i < _titleSaveSlots__saveSlots.Length; i++)
        {
            DeselectOption(_titleSaveSlots__saveSlots[i]);
            StopAnimation(_titleSaveSlots__saveSlots[i]);
        }
        DeselectOption(_titleSaveSlots__backOption);
        StopAnimation(_titleSaveSlots__backOption);

        if (isSelecttingBackOption)
        {
            SelectOption(_titleSaveSlots__backOption);
            StartAnimation(_titleSaveSlots__backOption);
            return;
        }

        if (index < 0 || index >= _titleSaveSlots__saveSlots.Length) return;

        SelectOption(_titleSaveSlots__saveSlots[index]);
        StartAnimation(_titleSaveSlots__saveSlots[index]);
    }

    public void SelectExitOption(int index)
    {
        DeselectOption(_titleExit__backOption);
        DeselectOption(_titleExit__exitOption);
        StopAnimation(_titleExit__backOption);
        StopAnimation(_titleExit__exitOption);

        switch (index)
        {
            case 0:
                SelectOption(_titleExit__backOption);
                StartAnimation(_titleExit__backOption);
                break;
            case 1:
                SelectOption(_titleExit__exitOption);
                StartAnimation(_titleExit__exitOption);
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
        if (!visualElement.ClassListContains("option")) return;

        visualElement.AddToClassList("option--select");
    }
    private void DeselectOption(VisualElement visualElement)
    {
        if (!visualElement.ClassListContains("option--select")) return;

        visualElement.RemoveFromClassList("option--select");
    }

    /// <summary>
    /// テキストのアニメーション
    /// </summary>
    private void StartAnimation(VisualElement visualElement)
    {
        if (!visualElement.ClassListContains("animatable")) return;

        if (_CTSDictionary[visualElement] != null) return;
        _CTSDictionary[visualElement] = new CancellationTokenSource();

        visualElement.AddToClassList("animatable--animate");
        ActionLoop(() => visualElement.ToggleInClassList("animatable--animate-toggle"), _CTSDictionary[visualElement].Token).Forget();
    }
    private void StopAnimation(VisualElement visualElement)
    {
        if (!visualElement.ClassListContains("animatable--animate")) return;

        _CTSDictionary[visualElement]?.Cancel();
        _CTSDictionary[visualElement] = null;

        visualElement.RemoveFromClassList("animatable--animate");
        visualElement.RemoveFromClassList("animatable--animate-toggle");
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
