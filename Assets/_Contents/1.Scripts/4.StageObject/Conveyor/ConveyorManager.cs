using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor.Purchasing;
using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    private List<Conveyor> _conveyorList = new List<Conveyor>();

    // ----- Public Methods -----

    public void Register(Conveyor conveyor)
    {
        if (conveyor != null && !_conveyorList.Contains(conveyor))
        {
            _conveyorList.Add(conveyor);
        }
    }

    public void StageObjectInitialize()
    {
        foreach (var conveyor in _conveyorList)
        {
            conveyor.ConveyorInitialize();
        }
    }

}
