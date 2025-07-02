using System;
using UnityEngine;

public class CameraArea : StageAreaBase
{
    [SerializeField] private CameraAreaManager _cameraAreaManager;
    
    [SerializeField] public int CameraAreaPriority;
    [SerializeField] public float CameraSize;

    // ----- Private Methods -----

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerStageAreaChecker"))
        {
            _cameraAreaManager.Register(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerStageAreaChecker"))
        {
            _cameraAreaManager.Unregister(this);
        }
    }
}
