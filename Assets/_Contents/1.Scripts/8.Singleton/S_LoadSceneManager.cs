using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;

// Scene名を列挙する
public enum SceneKind { Title, Stage }

public class S_LoadSceneManager : Singleton<S_LoadSceneManager>, IInputLockable
{
    private const float FadeTime = 1f;

    // ----- Public Methods -----

    public async UniTaskVoid LoadScene(SceneKind sceneKind)
    {
        S_InputSystemManager.Instance.SetInputLock(this, true);

        await S_FadeManager.Instance.FadeOut(FadeTime, destroyCancellationToken);

        SceneManager.LoadScene(sceneKind.ToString());

        await UniTask.WaitForSeconds(FadeTime, true, cancellationToken: destroyCancellationToken);

        S_InputSystemManager.Instance.SetInputLock(this, false);

        await S_FadeManager.Instance.FadeIn(FadeTime, destroyCancellationToken);
    }

    public SceneKind GetCurrentSceneKind()
    {
        return (SceneKind)Enum.Parse(typeof(SceneKind), SceneManager.GetActiveScene().name);
    }
}