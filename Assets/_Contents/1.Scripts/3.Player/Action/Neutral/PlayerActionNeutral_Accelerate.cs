using Unity.VisualScripting;
using UnityEngine;

public class PlayerActionNeutral_Accelerate : PlayerActionBase
{
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] PlayerViewManager _playerViewManager;
    [SerializeField] private float _accelerateSpeed;

    // ----- Public Methods -----

    public override void InitAction()
    {
        base.InitAction();

        _playerMovementManager.SubscribeSpeedAdjustCallback(Accelerate);
        _playerViewManager.IsAccelerate = true;
    }

    public override void EndAction()
    {
        base.EndAction();

        _playerMovementManager.UnsubscribeSpeedAdjustCallback(Accelerate);
        _playerViewManager.IsAccelerate = false;
    }

    public override void InitializeAction()
    {
        base.InitializeAction();

        _playerMovementManager.UnsubscribeSpeedAdjustCallback(Accelerate);
        _playerViewManager.IsAccelerate = false;
    }

    // ----- Private Methods -----

    private float Accelerate()
    {
        return _playerMovementManager.IsFacingRight ? _accelerateSpeed : -_accelerateSpeed;
    }
}
