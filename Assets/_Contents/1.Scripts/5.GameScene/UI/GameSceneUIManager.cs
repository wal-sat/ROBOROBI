using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameSceneUIState { Sleep, Playing, SleepCamera, Clear }

public class GameSceneUIManager : MonoBehaviour
{
    [Serializable]
    private class ClearKindInfo
    {
        public ClearKind ClearKind;
        public Sprite ClearIcon;
        public string ClearText;
    }

    [SerializeField] private GameSceneUIToolkit _gameSceneUIToolkit;
    [SerializeField] private GameSceneClearUIToolkit _gameSceneClearUIToolkit;
    [SerializeField] private ClearKindInfo[] _clearKindInfos;
    [SerializeField] private Sprite _defaultGearSprite;
    [SerializeField] private Sprite _disableGearSprite;

    private Dictionary<ClearKind, ClearKindInfo> _clearKindInfoDictionary = new Dictionary<ClearKind, ClearKindInfo>();

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        for (int i = 0; i < _clearKindInfos.Length; i++)
        {
            _clearKindInfoDictionary.Add(_clearKindInfos[i].ClearKind, _clearKindInfos[i]);
        }
    }

    // ----- Public Methods -----

    public void DisplayUI(GameSceneUIState gameSceneUIState)
    {
        _gameSceneUIToolkit.DisplayMainUI(false);
        _gameSceneUIToolkit.DisplayActivateUI(false);
        _gameSceneUIToolkit.DisplaySleepCameraUI(false);
        _gameSceneClearUIToolkit.DisplayClear(false);

        switch (gameSceneUIState)
        {
            case GameSceneUIState.Sleep:
                _gameSceneUIToolkit.DisplayMainUI(true);
                _gameSceneUIToolkit.DisplayActivateUI(true);
                break;
            case GameSceneUIState.Playing:
                _gameSceneUIToolkit.DisplayMainUI(true);
                break;
            case GameSceneUIState.SleepCamera:
                _gameSceneUIToolkit.DisplaySleepCameraUI(true);
                break;
            case GameSceneUIState.Clear:
                _gameSceneClearUIToolkit.DisplayClear(true);
                break;
        }
    }

    // ----- UI -----

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

    public void ChangeClearPanelInformation(SceneKind sceneKind, ClearKind clearKind, string deathCountText, string playTimeText)
    {
        StageData stageData = S_StageInfoManager.Instance.StageDataDictionary[sceneKind];

        _gameSceneClearUIToolkit.ChangeClearIcon(_clearKindInfoDictionary[clearKind].ClearIcon);
        _gameSceneClearUIToolkit.ChangeClearSubText(_clearKindInfoDictionary[clearKind].ClearText);
        for (int i = 0; i < 5; i++)
        {
            _gameSceneClearUIToolkit.ChangeGearIcons(i, stageData.IsAcquiredGears[i] ? _defaultGearSprite : _disableGearSprite);
        }
        _gameSceneClearUIToolkit.ChangeDeathCountText(deathCountText);
        _gameSceneClearUIToolkit.ChangeMinimumDeathCountText(stageData.GetMinimumDeathCountString());
        _gameSceneClearUIToolkit.ChangePlayTimeText(playTimeText);
        _gameSceneClearUIToolkit.ChangeFastestPlayTimeText(stageData.GetFastestClearTimeString());
    }

    public void SelectClearOption(int index)
    {
        _gameSceneClearUIToolkit.SelectClearOption(index);
    }
}
