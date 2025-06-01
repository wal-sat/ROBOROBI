using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;

[DefaultExecutionOrder(1)] // ... (1)クラスの下で説明
public class S_TransitionManager : Singleton<S_TransitionManager>
{
    private enum PanelPosition { Left, Center, Right }

    private const float PanelDelay = 0.1f;

    private VisualElement _transitionPanel__white;
    private VisualElement _transitionPanel__blue;

    // ----- Life Cycle Methods -----

    public override void Awake()
    {
        base.Awake();

        var root = this.gameObject.GetComponent<UIDocument>().rootVisualElement;

        _transitionPanel__white = root.Q<VisualElement>("transition-panel__white");
        _transitionPanel__blue = root.Q<VisualElement>("transition-panel__blue");
    }

    // ----- Public Methods -----

    public async UniTask OutTransition(float duration = 0.5f)
    {
        _transitionPanel__white.style.display = DisplayStyle.Flex;
        _transitionPanel__blue.style.display = DisplayStyle.Flex;

        _transitionPanel__white.style.transitionDuration = new List<TimeValue> { new(duration - PanelDelay, TimeUnit.Second) };
        _transitionPanel__blue.style.transitionDuration = new List<TimeValue> { new(duration - PanelDelay, TimeUnit.Second) };

        MakePanelTransitions(_transitionPanel__white, PanelPosition.Center);

        await UniTask.WaitForSeconds(PanelDelay);

        MakePanelTransitions(_transitionPanel__blue, PanelPosition.Center);

        await UniTask.WaitForSeconds(duration - PanelDelay);
    }

    public async UniTask InTransition(float duration = 0.5f)
    {
        _transitionPanel__white.style.transitionDuration = new List<TimeValue> { new(duration - PanelDelay, TimeUnit.Second) };
        _transitionPanel__blue.style.transitionDuration = new List<TimeValue> { new(duration - PanelDelay, TimeUnit.Second) };

        MakePanelTransitions(_transitionPanel__blue, PanelPosition.Left);

        await UniTask.WaitForSeconds(PanelDelay);

        MakePanelTransitions(_transitionPanel__white, PanelPosition.Left);

        await UniTask.WaitForSeconds(duration - PanelDelay);

        Initialize().Forget();
    }

    // ----- Private Methods -----

    private void MakePanelTransitions(VisualElement visualElement, PanelPosition position)
    {
        visualElement.RemoveFromClassList("transition-panel--left");
        visualElement.RemoveFromClassList("transition-panel--center");
        visualElement.RemoveFromClassList("transition-panel--right");

        switch (position)
        {
            case PanelPosition.Left:
                visualElement.AddToClassList("transition-panel--left");
                break;
            case PanelPosition.Center:
                visualElement.AddToClassList("transition-panel--center");
                break;
            case PanelPosition.Right:
                visualElement.AddToClassList("transition-panel--right");
                break;
        }
    }

    private async UniTaskVoid Initialize()
    {
        _transitionPanel__white.style.display = DisplayStyle.None;
        _transitionPanel__blue.style.display = DisplayStyle.None;

        await UniTask.WaitForSeconds(0.25f); // Display.Noneが反映されるまでの待機

        _transitionPanel__white.style.transitionDuration = new List<TimeValue> { new(0.1f, TimeUnit.Second) };
        _transitionPanel__blue.style.transitionDuration = new List<TimeValue> { new(0.1f, TimeUnit.Second) };

        MakePanelTransitions(_transitionPanel__white, PanelPosition.Right);
        MakePanelTransitions(_transitionPanel__blue, PanelPosition.Right);
    }
}

// (1)
// UIDocument周りの初期化の後にこのクラスのAwake()を実行するために、ExecutionOrderを1に設定している。
