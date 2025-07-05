using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class S_StageInfoManager : Singleton<S_StageInfoManager>
{
    [SerializeField] private StageData[] _stageData;

    public Dictionary<SceneKind, StageData> StageDataDictionary { get; private set; } = new Dictionary<SceneKind, StageData>();

    // ----- Life Cycle Methods -----

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < _stageData.Length; i++)
        {
            StageDataDictionary.Add(_stageData[i].SceneKind, _stageData[i]);
        }
    }

    // ----- Public Methods -----

    public void SetClearStatus(SceneKind sceneKind, bool isClear)
    {
        StageDataDictionary[sceneKind].IsClear = isClear;
    }

    public void SetAcquiredGearStatus(SceneKind sceneKind, int gearIndex, bool isAcquired)
    {
        StageDataDictionary[sceneKind].IsAcquiredGears[gearIndex] = isAcquired;
    }

    public void AddDeathCount(SceneKind sceneKind, int count, bool isCheckMinimus)
    {
        StageDataDictionary[sceneKind].SetDeathCount(count, isCheckMinimus);
    }

    public void AddPlayTime(SceneKind sceneKind, int time, bool isCheckFastest)
    {
        StageDataDictionary[sceneKind].SetPlayTime(time, isCheckFastest);
    }
}
