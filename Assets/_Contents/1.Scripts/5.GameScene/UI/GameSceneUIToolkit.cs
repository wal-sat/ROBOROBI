using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSceneUIToolkit : MonoBehaviour
{
    private VisualElement _main;
    private VisualElement _main__actionCard;
    private VisualElement _main__actionCard__actionIcon;
    private VisualElement _activate;
    private VisualElement _activate__eButton;
    private VisualElement _sleepCamera;

    private Label _main__gearCount__text;
    private Label _main__gearCount__subText;
    private Label _main__deathCount__text;
    private Label _main__stageName__text;
    private Label _main__playTime__text;
    private Label _main__actionCard__titleText;
    private Label _main__actionCard__text;

    private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _main = root.Q<VisualElement>("main");
        _main__actionCard = root.Q<VisualElement>("main__action-card");
        _main__actionCard__actionIcon = root.Q<VisualElement>("main__action-card__action-icon");
        _activate = root.Q<VisualElement>("activate");
        _activate__eButton = root.Q<VisualElement>("activate__e-button");
        _sleepCamera = root.Q<VisualElement>("sleep-camera");

        _main__gearCount__text = root.Q<Label>("main__gear-count__text");
        _main__gearCount__subText = root.Q<Label>("main__gear-count__sub-text");
        _main__deathCount__text = root.Q<Label>("main__death-count__text");
        _main__stageName__text = root.Q<Label>("main__stage-name__text");
        _main__playTime__text = root.Q<Label>("main__play-time__text");
        _main__actionCard__titleText = root.Q<Label>("main__action-card__title-text");
        _main__actionCard__text = root.Q<Label>("main__action-card__text");
    }

    // ----- Public Methods -----

    public void DisplayMainUI(bool isDisplay)
    {
        if (isDisplay)
        {
            MakeVisible(_main);
        }
        else
        {
            MakeInvisible(_main);
        }
    }

    public void DisplayActivateUI(bool isDisplay)
    {
        if (isDisplay)
        {
            MakeVisible(_activate);
        }
        else
        {
            MakeInvisible(_activate);
        }
    }

    public void DisplaySleepCameraUI(bool isDisplay)
    {
        if (isDisplay)
        {
            MakeVisible(_sleepCamera);
        }
        else
        {
            MakeInvisible(_sleepCamera);
        }
    }

    public void DisplayEButton(bool isDisplay)
    {
        if (isDisplay)
        {
            _activate__eButton.style.display = DisplayStyle.Flex;
        }
        else
        {
            _activate__eButton.style.display = DisplayStyle.None;
        }
    }

    public void ChangeGearText(string text, string subText)
    {
        _main__gearCount__text.text = text;

        if (subText == "0")
        {
            _main__gearCount__subText.style.visibility = Visibility.Hidden;
        }
        else
        {
            _main__gearCount__subText.style.visibility = Visibility.Visible;
            _main__gearCount__subText.text = $"(+{subText})";
        }
    }

    public void ChangeDeathText(string text)
    {
        _main__deathCount__text.text = text;
    }

    public void ChangeStageNameText(string worldName, string stageName)
    {
        _main__stageName__text.text = worldName + "\n" + stageName;
    }

    public void ChangePlayTimeText(string text)
    {
        _main__playTime__text.text = text;
    }

    public async UniTaskVoid MakeActionCard(bool isAcquired, string actionName, Sprite actionIcon)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            await DisplayActionCard(isAcquired, actionName, actionIcon);
        }
        finally
        {
            _semaphoreSlim.Release();
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

    private async UniTask DisplayActionCard(bool isAcquired, string actionName, Sprite actionIcon)
    {
        if (isAcquired)
        {
            _main__actionCard__titleText.text = "アクションゲット";
        }
        else
        {
            _main__actionCard__titleText.text = "アクションロスト";
        }
        _main__actionCard__text.text = actionName;
        _main__actionCard__actionIcon.style.backgroundImage = new StyleBackground(actionIcon);

        await UniTask.WaitForSeconds(0.1f);

        _main__actionCard.AddToClassList("main__action-card--display");

        await UniTask.WaitForSeconds(4f);

        _main__actionCard.RemoveFromClassList("main__action-card--display");

        await UniTask.WaitForSeconds(1f);
    }
}
