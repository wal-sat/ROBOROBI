using System.Collections.Generic;
using UnityEngine;

public class BreakableBlockManager : MonoBehaviour
{
    private List<BreakableBlock> _breakableBlockList = new List<BreakableBlock>();

    // ----- Public Methods -----

    public void Register(BreakableBlock breakableBlock)
    {
        if (breakableBlock != null && !_breakableBlockList.Contains(breakableBlock))
        {
            _breakableBlockList.Add(breakableBlock);
        }
    }

    public void StageObjectInitialize()
    {
        foreach (var breakableBlock in _breakableBlockList)
        {
            breakableBlock.BreakableBlockInitialize();
        }
    }
}
