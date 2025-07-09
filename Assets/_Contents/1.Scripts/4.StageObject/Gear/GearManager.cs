using UnityEngine;
using System.Collections.Generic;

public enum GearStatus { Acquired, NotAcquired, TemporaryAcquired }

public class GearManager : MonoBehaviour
{
    [SerializeField] private GameSceneUIManager _gameSceneUIManager;

    private List<Gear> _gearList = new List<Gear>();
    private SceneKind _sceneKind;

    // ----- Private Methods -----

    private void Awake()
    {
        _sceneKind = S_LoadSceneManager.Instance.GetCurrentSceneKind();
    }

    // ----- Public Methods -----

    public void Register(Gear gear)
    {
        if (gear != null && !_gearList.Contains(gear))
        {
            _gearList.Add(gear);
        }
    }

    public void GearInitialize()
    {
        foreach (var gear in _gearList)
        {
            if (S_StageInfoManager.Instance.StageDataDictionary[_sceneKind].IsAcquiredGears[gear.GearIndex])
            {
                gear.GearInitialize(GearStatus.Acquired);
            }
            else
            {
                gear.GearInitialize(GearStatus.NotAcquired);
            }
        }

        _gameSceneUIManager.ChangeGearCount(GetAcquiredGearCount(), GetTemporaryAcquiredGearCount());
    }

    public void OnAcquired()
    {
        _gameSceneUIManager.ChangeGearCount(GetAcquiredGearCount(), GetTemporaryAcquiredGearCount());
    }

    public void OnSave()
    {
        foreach (var gear in _gearList)
        {
            if (gear.GearStatus == GearStatus.TemporaryAcquired)
            {
                gear.GearStatus = GearStatus.Acquired;
                S_StageInfoManager.Instance.StageDataDictionary[_sceneKind].IsAcquiredGears[gear.GearIndex] = true;
            }
        }

        _gameSceneUIManager.ChangeGearCount(GetAcquiredGearCount(), GetTemporaryAcquiredGearCount());
    }

    // ----- Private Methods -----

    private int GetAcquiredGearCount()
    {
        int count = 0;
        foreach (var gear in _gearList)
        {
            if (gear.GearStatus == GearStatus.Acquired)
            {
                count++;
            }
        }

        return count;
    }

    private int GetTemporaryAcquiredGearCount()
    {
        int count = 0;
        foreach (var gear in _gearList)
        {
            if (gear.GearStatus == GearStatus.TemporaryAcquired)
            {
                count++;
            }
        }

        return count;
    }
}
