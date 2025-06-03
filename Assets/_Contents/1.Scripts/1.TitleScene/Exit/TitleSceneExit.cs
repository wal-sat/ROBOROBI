using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TitleSceneExit : MonoBehaviour
{
    [SerializeField] private TitleSceneUIToolkit _titleSceneUIToolkit;

    public Action<TitleSceneState> OnChangeTitleSceneState;

    private int _exitIndex;
    int ExitIndex
    {
        get => _exitIndex;
        set
        {
            _exitIndex = Mathf.Clamp(value, 0, 1);
            _titleSceneUIToolkit.SelectExitOption(_exitIndex);
        }
    }

    // ----- Public Methods -----

    public void Left()
    {
        ExitIndex--;
    }
    public void Right()
    {
        ExitIndex++;
    }
    public void Submit()
    {
        ExitSubmit(ExitIndex);
    }
    public void Cancel()
    {
        BackMenu();
    }

    public void Initialize()
    {
        ExitIndex = 0;
    }

    // ----- Private Methods -----

    private void ExitSubmit(int index)
    {
        switch (index)
        {
            case 0:
                BackMenu();
                break;
            case 1:
                ExitGame().Forget();
                break;
        }
    }
    
    private void BackMenu()
    {
        _titleSceneUIToolkit.SelectExitOption(-1);
        OnChangeTitleSceneState?.Invoke(TitleSceneState.Menu);
    }

    private async UniTaskVoid ExitGame()
    {
        await S_FadeManager.Instance.FadeOut(1f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
