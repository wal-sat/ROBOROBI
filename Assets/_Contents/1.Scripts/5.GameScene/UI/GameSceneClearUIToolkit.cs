using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;
public class GameSceneClearUIToolkit : MonoBehaviour
{
    private VisualElement _clear;
    private VisualElement _clear__icon;
    private VisualElement[] _clear__panel__gear__icons = new VisualElement[5];
    private VisualElement _clear__option__stageSelect;
    private VisualElement _clear__option__playAgain;

    private Label _clear__title__subText;
    private Label _clear__panel__deathCount__text;
    private Label _clear__panel__deathCount__minimumText;
    private Label _clear__panel__playTime__text;
    private Label _clear__panel__playTime__fastestText;

    private Dictionary<VisualElement, CancellationTokenSource> _CTSDictionary = new Dictionary<VisualElement, CancellationTokenSource>();

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _clear = root.Q<VisualElement>("clear");
        _clear__icon = root.Q<VisualElement>("clear__icon");
        for (int i = 0; i < 5; i++)
        {
            _clear__panel__gear__icons[i] = root.Q<VisualElement>($"clear__panel__gear__icon-{i}");
        }
        _clear__option__stageSelect = root.Q<VisualElement>("clear__option__stage-select");
        _clear__option__playAgain = root.Q<VisualElement>("clear__option__play-again");

        _clear__title__subText = root.Q<Label>("clear__title__sub-text");
        _clear__panel__deathCount__text = root.Q<Label>("clear__panel__death-count__text");
        _clear__panel__deathCount__minimumText = root.Q<Label>("clear__panel__death-count__minimum-text");
        _clear__panel__playTime__text = root.Q<Label>("clear__panel__play-time__text");
        _clear__panel__playTime__fastestText = root.Q<Label>("clear__panel__play-time__fastest-text");

        _CTSDictionary.Add(_clear__option__playAgain, null);
        _CTSDictionary.Add(_clear__option__stageSelect, null);
    }

    // ----- Public Methods -----

    public void DisplayClear(bool isDisplay)
    {
        if (isDisplay)
        {
            MakeVisible(_clear);
        }
        else
        {
            MakeInvisible(_clear);
        }
    }

    public void ChangeClearIcon(Sprite sprite)
    {
        _clear__icon.style.backgroundImage = new StyleBackground(sprite);
    }

    public void ChangeClearSubText(string text)
    {
        _clear__title__subText.text = text;
    }

    public void ChangeGearIcons(int index, Sprite sprite)
    {
        _clear__panel__gear__icons[index].style.backgroundImage = new StyleBackground(sprite);
    }

    public void ChangeDeathCountText(string text)
    {
        _clear__panel__deathCount__text.text = "デス数：" + text;
    }

    public void ChangeMinimumDeathCountText(string text)
    {
        _clear__panel__deathCount__minimumText.text = "最小デス数：" + text;
    }

    public void ChangePlayTimeText(string text)
    {
        _clear__panel__playTime__text.text = "タイム：" +  text;
    }

    public void ChangeFastestPlayTimeText(string text)
    {
        _clear__panel__playTime__fastestText.text = "最速タイム：" + text;
    }

    public void SelectClearOption(int index)
    {
        DeselectOption(_clear__option__stageSelect);
        DeselectOption(_clear__option__playAgain);
        StopAnimation(_clear__option__stageSelect);
        StopAnimation(_clear__option__playAgain);

        switch (index)
        {
            case 0:
                SelectOption(_clear__option__stageSelect);
                StartAnimation(_clear__option__stageSelect);
                break;
            case 1:
                SelectOption(_clear__option__playAgain);
                StartAnimation(_clear__option__playAgain);
                break;
        }
    }

    // ----- Private Methods -----

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
        _CTSDictionary[visualElement]?.Dispose();
        _CTSDictionary[visualElement] = null;

        visualElement.RemoveFromClassList("animatable--animate");
        visualElement.RemoveFromClassList("animatable--animate-toggle");
    }

    private async UniTaskVoid ActionLoop(Action action, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(1f, true, cancellationToken: token);

            action();
        }
    }
}
