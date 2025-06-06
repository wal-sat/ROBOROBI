using UnityEngine;

public class PlayerActionNeutral_Swap : PlayerActionBase
{
    [SerializeField] private PlayerMovementSwap _playerMovementSwap;

    public override void InitAction()
    {
        base.InitAction();

        _playerMovementSwap.OnSwapCallback();
    }
}
