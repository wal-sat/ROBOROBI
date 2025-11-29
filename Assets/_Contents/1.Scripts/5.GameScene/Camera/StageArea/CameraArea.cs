using System;
using UnityEngine;

public class CameraArea : StageAreaBase
{
    [SerializeField] private CameraAreaManager _cameraAreaManager;
    [SerializeField] private OnTriggerWithRigidbodyObject _onTriggerWithRigidbodyObject;
    
    [SerializeField] public int CameraAreaPriority;
    [SerializeField] public float CameraSize;

    // ----- Life Cycle Methods -----

    protected override void Awake()
    {
        base.Awake();
        _onTriggerWithRigidbodyObject.TriggerEnterCallback += TriggerEnter;
        _onTriggerWithRigidbodyObject.TriggerExitCallback += TriggerExit;
    }

    // ----- Private Methods -----

    private void TriggerEnter(RigidbodyObject rigidbodyObject)
    {
        _cameraAreaManager.Register(this);
    }
    private void TriggerExit(RigidbodyObject rigidbodyObject)
    {
        _cameraAreaManager.Unregister(this);
    }
}
