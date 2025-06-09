using UnityEngine;

public class PlayerActionNeutralManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private PlayerActionBase _grabAction;
    [SerializeField] private PlayerActionBase _crouchAction;
    [SerializeField] private PlayerActionBase _accelerateAction;
    [SerializeField] private PlayerActionBase _decelerateAction;
    [SerializeField] private PlayerActionBase _swapAction;
    [SerializeField] private PlayerActionBase _kickAction;
    [SerializeField] private PlayerActionBase _interactLAction;
    [SerializeField] private PlayerActionBase _interactRAction;

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

    public void ActionUpdte(Vector2 leftDirection)
    {
        // Up : Grab Action
        if (leftDirection == Vector2.up && !_isPushingUp)
        {
            _isPushingUp = true;
            CallInitAction(_grabAction);
        }
        else if (leftDirection == Vector2.up && _isPushingUp)
        {
            CallInAction(_grabAction);
        }
        else if (leftDirection != Vector2.up && _isPushingUp)
        {
            _isPushingUp = false;
            CallEndAction(_grabAction);
        }

        // Down : Crouch Action
        if (leftDirection == Vector2.down && !_isPushingDown)
        {
            _isPushingDown = true;
            CallInitAction(_crouchAction);
        }
        else if (leftDirection == Vector2.down && _isPushingDown)
        {
            CallInAction(_crouchAction);
        }
        else if (leftDirection != Vector2.down && _isPushingDown)
        {
            _isPushingDown = false;
            CallEndAction(_crouchAction);
        }

        // L2 : InteractL Action
        if (S_InputSystemManager.Instance.IsPushingL2 && !_isPushingL2)
        {
            _isPushingL2 = true;
            CallInitAction(_interactLAction);
        }
        else if (S_InputSystemManager.Instance.IsPushingL2 && _isPushingL2)
        {
            CallInAction(_interactLAction);
        }
        else if (!S_InputSystemManager.Instance.IsPushingL2 && _isPushingL2)
        {
            _isPushingL2 = false;
            CallEndAction(_interactLAction);
        }

        // R2 : Kick Action
        if (S_InputSystemManager.Instance.IsPushingR2 && !_isPushingR2)
        {
            _isPushingR2 = true;
            CallInitAction(_interactRAction);
        }
        else if (S_InputSystemManager.Instance.IsPushingR2 && _isPushingR2)
        {
            CallInAction(_interactRAction);
        }
        else if (!S_InputSystemManager.Instance.IsPushingR2 && _isPushingR2)
        {
            _isPushingR2 = false;
            CallEndAction(_interactRAction);
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
                    CallEndAction(_accelerateAction);
                }
                if (_isPushingRight)
                {
                    _isPushingRight = false;
                    CallEndAction(_decelerateAction);
                }
                if (_isPushingL1)
                {
                    _isPushingL1 = false;
                    CallEndAction(_kickAction);
                }
                if (_isPushingR1)
                {
                    _isPushingR1 = false;
                    CallEndAction(_swapAction);
                }
            }

            // Left : Decelerate Action
            if (leftDirection == Vector2.left && !_isPushingLeft)
            {
                _isPushingLeft = true;
                CallInitAction(_decelerateAction);
            }
            else if (leftDirection == Vector2.left && _isPushingLeft)
            {
                CallInAction(_decelerateAction);
            }
            else if (leftDirection != Vector2.left && _isPushingLeft)
            {
                _isPushingLeft = false;
                CallEndAction(_decelerateAction);
            }

            // Right : Accelerate Action
            if (leftDirection == Vector2.right && !_isPushingRight)
            {
                _isPushingRight = true;
                CallInitAction(_accelerateAction);
            }
            else if (leftDirection == Vector2.right && _isPushingRight)
            {
                CallInAction(_accelerateAction);
            }
            else if (leftDirection != Vector2.right && _isPushingRight)
            {
                _isPushingRight = false;
                CallEndAction(_accelerateAction);
            }

            // L1 : Swap Action
            if (S_InputSystemManager.Instance.IsPushingL1 && !_isPushingL1)
            {
                _isPushingL1 = true;
                CallInitAction(_swapAction);
            }
            else if (S_InputSystemManager.Instance.IsPushingL1 && _isPushingL1)
            {
                CallInAction(_swapAction);
            }
            else if (!S_InputSystemManager.Instance.IsPushingL1 && _isPushingL1)
            {
                _isPushingL1 = false;
                CallEndAction(_swapAction);
            }

            // R1 : Kick Action
            if (S_InputSystemManager.Instance.IsPushingR1 && !_isPushingR1)
            {
                _isPushingR1 = true;
                CallInitAction(_kickAction);
            }
            else if (S_InputSystemManager.Instance.IsPushingR1 && _isPushingR1)
            {
                CallInAction(_kickAction);
            }
            else if (!S_InputSystemManager.Instance.IsPushingR1 && _isPushingR1)
            {
                _isPushingR1 = false;
                CallEndAction(_kickAction);
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
                    CallEndAction(_decelerateAction);
                }
                if (_isPushingRight)
                {
                    _isPushingRight = false;
                    CallEndAction(_accelerateAction);
                }
                if (_isPushingL1)
                {
                    _isPushingL1 = false;
                    CallEndAction(_swapAction);
                }
                if (_isPushingR1)
                {
                    _isPushingR1 = false;
                    CallEndAction(_kickAction);
                }
            }

            // Left : Accelerate Action
            if (leftDirection == Vector2.left && !_isPushingLeft)
            {
                _isPushingLeft = true;
                CallInitAction(_accelerateAction);
            }
            else if (leftDirection == Vector2.left && _isPushingLeft)
            {
                CallInAction(_accelerateAction);
            }
            else if (leftDirection != Vector2.left && _isPushingLeft)
            {
                _isPushingLeft = false;
                CallEndAction(_accelerateAction);
            }

            // Right : Decelerate Action
            if (leftDirection == Vector2.right && !_isPushingRight)
            {
                _isPushingRight = true;
                CallInitAction(_decelerateAction);
            }
            else if (leftDirection == Vector2.right && _isPushingRight)
            {
                CallInAction(_decelerateAction);
            }
            else if (leftDirection != Vector2.right && _isPushingRight)
            {
                _isPushingRight = false;
                CallEndAction(_decelerateAction);
            }

            // L1 : Kick Action
            if (S_InputSystemManager.Instance.IsPushingL1 && !_isPushingL1)
            {
                _isPushingL1 = true;
                CallInitAction(_kickAction);
            }
            else if (S_InputSystemManager.Instance.IsPushingL1 && _isPushingL1)
            {
                CallInAction(_kickAction);
            }
            else if (!S_InputSystemManager.Instance.IsPushingL1 && _isPushingL1)
            {
                _isPushingL1 = false;
                CallEndAction(_kickAction);
            }

            // R1 : Swap Action
            if (S_InputSystemManager.Instance.IsPushingR1 && !_isPushingR1)
            {
                _isPushingR1 = true;
                CallInitAction(_swapAction);
            }
            else if (S_InputSystemManager.Instance.IsPushingR1 && _isPushingR1)
            {
                CallInAction(_swapAction);
            }
            else if (!S_InputSystemManager.Instance.IsPushingR1 && _isPushingR1)
            {
                _isPushingR1 = false;
                CallEndAction(_swapAction);
            }
        }

        _wasFacingRight = isFacingRight;
    }

    // ----- Private Methods -----

    private void CallInitAction(PlayerActionBase action)
    {
        if (action.IsAcquired)
        {
            action.InitAction();
        }
    }
    private void CallInAction(PlayerActionBase action)
    {
        if (action.IsAcquired)
        {
            action.InAction();
        }
    }
    private void CallEndAction(PlayerActionBase action)
    {
        if (action.IsAcquired)
        {
            action.EndAction();
        }
    }
}
