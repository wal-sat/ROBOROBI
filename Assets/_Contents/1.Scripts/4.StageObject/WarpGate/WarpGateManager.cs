using System.Collections.Generic;
using UnityEngine;

public class WarpGateManager : MonoBehaviour
{
    private List<WarpGate> _warpGateList = new List<WarpGate>();

    // ----- Public Methods -----

    public void Register(WarpGate warpGate)
    {
        if (warpGate != null && !_warpGateList.Contains(warpGate))
        {
            _warpGateList.Add(warpGate);
        }
    }

    public void StageObjectInitialize()
    {
        foreach (var warpGate in _warpGateList)
        {
            warpGate.WarpGateInitialize();
        }
    }
}
