using NUnit.Framework;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerMovementRunning _playerMovementRunning;
    [SerializeField] private PlayerMovementLanding _playerMovementLanding;
    [SerializeField] private PlayerMovementSwap _playerMovementSwap;
    [SerializeField] private PlayerMovementOverhead _playerMovementOverhead;
    [SerializeField] private PlayerMovementTerminalVelocity _playerMovementTerminalVelocity;

    [HideInInspector] public bool IsFacingRight { get; private set; }

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _playerMovementSwap.OnSwapCallback += SwapIsFacingRightValue;

        Initialize();
    }

    // ----- Public Methods -----

    public void Initialize()
    {
        IsFacingRight = true;
        _playerMovementOverhead.Initialize();
        _playerMovementSwap.Initialize();
    }

    public void FixedUpdate()
    {
        _playerMovementRunning.RunningUpdate(IsFacingRight);
        _playerMovementLanding.LandingUpdate();
        _playerMovementSwap.SwapUpdate();
        _playerMovementOverhead.OverheadUpdate();
        _playerMovementTerminalVelocity.TerminalVelocityUpdate();
    }

    // ----- Private Methods -----

    private void SwapIsFacingRightValue()
    {
        IsFacingRight = !IsFacingRight;
        _playerTransform.transform.localScale = new Vector3(IsFacingRight ? 1 : -1, 1, 1);
    }
}
