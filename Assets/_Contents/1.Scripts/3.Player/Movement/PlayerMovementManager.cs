using System;
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

        MovementInitialize();
    }

    // ----- Public Methods -----

    public void MovementInitialize()
    {
        IsFacingRight = true;
        _playerMovementOverhead.Initialize();
        _playerMovementSwap.Initialize();
    }

    public void MovementUpdate()
    {
        _playerMovementRunning.RunningUpdate(IsFacingRight);
        _playerMovementLanding.LandingUpdate();
        _playerMovementSwap.SwapUpdate();
        _playerMovementOverhead.OverheadUpdate();
        _playerMovementTerminalVelocity.TerminalVelocityUpdate();
    }

    /// <summary>
    /// OnLandingCallbackへの登録
    /// </summary>
    // public void SubscribeLandingCallback(Action action)
    // {
    //     _playerMovementLanding.OnLandingCallback -= action;
    //     _playerMovementLanding.OnLandingCallback += action;
    // }
    // public void UnsubscribeLandingCallback(Action action)
    // {
    //     _playerMovementLanding.OnLandingCallback -= action;
    // }

    /// <summary>
    /// OnSwapCallbackへの登録
    /// </summary>
    public void SubscribeSwapCallback(Action action)
    {
        _playerMovementSwap.OnSwapCallback -= action;
        _playerMovementSwap.OnSwapCallback += action;
    }
    public void UnsubscribeSwapCallback(Action action)
    {
        _playerMovementSwap.OnSwapCallback -= action;
    }

    /// <summary>
    /// SpeedAdjustCallbackへの登録
    /// </summary>
    public void SubscribeSpeedAdjustCallback(Func<float> action)
    {
        _playerMovementRunning.SpeedAdjustCallback -= action;
        _playerMovementRunning.SpeedAdjustCallback += action;
    }
    public void UnsubscribeSpeedAdjustCallback(Func<float> action)
    {
        _playerMovementRunning.SpeedAdjustCallback -= action;
    }

    // ----- Private Methods -----

    private void SwapIsFacingRightValue()
    {
        IsFacingRight = !IsFacingRight;
        _playerTransform.transform.localScale = new Vector3(IsFacingRight ? 1 : -1, 1, 1);
    }
}
