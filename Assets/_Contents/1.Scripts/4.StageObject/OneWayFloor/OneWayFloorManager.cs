using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class OneWayFloorManager : MonoBehaviour
{
    private List<OneWayFloor> _oneWayFloorList = new List<OneWayFloor>();

    // ----- Public Methods -----

    public void Register(OneWayFloor oneWayFloor)
    {
        if (oneWayFloor != null && !_oneWayFloorList.Contains(oneWayFloor))
        {
            _oneWayFloorList.Add(oneWayFloor);
        }
    }

    public void SetColliderEnable(bool isEnable)
    {
        foreach (var oneWayFloor in _oneWayFloorList)
        {
            oneWayFloor.IsColliderEnable = isEnable;
        }
    }
}
