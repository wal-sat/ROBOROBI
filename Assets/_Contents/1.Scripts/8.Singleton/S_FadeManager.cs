using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;
using System.Threading;

[DefaultExecutionOrder(1)] // ... (1)クラスの下で説明
public class S_FadeManager : Singleton<S_FadeManager>
{
    private VisualElement _blackPanel;

    // ----- Life Cycle Methods -----

    protected override void Awake()
    {
        base.Awake();
        if (!_isValid) return;

        var root = this.gameObject.GetComponent<UIDocument>().rootVisualElement;
        _blackPanel = root.Q<VisualElement>("black-panel");
    }

    // ----- Public Methods -----

    public async UniTask FadeIn(float duration, CancellationToken cancellationToken)
    {
        _blackPanel.style.transitionDuration = new List<TimeValue> { new(duration, TimeUnit.Second) };
        _blackPanel.AddToClassList("visible--disable");

        await UniTask.WaitForSeconds(duration, true, cancellationToken: cancellationToken);
    }

    public async UniTask FadeOut(float duration, CancellationToken cancellationToken)
    {
        _blackPanel.style.transitionDuration = new List<TimeValue> { new(duration, TimeUnit.Second) };
        _blackPanel.RemoveFromClassList("visible--disable");

        await UniTask.WaitForSeconds(duration, true, cancellationToken: cancellationToken);
    }
}

// (1)
// UIDocument周りの初期化の後にこのクラスのAwake()を実行するために、ExecutionOrderを1に設定している。