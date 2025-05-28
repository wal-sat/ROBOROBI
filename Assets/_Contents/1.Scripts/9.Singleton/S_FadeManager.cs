using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;

[DefaultExecutionOrder(1)] // ... (1)
public class S_FadeManager : Singleton<S_FadeManager>
{
    private VisualElement _blackPanel;

    public override void Awake()
    {
        base.Awake();

        var root = this.gameObject.GetComponent<UIDocument>().rootVisualElement;
        _blackPanel = root.Q<VisualElement>("black-panel");
    }

    public async UniTask FadeIn(float duration = 1f)
    {
        _blackPanel.style.transitionDuration = new List<TimeValue> { new(duration, TimeUnit.Second) };
        _blackPanel.AddToClassList("visible--disable");

        await UniTask.WaitForSeconds(duration);
    }

    public async UniTask FadeOut(float duration = 1f)
    {
        _blackPanel.style.transitionDuration = new List<TimeValue> { new(duration, TimeUnit.Second) };
        _blackPanel.RemoveFromClassList("visible--disable");

        await UniTask.WaitForSeconds(duration);
    }

    // (1)
    // UIDocument周りの初期化の後にこのクラスのAwake()を実行するために、ExecutionOrderを1に設定している。
}