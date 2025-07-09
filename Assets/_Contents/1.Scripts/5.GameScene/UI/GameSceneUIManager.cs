using UnityEngine;

public enum GameSceneUIState { Sleep, Playing, SleepCamera }

public class GameSceneUIManager : MonoBehaviour
{
    [SerializeField] private GameSceneUIToolkit _gameSceneUIToolkit;

    // ----- Life Cycle Methods -----

    private void Awake()
    {

    }

    // ----- Public Methods -----

    public void DisplayUI(GameSceneUIState gameSceneUIState)
    {
        switch (gameSceneUIState)
        {
            case GameSceneUIState.Sleep:
                _gameSceneUIToolkit.DisplayMainUI(true);
                _gameSceneUIToolkit.DisplayActivateUI(true);
                _gameSceneUIToolkit.DisplaySleepCameraUI(false);
                break;
            case GameSceneUIState.Playing:
                _gameSceneUIToolkit.DisplayMainUI(true);
                _gameSceneUIToolkit.DisplayActivateUI(false);
                _gameSceneUIToolkit.DisplaySleepCameraUI(false);
                break;
            case GameSceneUIState.SleepCamera:
                _gameSceneUIToolkit.DisplayMainUI(false);
                _gameSceneUIToolkit.DisplayActivateUI(true);
                _gameSceneUIToolkit.DisplaySleepCameraUI(true);
                break;
        }
    }

    public void ChangeGearCount(int gearCount, int temporaryGearCount)
    {
        _gameSceneUIToolkit.ChangeGearText(gearCount.ToString(), temporaryGearCount.ToString());
    }

    public void ChangeDeathCount(int deathCount)
    {
        _gameSceneUIToolkit.ChangeDeathText(deathCount.ToString());
    }

    public void ChangeStageName(SceneKind sceneKind)
    {
        string worldName = S_StageInfoManager.Instance.StageDataDictionary[sceneKind].WorldName;
        string stageName = S_StageInfoManager.Instance.StageDataDictionary[sceneKind].StageName;
        _gameSceneUIToolkit.ChangeStageNameText(worldName, stageName);
    }

    public void UpdatePlayTime(string playTime)
    {
        _gameSceneUIToolkit.ChangePlayTimeText(playTime);
    }

    public void MakeActionCard(bool isAcquired, string actionName, Sprite ActionIcon)
    {
        _gameSceneUIToolkit.MakeActionCard(isAcquired, actionName, ActionIcon).Forget();
    }
}
