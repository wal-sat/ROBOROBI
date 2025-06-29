using UnityEngine;

public class PlayerActionNeutral_Crouch : PlayerActionBase
{
    [SerializeField] CapsuleCollider2D _playerCollider;
    [SerializeField] PlayerMovementSwap _playerMovementSwap;
    [SerializeField] PlayerMovementOverhead _playerMovementOverhead;
    [SerializeField] PlayerMovementBackside _playerMovementBackside;
    [SerializeField] PlayerMovementDieToGetStuck _playerMovementDieToGetStuck;
    [SerializeField] PlayerViewManager _playerViewManager;

    private const float StandOffsetY = -0.05f;
    private const float CrouchOffsetY = -0.175f;
    private const float SizeX = 0.4f;
    private const float StandSizeY = 0.9f;
    private const float CrouchSizeY = 0.65f;

    // ----- Public Methods -----

    public override void InitAction()
    {
        base.InitAction();

        _playerCollider.offset = new Vector2(0f, CrouchOffsetY);
        _playerCollider.size = new Vector2(SizeX, CrouchSizeY);
        _playerMovementSwap.ChangeSwapTransform(true);
        _playerMovementOverhead.ChangeOverheadTransform(true);
        _playerMovementBackside.ChangeBacksideTransform(true);
        _playerMovementDieToGetStuck.ChangeGetStuckTransform(true);
        _playerViewManager.IsCrouching = true;
    }

    public override void EndAction()
    {
        base.EndAction();

        _playerCollider.offset = new Vector2(0f, StandOffsetY);
        _playerCollider.size = new Vector2(SizeX, StandSizeY);
        _playerMovementSwap.ChangeSwapTransform(false);
        _playerMovementOverhead.ChangeOverheadTransform(false);
        _playerMovementBackside.ChangeBacksideTransform(false);
        _playerMovementDieToGetStuck.ChangeGetStuckTransform(false);
        _playerViewManager.IsCrouching = false;
    }

    public override void InitializeAction()
    {
        base.InitializeAction();

        _playerCollider.offset = new Vector2(0f, StandOffsetY);
        _playerCollider.size = new Vector2(SizeX, StandSizeY);
        _playerMovementSwap.ChangeSwapTransform(false);
        _playerMovementOverhead.ChangeOverheadTransform(false);
        _playerMovementBackside.ChangeBacksideTransform(false);
        _playerMovementDieToGetStuck.ChangeGetStuckTransform(false);
        _playerViewManager.IsCrouching = false;
    }
}
