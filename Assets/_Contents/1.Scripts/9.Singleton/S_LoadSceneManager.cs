using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

// Scene名を列挙する
public enum SceneKind { Title }

public class S_LoadSceneManager : Singleton<S_LoadSceneManager>, IInputLockable
{
    private const float FadeTime = 1f;
    
    // ----- Public Methods -----

    public async UniTaskVoid LoadScene(SceneKind sceneKind)
    {
        S_InputSystemManager.Instance.SetInputLock(this, true);

        await S_FadeManager.Instance.FadeOut(FadeTime);

        SceneManager.LoadScene(sceneKind.ToString());

        await UniTask.WaitForSeconds(FadeTime);

        S_InputSystemManager.Instance.SetInputLock(this, false);

        await S_FadeManager.Instance.FadeIn(FadeTime);
    }
}