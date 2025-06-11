using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IPlayerRunningLockable
{
    // SetPlayerRunningLock() を呼び出すためのインターフェース
}

public interface IPlayerGravityDisable
{
    // SetPlayerRunningLock() を呼び出すためのインターフェース
}

public class PlayerMovementManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerMovementRunning _playerMovementRunning;
    [SerializeField] private PlayerMovementLanding _playerMovementLanding;
    [SerializeField] private PlayerMovementSwap _playerMovementSwap;
    [SerializeField] private PlayerMovementOverhead _playerMovementOverhead;
    [SerializeField] private PlayerMovementGravity _playerMovementGravity;
    [SerializeField] private PlayerMovementTerminalVelocity _playerMovementTerminalVelocity;

    [HideInInspector] public bool IsFacingRight { get; private set; }

    private Rigidbody2D _playerRigidbody2D;
    private Dictionary<IPlayerRunningLockable, bool> _playerRunningLock = new Dictionary<IPlayerRunningLockable, bool>();
    private Dictionary<IPlayerGravityDisable, bool> _playerGravityDisable = new Dictionary<IPlayerGravityDisable, bool>();

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _playerRigidbody2D = _player.GetComponent<Rigidbody2D>();

        _playerMovementSwap.OnSwapCallback += SwapIsFacingRightValue;

        // TODO:マネージャー側から呼び出すようにする
        MovementInitialize(true);
    }

    // ----- Public Methods -----

    public void MovementInitialize(bool isFacingRight)
    {
        IsFacingRight = isFacingRight;
        _playerMovementOverhead.MovementInitialize();
        _playerMovementSwap.MovementInitialize();
        _playerMovementGravity.MovementInitialize();
    }

    public void MovementUpdate()
    {
        _playerMovementRunning.MovementUpdate(IsFacingRight, _playerRunningLock.Values.Any(x => x));
        _playerMovementLanding.MovementUpdate();
        _playerMovementSwap.MovementUpdate();
        _playerMovementOverhead.MovementUpdate();
        _playerMovementGravity.MovementUpdate(_playerGravityDisable.Values.Any(x => x));
        _playerMovementTerminalVelocity.MovementUpdate();
    }

    /// <summary>
    /// Playerの速度をゼロにする
    /// </summary>
    public void SetPlayerVelocityZero()
    {
        _playerRigidbody2D.linearVelocity = Vector2.zero;
    }

    /// <summary>
    /// PlayerのRunningの制限の状態を変更する
    /// </summary>
    public void SetPlayerRunningLock(IPlayerRunningLockable gameObject, bool isLock)
    {
        if (_playerRunningLock.ContainsKey(gameObject))
        {
            _playerRunningLock[gameObject] = isLock;
        }
        else
        {
            _playerRunningLock.Add(gameObject, isLock);
        }
    }

    /// <summary>
    /// PlayerのGravityの無効状態を変更する
    /// </summary>
    public void SetPlayerGravityDisable(IPlayerGravityDisable gameObject, bool isDisable)
    {
        if (_playerGravityDisable.ContainsKey(gameObject))
        {
            _playerGravityDisable[gameObject] = isDisable;
        }
        else
        {
            _playerGravityDisable.Add(gameObject, isDisable);
        }
    }

    /// <summary>
    /// /// SpeedAdjustCallbackへの登録
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

    // ----- Private Methods -----

    private void SwapIsFacingRightValue()
    {
        IsFacingRight = !IsFacingRight;
        _player.transform.transform.localScale = new Vector3(IsFacingRight ? 1 : -1, 1, 1);
    }
}
