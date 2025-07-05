using System;
using UnityEngine;

public class PlayerActionNeutralManager : MonoBehaviour
{
    [SerializeField] private GameScenePlayingInput _gameScenePlayingInput;
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private PlayerActionBase[] _neutralActions;

    private bool _wasActionGrab;
    private bool _wasActionCrouch;
    private bool _wasActionAccelerate;
    private bool _wasActionDecelerate;
    private bool _wasActionSwap;
    private bool _wasActionKick;
    private bool _wasActionInteractL;
    private bool _wasActionInteractR;

    // ----- Public Methods -----

    public void ActionInitialize()
    {

    }

    public void ActionUpdate()
    {
        // Trace Input Value
        Vector2 normalizedLeftDirection = _gameScenePlayingInput.NormalizedLeftDirection;
        bool isPushingL1 = _gameScenePlayingInput.IsPushingL1;
        bool isPushingR1 = _gameScenePlayingInput.IsPushingR1;
        bool isPushingL2 = _gameScenePlayingInput.IsPushingL2;
        bool isPushingR2 = _gameScenePlayingInput.IsPushingR2;
        bool isFacingRight = _playerMovementManager.IsFacingRight;

        // Input Check
        bool isActionGrab = false, isActionCrouch = false, isActionAccelerate = false, isActionDecelerate = false;
        bool isActionSwap = false, isActionKick = false, isActionInteractL = false, isActionInteractR = false;
        if (normalizedLeftDirection == Vector2.up && IsAcquiredAction(ActionKind.Neutral_Grab))
        {
            isActionGrab = true;
        }
        else if (normalizedLeftDirection == Vector2.down && IsAcquiredAction(ActionKind.Neutral_Crouch))
        {
            isActionCrouch = true;
        }
        else if (( (normalizedLeftDirection == Vector2.left && !isFacingRight) || (normalizedLeftDirection == Vector2.right && isFacingRight) ) && IsAcquiredAction(ActionKind.Neutral_Accelerate))
        {
            isActionAccelerate = true;
        }
        else if (( (normalizedLeftDirection == Vector2.left && isFacingRight) || (normalizedLeftDirection == Vector2.right && !isFacingRight) ) && IsAcquiredAction(ActionKind.Neutral_Decelerate))
        {
            isActionDecelerate = true;
        }
        if (( (isPushingL1 && isFacingRight) || (isPushingR1 && !isFacingRight) ) && IsAcquiredAction(ActionKind.Neutral_Swap))
        {
            isActionSwap = true;
        }
        if (( (isPushingL1 && !isFacingRight) || (isPushingR1 && isFacingRight) ) && IsAcquiredAction(ActionKind.Neutral_Kick))
        {
            isActionKick = true;
        }
        if (isPushingL2 && IsAcquiredAction(ActionKind.Neutral_InteractL))
        {
            isActionInteractL = true;
        }
        if (isPushingR2 && IsAcquiredAction(ActionKind.Neutral_InteractR))
        {
            isActionInteractR = true;
        }

        // Up : Grab Action
        if (isActionGrab && !_wasActionGrab)
        {
            _wasActionGrab = true;
            CallInitAction(ActionKind.Neutral_Grab);
        }
        else if (isActionGrab && _wasActionGrab)
        {
            CallInAction(ActionKind.Neutral_Grab);
        }
        else if (!isActionGrab && _wasActionGrab)
        {
            _wasActionGrab = false;
            CallEndAction(ActionKind.Neutral_Grab);
        }

        // Down : Crouch Action
        if (isActionCrouch && !_wasActionCrouch)
        {
            _wasActionCrouch = true;
            CallInitAction(ActionKind.Neutral_Crouch);
        }
        else if (isActionCrouch && _wasActionCrouch)
        {
            CallInAction(ActionKind.Neutral_Crouch);
        }
        else if (!isActionCrouch && _wasActionCrouch)
        {
            _wasActionCrouch = false;
            CallEndAction(ActionKind.Neutral_Crouch);
        }

        // Front : Accelerate Action
        if (isActionAccelerate && !_wasActionAccelerate)
        {
            _wasActionAccelerate = true;
            CallInitAction(ActionKind.Neutral_Accelerate);
        }
        else if (isActionAccelerate && _wasActionAccelerate)
        {
            CallInAction(ActionKind.Neutral_Accelerate);
        }
        else if (!isActionAccelerate && _wasActionAccelerate)
        {
            _wasActionAccelerate = false;
            CallEndAction(ActionKind.Neutral_Accelerate);
        }
        
        // Back : Decelerate Action
        if (isActionDecelerate && !_wasActionDecelerate)
        {
            _wasActionDecelerate = true;
            CallInitAction(ActionKind.Neutral_Decelerate);
        }
        else if (isActionDecelerate && _wasActionDecelerate)
        {
            CallInAction(ActionKind.Neutral_Decelerate);
        }
        else if (!isActionDecelerate && _wasActionDecelerate)
        {
            _wasActionDecelerate = false;
            CallEndAction(ActionKind.Neutral_Decelerate);
        }

        // LR Front : Kick Action
        if (isActionKick && !_wasActionKick)
        {
            _wasActionKick = true;
            CallInitAction(ActionKind.Neutral_Kick);
        }
        else if (isActionKick && _wasActionKick)
        {
            CallInAction(ActionKind.Neutral_Kick);
        }
        else if (!isActionKick && _wasActionKick)
        {
            _wasActionKick = false;
            CallEndAction(ActionKind.Neutral_Kick);
        }

        // LR Back : Swap Action
        if (isActionSwap && !_wasActionSwap)
        {
            _wasActionSwap = true;
            CallInitAction(ActionKind.Neutral_Swap);
        }
        else if (isActionSwap && _wasActionSwap)
        {
            CallInAction(ActionKind.Neutral_Swap);
        }
        else if (!isActionSwap && _wasActionSwap)
        {
            _wasActionSwap = false;
            CallEndAction(ActionKind.Neutral_Swap);
        }

        // L2 : InteractL Action
        if (isActionInteractL && !_wasActionInteractL)
        {
            _wasActionInteractL = true;
            CallInitAction(ActionKind.Neutral_InteractL);
        }
        else if (isActionInteractL && _wasActionInteractL)
        {
            CallInAction(ActionKind.Neutral_InteractL);
        }
        else if (!isActionInteractL && _wasActionInteractL)
        {
            _wasActionInteractL = false;
            CallEndAction(ActionKind.Neutral_InteractL);
        }

        // R2 : Kick Action
        if (isActionInteractR && !_wasActionInteractR)
        {
            _wasActionInteractR = true;
            CallInitAction(ActionKind.Neutral_InteractR);
        }
        else if (isActionInteractR && _wasActionInteractR)
        {
            CallInAction(ActionKind.Neutral_InteractR);
        }
        else if (!isActionInteractR && _wasActionInteractR)
        {
            _wasActionInteractR = false;
            CallEndAction(ActionKind.Neutral_InteractR);
        }
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
            if (action == null) continue;

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
            if (action == null) continue;

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
            if (action == null) continue;

            if (action.ActionKind == actionKind && action.IsAcquired)
            {
                action.EndAction();
            }
        }
    }
    
    /// <summary>
    /// 引数で与えられたアクションが習得済みであるかどうかを返す
    /// </summary>
    private bool IsAcquiredAction(ActionKind actionKind)
    {
        foreach (var action in _neutralActions)
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
