using System;
using NaughtyAttributes;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerActionSManager : MonoBehaviour
{
    [SerializeField] private PlayerActionBase[] _sActions;

    private const float InputBuffer = 0.05f;

    private bool _isPushingNone;
    private bool _isPushingUp;
    private bool _isPushingDown;
    private bool _wasJumped;
    private float _bufferTimer;

    private int _maxJumpTime;
    private int _jumpTime;
    private bool _isRecoveryJumpTime;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        ;
    }

    // ----- Public Methods -----

    public void ActionUpdate(Vector2 leftDirection)
    {
        // S + Down : Go Down Action
        if (leftDirection == Vector2.down && !_isPushingDown)
        {
            _isPushingDown = true;
            CallInitAction(ActionKind.S_GoDown);
        }
        else if (leftDirection == Vector2.down && _isPushingDown)
        {
            CallInAction(ActionKind.S_GoDown);
        }
        else if (leftDirection != Vector2.down && _isPushingDown)
        {
            _isPushingDown = false;
            _bufferTimer = 0f;
            CallEndAction(ActionKind.S_GoDown);
        }

        _bufferTimer += Time.deltaTime;
        if (_bufferTimer <= InputBuffer) return;

        // S + Up : Big Jump Action
        if (leftDirection == Vector2.up && !_isPushingUp && !_wasJumped && IsJumpable())
        {
            _isPushingUp = true;
            _wasJumped = true;
            CallInitAction(ActionKind.S_BigJump);
        }
        else if (leftDirection == Vector2.up && _isPushingUp)
        {
            CallInAction(ActionKind.S_BigJump);
        }
        else if (leftDirection != Vector2.up && _isPushingUp)
        {
            _isPushingUp = false;
            DecrementJumpTime();
            CallEndAction(ActionKind.S_BigJump);
        }

        // S : Jump Action
        if ((leftDirection == Vector2.zero || leftDirection == Vector2.left || leftDirection == Vector2.right) && !_isPushingNone && !_wasJumped && IsJumpable())
        {
            _isPushingNone = true;
            _wasJumped = true;
            CallInitAction(ActionKind.S_Jump);
        }
        else if ((leftDirection == Vector2.zero || leftDirection == Vector2.left || leftDirection == Vector2.right) && _isPushingNone)
        {
            CallInAction(ActionKind.S_Jump);
        }
        else if ((leftDirection != Vector2.zero && leftDirection != Vector2.left && leftDirection != Vector2.right) && _isPushingNone)
        {
            _isPushingNone = false;
            DecrementJumpTime();
            CallEndAction(ActionKind.S_Jump);
        }
    }

    public void ActionEnd()
    {
        _wasJumped = false;
        _bufferTimer = 0f;

        if (_isPushingUp)
        {
            _isPushingUp = false;
            DecrementJumpTime();
            CallEndAction(ActionKind.S_BigJump);
        }
        if (_isPushingDown)
        {
            _isPushingDown = false;
            CallEndAction(ActionKind.S_GoDown);
        }
        if (_isPushingNone)
        {
            _isPushingNone = false;
            DecrementJumpTime();
            CallEndAction(ActionKind.S_Jump);
        }
    }

    public void SetAcquireAction(ActionKind actionKind, bool isAcquired)
    {
        foreach (var action in _sActions)
        {
            if (action.ActionKind == actionKind)
            {
                if (action.IsAcquired && !isAcquired)
                {
                    action.InitializeAction();
                }
                action.IsAcquired = isAcquired;
            }
        }

        SetMaxJumpTime();
    }

    public void RecoveryJumpTime()
    {
        _jumpTime = _maxJumpTime;

        if (_isPushingUp || _isPushingNone)
        {
            _isRecoveryJumpTime = true;
        }
    }

    public void DepleteJumpTime()
    {
        _jumpTime = 0;
    }

    // ----- Private Methods -----

    private void CallInitAction(ActionKind actionKind)
    {
        foreach (var action in _sActions)
        {
            if (action == null) return;

            if (action.ActionKind == actionKind && action.IsAcquired)
            {
                action.InitAction();
            }
        }
    }
    private void CallInAction(ActionKind actionKind)
    {
        foreach (var action in _sActions)
        {
            if (action == null) return;

            if (action.ActionKind == actionKind && action.IsAcquired)
            {
                action.InAction();
            }
        }
    }
    private void CallEndAction(ActionKind actionKind)
    {
        foreach (var action in _sActions)
        {
            if (action == null) return;

            if (action.ActionKind == actionKind && action.IsAcquired)
            {
                action.EndAction();
            }
        }
    }

    private void SetMaxJumpTime()
    {
        int newMaxJumpTime = 1;
        foreach (var action in _sActions)
        {
            if (action.ActionKind == ActionKind.S_DoubleJump && action.IsAcquired && newMaxJumpTime == 1)
            {
                newMaxJumpTime = 2;
            }
            else if (action.ActionKind == ActionKind.S_InfiniteJump && action.IsAcquired)
            {
                newMaxJumpTime = -1;
            }
        }

        if (_jumpTime < _maxJumpTime)
        {
            _jumpTime = Math.Max(newMaxJumpTime - (_maxJumpTime - _jumpTime), 0);
        }

        _maxJumpTime = newMaxJumpTime;
    }

    private void DecrementJumpTime()
    {
        if (!_isRecoveryJumpTime && _jumpTime > 0)
        {
            _jumpTime--;
        }
        else
        {
            _isRecoveryJumpTime = false;
        }
    }

    /// <summary>
    /// _jumpTimeからジャンプ可能かどうかを返す
    /// </summary>
    private bool IsJumpable()
    {
        if (_jumpTime == -1) return true;
        if (_jumpTime > 0) return true;

        return false;
    }
}
