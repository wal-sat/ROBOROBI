using System.Collections.Generic;
using UnityEngine;

public class PlayerBreakBlock : MonoBehaviour, IBreakBlock
{
    private List<BreakableBlock> _breakableBlockList = new List<BreakableBlock>();

    public void Register(BreakableBlock breakableBlock)
    {
        if (breakableBlock != null && !_breakableBlockList.Contains(breakableBlock))
        {
            _breakableBlockList.Add(breakableBlock);
        }
    }

    public void Initialize()
    {
        foreach (var breakableBlock in _breakableBlockList)
        {
            breakableBlock.BreakableBlockInitialize();
        }
        _breakableBlockList.Clear();
    }
}
