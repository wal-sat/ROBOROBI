using UnityEngine;
using System.Collections.Generic;

public class BarrelManager : MonoBehaviour
{
    private List<Barrel> _barrelList = new List<Barrel>();

    // ----- Public Methods -----

    public void Register(Barrel barrel)
    {
        if (barrel != null && !_barrelList.Contains(barrel))
        {
            _barrelList.Add(barrel);
        }
    }

    public void StageObjectInitialize()
    {
        foreach (var barrel in _barrelList)
        {
            barrel.BarrelInitialize();
        }
    }
}
