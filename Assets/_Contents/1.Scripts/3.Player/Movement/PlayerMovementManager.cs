using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IPlayerMovementPropertyLockable
{
    // PlayerMovementManagerに登録されているかどうか
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
    [SerializeField] private PlayerMovementDieToGetStuck _playerMovementDieToGetStuck;

    [HideInInspector] public bool IsFacingRight { get; private set; }

    private Rigidbody2D _playerRigidbody2D;
    private Dictionary<IPlayerMovementPropertyLockable, bool> _playerRunningLock = new Dictionary<IPlayerMovementPropertyLockable, bool>();
    private Dictionary<IPlayerMovementPropertyLockable, bool> _playerLandingLockable = new Dictionary<IPlayerMovementPropertyLockable, bool>();
    private Dictionary<IPlayerMovementPropertyLockable, bool> _playerSwapLockable = new Dictionary<IPlayerMovementPropertyLockable, bool>();
    private Dictionary<IPlayerMovementPropertyLockable, bool> _playerOverheadLockable = new Dictionary<IPlayerMovementPropertyLockable, bool>();
    private Dictionary<IPlayerMovementPropertyLockable, bool> _playerGravityLockable = new Dictionary<IPlayerMovementPropertyLockable, bool>();
    private Dictionary<IPlayerMovementPropertyLockable, bool> _playerTerminalVelocityLockable = new Dictionary<IPlayerMovementPropertyLockable, bool>();
    private Dictionary<IPlayerMovementPropertyLockable, bool> _playerDieToGetStuckLockable = new Dictionary<IPlayerMovementPropertyLockable, bool>();

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
        _playerMovementGravity.MovementInitialize();
    }

    public void MovementUpdate()
    {
        _playerMovementRunning.MovementUpdate(IsFacingRight, _playerRunningLock.Values.Any(x => x));
        _playerMovementLanding.MovementUpdate(_playerLandingLockable.Values.Any(x => x));
        _playerMovementSwap.MovementUpdate(_playerSwapLockable.Values.Any(x => x));
        _playerMovementOverhead.MovementUpdate(_playerOverheadLockable.Values.Any(x => x));
        _playerMovementGravity.MovementUpdate(_playerGravityLockable.Values.Any(x => x));
        _playerMovementTerminalVelocity.MovementUpdate(_playerTerminalVelocityLockable.Values.Any(x => x));
        _playerMovementDieToGetStuck.MovementUpdate(_playerDieToGetStuckLockable.Values.Any(x => x));
    }

    public void SetPlayerVelocityZero()
    {
        _playerRigidbody2D.linearVelocity = Vector2.zero;
    }

    public void SetPlayerMovementPropertyLockable(IPlayerMovementPropertyLockable gameObject, bool isLock)
    {
        SetPlayerRunningLock(gameObject, isLock);
        SetPlayerLandingLockable(gameObject, isLock);
        SetPlayerSwapLockable(gameObject, isLock);
        SetPlayerOverheadLockable(gameObject, isLock);
        SetPlayerGravityLockable(gameObject, isLock);
        SetPlayerTerminalVelocityLockable(gameObject, isLock);
        SetPlayerDieToGetStuckLockable(gameObject, isLock);
    }
    public void SetPlayerRunningLock(IPlayerMovementPropertyLockable gameObject, bool isLock)
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
    public void SetPlayerLandingLockable(IPlayerMovementPropertyLockable gameObject, bool isLock)
    {
        if (_playerLandingLockable.ContainsKey(gameObject))
        {
            _playerLandingLockable[gameObject] = isLock;
        }
        else
        {
            _playerLandingLockable.Add(gameObject, isLock);
        }
    }
    public void SetPlayerSwapLockable(IPlayerMovementPropertyLockable gameObject, bool isLock)
    {
        if (_playerSwapLockable.ContainsKey(gameObject))
        {
            _playerSwapLockable[gameObject] = isLock;
        }
        else
        {
            _playerSwapLockable.Add(gameObject, isLock);
        }
    }
    public void SetPlayerOverheadLockable(IPlayerMovementPropertyLockable gameObject, bool isLock)
    {
        if (_playerOverheadLockable.ContainsKey(gameObject))
        {
            _playerOverheadLockable[gameObject] = isLock;
        }
        else
        {
            _playerOverheadLockable.Add(gameObject, isLock);
        }
    }
    public void SetPlayerGravityLockable(IPlayerMovementPropertyLockable gameObject, bool isLock)
    {
        if (_playerGravityLockable.ContainsKey(gameObject))
        {
            _playerGravityLockable[gameObject] = isLock;
        }
        else
        {
            _playerGravityLockable.Add(gameObject, isLock);
        }
    }
    public void SetPlayerTerminalVelocityLockable(IPlayerMovementPropertyLockable gameObject, bool isLock)
    {
        if (_playerTerminalVelocityLockable.ContainsKey(gameObject))
        {
            _playerTerminalVelocityLockable[gameObject] = isLock;
        }
        else
        {
            _playerTerminalVelocityLockable.Add(gameObject, isLock);
        }
    }
    public void SetPlayerDieToGetStuckLockable(IPlayerMovementPropertyLockable gameObject, bool isLock)
    {
        if (_playerDieToGetStuckLockable.ContainsKey(gameObject))
        {
            _playerDieToGetStuckLockable[gameObject] = isLock;
        }
        else
        {
            _playerDieToGetStuckLockable.Add(gameObject, isLock);
        }
    }

    /// <summary>
    /// /// SpeedAdjustCallbackへの登録
    /// </summary>
    public void SubscribeSpeedAdjustCallback(Func<float> func)
    {
        if (!_playerMovementRunning.SpeedAdjustCallbackList.Contains(func))
        {
            _playerMovementRunning.SpeedAdjustCallbackList.Add(func);
        }
    }
    public void UnsubscribeSpeedAdjustCallback(Func<float> func)
    {
        if (_playerMovementRunning.SpeedAdjustCallbackList.Contains(func))
        {
            _playerMovementRunning.SpeedAdjustCallbackList.Remove(func);
        }
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
