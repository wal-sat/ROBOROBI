using Unity.VisualScripting;
using UnityEngine;

public class PlayerActionNeutral_Decelerate : PlayerActionBase
{
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private float _decelerateSpeed;

    // ----- Public Methods -----

    public override void InitAction()
    {
        base.InitAction();

        _playerMovementManager.SubscribeSpeedAdjustCallback(Decelerate);
    }

    public override void EndAction()
    {
        base.EndAction();

        _playerMovementManager.UnsubscribeSpeedAdjustCallback(Decelerate);
    }

    public override void InitializeAction()
    {
        base.InitializeAction();

        _playerMovementManager.UnsubscribeSpeedAdjustCallback(Decelerate);
    }

    // ----- Private Methods -----

    private float Decelerate()
    {
        return _playerMovementManager.IsFacingRight ? -_decelerateSpeed : _decelerateSpeed;
    }
}
