using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSceneClearUIToolkit : MonoBehaviour
{
    private VisualElement _clear;
    private VisualElement _clear__icon;
    private VisualElement[] _clear__panel__gear__icons = new Label[5];
    private VisualElement _clear__option__stageSelect;
    private VisualElement _clear__option__playAgain;

    private Label _clear__title__subText;
    private Label _clear__panel__deathCount_text;
    private Label _clear__panel__deathCount_minimumText;
    private Label _clear__panel__playTime__text;
    private Label _clear__panel__playTime__fastestText;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _clear = root.Q<VisualElement>("clear");
        _clear__icon = root.Q<VisualElement>("clear__icon");
        for (int i = 0; i < 5; i++)
        {
            _clear__panel__gear__icons[i] = root.Q<Label>($"clear__panel__gear__icon-{i + 1}");
        }
        _clear__option__stageSelect = root.Q<VisualElement>("clear__option__stage-select");
        _clear__option__playAgain = root.Q<VisualElement>("clear__option__play-again");

        _clear__title__subText = root.Q<Label>("clear__title__sub-text");
        _clear__panel__deathCount_text = root.Q<Label>("clear__panel__death-count_text");
        _clear__panel__deathCount_minimumText = root.Q<Label>("clear__panel__death-count_minimum-text");
        _clear__panel__playTime__text = root.Q<Label>("clear__panel__play-time__text");
        _clear__panel__playTime__fastestText = root.Q<Label>("clear__panel__play-time__fastest-text");
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

    public void ChangeGearIcons(int index, Sprite sprite)
    {
        _clear__panel__gear__icons[index].style.backgroundImage = new StyleBackground(sprite);
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
}
