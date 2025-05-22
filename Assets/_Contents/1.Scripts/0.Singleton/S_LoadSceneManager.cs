using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

// Scene名を列挙する
public enum SceneKind { Title, Stage0, Stage1, Ending }

public class S_LoadSceneManager : Singleton<S_LoadSceneManager>
{
    private const float FADE_TIME = 1f;

    public async void LoadScene(SceneKind sceneKind)
    {
        S_InputSystemManager.instance.SetLockInputDictionary(this.gameObject, true);
        S_FadeManager.instance.FadeOut(FADE_TIME);

        await UniTask.WaitForSeconds(FADE_TIME);

        SceneManager.LoadScene(sceneKind.ToString());

        await UniTask.WaitForSeconds(FADE_TIME / 2);

        S_InputSystemManager.instance.SetLockInputDictionary(this.gameObject, false);
        S_FadeManager.instance.FadeIn(FADE_TIME);
    }
}