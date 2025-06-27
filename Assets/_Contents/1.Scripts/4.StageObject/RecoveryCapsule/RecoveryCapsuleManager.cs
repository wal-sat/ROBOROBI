using System.Collections.Generic;
using UnityEngine;

public class RecoveryCapsuleManager : MonoBehaviour
{
    private List<RecoveryCapsule> _recoveryCapsuleList = new List<RecoveryCapsule>();

    // ----- Public Methods -----

    public void Register(RecoveryCapsule recoveryCapsule)
    {
        if (recoveryCapsule != null && !_recoveryCapsuleList.Contains(recoveryCapsule))
        {
            _recoveryCapsuleList.Add(recoveryCapsule);
        }
    }

    public void StageObjectInitialize()
    {
        foreach (var recoveryCapsule in _recoveryCapsuleList)
        {
            recoveryCapsule.RecoveryCapsuleInitialize();
        }
    }
}