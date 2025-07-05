using System;
using NaughtyAttributes;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerActionSManager : MonoBehaviour
{
    [SerializeField] private GameScenePlayingInput _gameScenePlayingInput;
    [SerializeField] private PlayerActionBase[] _sActions;

    private const float InputBuffer = 0.05f;

    private bool _wasActionGoDownPast;
    private bool _wasActionBigJumpPast;
    private bool _wasActionJumpPast;

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

    public void ActionUpdate()
    {
        // Trace Input Value
        bool isPushingS = _gameScenePlayingInput.IsPushingS;
        Vector2 normalizedLeftDirection = _gameScenePlayingInput.NormalizedLeftDirection;

        // Input Check
        bool _isActionGoDown = false, _isActionBigJump = false, _isActionJump = false;
        if (isPushingS && normalizedLeftDirection == Vector2.down && IsAcquiredAction(ActionKind.S_GoDown))
        {
            _isActionGoDown = true;
        }
        else if (isPushingS && normalizedLeftDirection == Vector2.up && IsAcquiredAction(ActionKind.S_BigJump))
        {
            _isActionBigJump = true;
        }
        else if (isPushingS && IsAcquiredAction(ActionKind.S_Jump))
        {
            _isActionJump = true;
        }

        // Count Buffer Time
        if (_isActionJump || _isActionBigJump || _isActionGoDown)
        {
            _bufferTimer += Time.deltaTime;
        }

        // S + Down : Go Down Action
        if (_isActionGoDown && !_wasActionGoDownPast)
        {
            _wasActionGoDownPast = true;
            CallInitAction(ActionKind.S_GoDown);
        }
        else if (_isActionGoDown && _wasActionGoDownPast)
        {
            CallInAction(ActionKind.S_GoDown);
        }
        else if (!_isActionGoDown && _wasActionGoDownPast)
        {
            _wasActionGoDownPast = false;
            CallEndAction(ActionKind.S_GoDown);

            _bufferTimer = 0f;
        }

        // Buffer Time Return
        if (_bufferTimer <= InputBuffer) return;

        // S + Up : Big Jump Action
        if (_isActionBigJump && !_wasActionBigJumpPast && IsJumpable())
        {
            _wasActionBigJumpPast = true;
            CallInitAction(ActionKind.S_BigJump);
        }
        else if (_isActionBigJump && _wasActionBigJumpPast)
        {
            CallInAction(ActionKind.S_BigJump);
        }
        else if (!_isActionBigJump && _wasActionBigJumpPast)
        {
            _wasActionBigJumpPast = false;
            CallEndAction(ActionKind.S_BigJump);

            DecrementJumpTime();
            _bufferTimer = 0;
        }

        // S : Jump Action
        if (_isActionJump && !_wasActionJumpPast && IsJumpable())
        {
            _wasActionJumpPast = true;
            CallInitAction(ActionKind.S_Jump);
        }
        else if (_isActionJump && _wasActionJumpPast)
        {
            CallInAction(ActionKind.S_Jump);
        }
        else if (!_isActionJump && _wasActionJumpPast)
        {
            _wasActionJumpPast = false;
            CallEndAction(ActionKind.S_Jump);

            DecrementJumpTime();
            _bufferTimer = 0;
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

        if (_wasActionJumpPast || _wasActionBigJumpPast)
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
            if (action == null) continue;

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
            if (action == null) continue;

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
            if (action == null) continue;

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

    /// <summary>
    /// 引数で与えられたアクションが習得済みであるかどうかを返す
    /// </summary>
    private bool IsAcquiredAction(ActionKind actionKind)
    {
        foreach (var action in _sActions)
        {
            if (action == null) continue;

            if (action.ActionKind == actionKind)
            {
                return action.IsAcquired;
            }
        }

        return false;
    }
}
