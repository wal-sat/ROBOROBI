using UnityEngine;
using System.Collections.Generic;

public enum GearStatus { Acquired, NotAcquired, TemporaryAcquired }

public class GearManager : MonoBehaviour
{
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

    public void StageObjectInitialize()
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

        // gear数表示のUIの変更
    }

    public void OnAcquired()
    {
        // gear数表示のUIの変更
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

        // gear数表示のUIの変更
    }

    // ----- Private Methods -----

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
