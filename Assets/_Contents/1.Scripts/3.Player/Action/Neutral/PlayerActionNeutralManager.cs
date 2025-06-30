using UnityEngine;

public class PlayerActionNeutralManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private PlayerActionBase[] _neutralActions;

    private bool _isPushingUp;
    private bool _isPushingDown;
    private bool _isPushingLeft;
    private bool _isPushingRight;
    private bool _isPushingL1;
    private bool _isPushingR1;
    private bool _isPushingL2;
    private bool _isPushingR2;
    private bool _wasFacingRight;

    // ----- Public Methods -----

    public void ActionInitialize(bool isFacingRight)
    {
        _isPushingUp = false;
        _isPushingDown = false;
        _isPushingLeft = false;
        _isPushingRight = false;
        _isPushingL1 = false;
        _isPushingR1 = false;
        _isPushingL2 = false;
        _isPushingR2 = false;
        _wasFacingRight = isFacingRight;
    }

    public void ActionUpdate(Vector2 leftDirection)
    {
        // Up : Grab Action
        if (leftDirection == Vector2.up && !_isPushingUp)
        {
            _isPushingUp = true;
            CallInitAction(ActionKind.Neutral_Grab);
        }
        else if (leftDirection == Vector2.up && _isPushingUp)
        {
            CallInAction(ActionKind.Neutral_Grab);
        }
        else if (leftDirection != Vector2.up && _isPushingUp)
        {
            _isPushingUp = false;
            CallEndAction(ActionKind.Neutral_Grab);
        }

        // Down : Crouch Action
        if (leftDirection == Vector2.down && !_isPushingDown)
        {
            _isPushingDown = true;
            CallInitAction(ActionKind.Neutral_Crouch);
        }
        else if (leftDirection == Vector2.down && _isPushingDown)
        {
            CallInAction(ActionKind.Neutral_Crouch);
        }
        else if (leftDirection != Vector2.down && _isPushingDown)
        {
            _isPushingDown = false;
            CallEndAction(ActionKind.Neutral_Crouch);
        }

        // L2 : InteractL Action
        if (S_InputSystemManager.Instance.IsPushingL2 && !_isPushingL2)
        {
            _isPushingL2 = true;
            CallInitAction(ActionKind.Neutral_InteractL);
        }
        else if (S_InputSystemManager.Instance.IsPushingL2 && _isPushingL2)
        {
            CallInAction(ActionKind.Neutral_InteractL);
        }
        else if (!S_InputSystemManager.Instance.IsPushingL2 && _isPushingL2)
        {
            _isPushingL2 = false;
            CallEndAction(ActionKind.Neutral_InteractL);
        }

        // R2 : Kick Action
        if (S_InputSystemManager.Instance.IsPushingR2 && !_isPushingR2)
        {
            _isPushingR2 = true;
            CallInitAction(ActionKind.Neutral_InteractR);
        }
        else if (S_InputSystemManager.Instance.IsPushingR2 && _isPushingR2)
        {
            CallInAction(ActionKind.Neutral_InteractR);
        }
        else if (!S_InputSystemManager.Instance.IsPushingR2 && _isPushingR2)
        {
            _isPushingR2 = false;
            CallEndAction(ActionKind.Neutral_InteractR);
        }

        bool isFacingRight = _playerMovementManager.IsFacingRight;
        if (isFacingRight)
        {
            // Swap時のEnd処理
            if (!_wasFacingRight)
            {
                if (_isPushingLeft)
                {
                    _isPushingLeft = false;
                    CallEndAction(ActionKind.Neutral_Accelerate);
                }
                if (_isPushingRight)
                {
                    _isPushingRight = false;
                    CallEndAction(ActionKind.Neutral_Decelerate);
                }
                if (_isPushingL1)
                {
                    _isPushingL1 = false;
                    CallEndAction(ActionKind.Neutral_Kick);
                }
                if (_isPushingR1)
                {
                    _isPushingR1 = false;
                    CallEndAction(ActionKind.Neutral_Swap);
                }
            }

            // Left : Decelerate Action
            if (leftDirection == Vector2.left && !_isPushingLeft)
            {
                _isPushingLeft = true;
                CallInitAction(ActionKind.Neutral_Decelerate);
            }
            else if (leftDirection == Vector2.left && _isPushingLeft)
            {
                CallInAction(ActionKind.Neutral_Decelerate);
            }
            else if (leftDirection != Vector2.left && _isPushingLeft)
            {
                _isPushingLeft = false;
                CallEndAction(ActionKind.Neutral_Decelerate);
            }

            // Right : Accelerate Action
            if (leftDirection == Vector2.right && !_isPushingRight)
            {
                _isPushingRight = true;
                CallInitAction(ActionKind.Neutral_Accelerate);
            }
            else if (leftDirection == Vector2.right && _isPushingRight)
            {
                CallInAction(ActionKind.Neutral_Accelerate);
            }
            else if (leftDirection != Vector2.right && _isPushingRight)
            {
                _isPushingRight = false;
                CallEndAction(ActionKind.Neutral_Accelerate);
            }

            // L1 : Swap Action
            if (S_InputSystemManager.Instance.IsPushingL1 && !_isPushingL1)
            {
                _isPushingL1 = true;
                CallInitAction(ActionKind.Neutral_Swap);
            }
            else if (S_InputSystemManager.Instance.IsPushingL1 && _isPushingL1)
            {
                CallInAction(ActionKind.Neutral_Swap);
            }
            else if (!S_InputSystemManager.Instance.IsPushingL1 && _isPushingL1)
            {
                _isPushingL1 = false;
                CallEndAction(ActionKind.Neutral_Swap);
            }

            // R1 : Kick Action
            if (S_InputSystemManager.Instance.IsPushingR1 && !_isPushingR1)
            {
                _isPushingR1 = true;
                CallInitAction(ActionKind.Neutral_Kick);
            }
            else if (S_InputSystemManager.Instance.IsPushingR1 && _isPushingR1)
            {
                CallInAction(ActionKind.Neutral_Kick);
            }
            else if (!S_InputSystemManager.Instance.IsPushingR1 && _isPushingR1)
            {
                _isPushingR1 = false;
                CallEndAction(ActionKind.Neutral_Kick);
            }
        }
        else if (!isFacingRight)
        {
            // Swap時のEnd処理
            if (_wasFacingRight)
            {
                if (_isPushingLeft)
                {
                    _isPushingLeft = false;
                    CallEndAction(ActionKind.Neutral_Decelerate);
                }
                if (_isPushingRight)
                {
                    _isPushingRight = false;
                    CallEndAction(ActionKind.Neutral_Accelerate);
                }
                if (_isPushingL1)
                {
                    _isPushingL1 = false;
                    CallEndAction(ActionKind.Neutral_Swap);
                }
                if (_isPushingR1)
                {
                    _isPushingR1 = false;
                    CallEndAction(ActionKind.Neutral_Kick);
                }
            }

            // Left : Accelerate Action
            if (leftDirection == Vector2.left && !_isPushingLeft)
            {
                _isPushingLeft = true;
                CallInitAction(ActionKind.Neutral_Accelerate);
            }
            else if (leftDirection == Vector2.left && _isPushingLeft)
            {
                CallInAction(ActionKind.Neutral_Accelerate);
            }
            else if (leftDirection != Vector2.left && _isPushingLeft)
            {
                _isPushingLeft = false;
                CallEndAction(ActionKind.Neutral_Accelerate);
            }

            // Right : Decelerate Action
            if (leftDirection == Vector2.right && !_isPushingRight)
            {
                _isPushingRight = true;
                CallInitAction(ActionKind.Neutral_Decelerate);
            }
            else if (leftDirection == Vector2.right && _isPushingRight)
            {
                CallInAction(ActionKind.Neutral_Decelerate);
            }
            else if (leftDirection != Vector2.right && _isPushingRight)
            {
                _isPushingRight = false;
                CallEndAction(ActionKind.Neutral_Decelerate);
            }

            // L1 : Kick Action
            if (S_InputSystemManager.Instance.IsPushingL1 && !_isPushingL1)
            {
                _isPushingL1 = true;
                CallInitAction(ActionKind.Neutral_Kick);
            }
            else if (S_InputSystemManager.Instance.IsPushingL1 && _isPushingL1)
            {
                CallInAction(ActionKind.Neutral_Kick);
            }
            else if (!S_InputSystemManager.Instance.IsPushingL1 && _isPushingL1)
            {
                _isPushingL1 = false;
                CallEndAction(ActionKind.Neutral_Kick);
            }

            // R1 : Swap Action
            if (S_InputSystemManager.Instance.IsPushingR1 && !_isPushingR1)
            {
                _isPushingR1 = true;
                CallInitAction(ActionKind.Neutral_Swap);
            }
            else if (S_InputSystemManager.Instance.IsPushingR1 && _isPushingR1)
            {
                CallInAction(ActionKind.Neutral_Swap);
            }
            else if (!S_InputSystemManager.Instance.IsPushingR1 && _isPushingR1)
            {
                _isPushingR1 = false;
                CallEndAction(ActionKind.Neutral_Swap);
            }
        }

        _wasFacingRight = isFacingRight;
    }

    public void SetAcquireAction(ActionKind actionKind, bool isAcquired)
    {
        foreach (var action in _neutralActions)
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
    }

    // ----- Private Methods -----

    private void CallInitAction(ActionKind actionKind)
    {
        foreach (var action in _neutralActions)
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
        foreach (var action in _neutralActions)
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
        foreach (var action in _neutralActions)
        {
            if (action == null) return;

            if (action.ActionKind == actionKind && action.IsAcquired)
            {
                action.EndAction();
            }
        }
    }
}
