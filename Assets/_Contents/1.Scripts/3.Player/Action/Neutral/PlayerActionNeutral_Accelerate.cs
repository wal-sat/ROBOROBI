using Unity.VisualScripting;
using UnityEngine;

public class PlayerActionNeutral_Accelerate : PlayerActionBase
{
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private float _accelerateSpeed;

    // ----- Public Methods -----

    public override void InitAction()
    {
        base.InitAction();

        _playerMovementManager.SubscribeSpeedAdjustCallback(Accelerate);
    }

    public override void EndAction()
    {
        base.EndAction();

        _playerMovementManager.UnsubscribeSpeedAdjustCallback(Accelerate);
    }

    // ----- Private Methods -----

    private float Accelerate()
    {
        return _playerMovementManager.IsFacingRight ? _accelerateSpeed : -_accelerateSpeed;
    }
}
