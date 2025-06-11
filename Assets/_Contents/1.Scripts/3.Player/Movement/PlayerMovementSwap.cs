using System;
using UnityEngine;

public class PlayerMovementSwap : MonoBehaviour
{
    [SerializeField] private Transform _swapCheckerTransform;
    [SerializeField] private LayerMask _groundLayer;

    public Action OnSwapCallback;

    private const float CapsulePositionX = 0.2f;
    private const float StandCapsulePositionY = -0.05f;
    private const float CrouchCapsulePositionY = -0.175f;
    private const float CapsuleSizeX = 0.1f;
    private const float StandCapsuleSizeY = 0.35f;
    private const float CrouchCapsuleSizeY = 0.225f;

    private Vector3 _swapCapsuleSize;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        OnSwapCallback += () => S_SEManager.Instance.Play("p_swap");
    }

    // ----- Public Methods -----

    public void MovementInitialize()
    {
        ChangeSwapTransform(false);
    }

    public void MovementUpdate()
    {
        if (Physics2D.OverlapCapsule(_swapCheckerTransform.position, _swapCapsuleSize, CapsuleDirection2D.Vertical, 0, _groundLayer) != null)
        {
            OnSwapCallback();
        }
    }

    public void ChangeSwapTransform(bool isCrouching)
    {
        if (isCrouching)
        {
            _swapCheckerTransform.localPosition = new Vector3(CapsulePositionX, CrouchCapsulePositionY, 0f);
            _swapCapsuleSize = new Vector3(CapsuleSizeX, CrouchCapsuleSizeY, 0f);
        }
        else
        {
            _swapCheckerTransform.localPosition = new Vector3(CapsulePositionX, StandCapsulePositionY, 0f);
            _swapCapsuleSize = new Vector3(CapsuleSizeX, StandCapsuleSizeY, 0f);
        }
    }
}
