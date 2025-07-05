using System;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public enum ActionKind
{
    Neutral_Grab, Neutral_Accelerate, Neutral_Decelerate, Neutral_Crouch, Neutral_Swap, Neutral_Kick, Neutral_InteractL, Neutral_InteractR,
    S_Jump, S_BigJump, S_GoDown, S_DoubleJump, S_InfiniteJump,
}

public class PlayerActionManager : MonoBehaviour
{
    [SerializeField] private GameScenePlayingInput _gameScenePlayingInput;
    [SerializeField] private PlayerActionNeutralManager _playerActionNeutralManager;
    [SerializeField] private PlayerActionSManager _playerActionSManager;
    [SerializeField] private PlayerMovementManager _playerMovementManager;

    // ----- Public Methods -----

    public void ActionInitialize()
    {

    }

    public void ActionUpdate(bool isActivePlayer)
    {
        if (!isActivePlayer) return;

        // Get IsLanding
        bool isLanding = _playerMovementManager.IsLanding;

        // Neutral
        _playerActionNeutralManager.ActionUpdate();

        // S Button
        _playerActionSManager.ActionUpdate(isLanding);
    }

    public void SetAcquiredAction(AcquiredActionData acquiredActionData)
    {
        foreach (var acquiredAction in acquiredActionData.AcquiredActionDictionary)
        {
            string actionKindString = acquiredAction.Key.ToString();

            if (actionKindString.StartsWith("Neutral_"))
            {
                _playerActionNeutralManager.SetAcquireAction(acquiredAction.Key, acquiredAction.Value);
            }
            else if (actionKindString.StartsWith("S_"))
            {
                _playerActionSManager.SetAcquireAction(acquiredAction.Key, acquiredAction.Value);
            }
            else if (actionKindString.StartsWith("E_"))
            {
                // SetAcquiredAction()
            }
            else if (actionKindString.StartsWith("W_"))
            {
                // SetAcquiredAction()
            }
            else if (actionKindString.StartsWith("N_"))
            {
                // SetAcquiredAction()
            }
        }
    }

    public void AcquireAction(ActionKind actionKind)
    {
        string actionKindString = actionKind.ToString();

        if (actionKindString.StartsWith("Neutral_"))
        {
            _playerActionNeutralManager.SetAcquireAction(actionKind, true);
        }
        else if (actionKindString.StartsWith("S_"))
        {
            _playerActionSManager.SetAcquireAction(actionKind, true);
        }
        else if (actionKindString.StartsWith("E_"))
        {
            // SetAcquiredAction()
        }
        else if (actionKindString.StartsWith("W_"))
        {
            // SetAcquiredAction()
        }
        else if (actionKindString.StartsWith("N_"))
        {
            // SetAcquiredAction()
        }
    }

    public void ForgetAction(ActionKind actionKind)
    {
        string actionKindString = actionKind.ToString();

        if (actionKindString.StartsWith("Neutral_"))
        {
            _playerActionNeutralManager.SetAcquireAction(actionKind, false);
        }
        else if (actionKindString.StartsWith("S_"))
        {
            _playerActionSManager.SetAcquireAction(actionKind, false);
        }
        else if (actionKindString.StartsWith("E_"))
        {
            // SetAcquiredAction()
        }
        else if (actionKindString.StartsWith("W_"))
        {
            // SetAcquiredAction()
        }
        else if (actionKindString.StartsWith("N_"))
        {
            // SetAcquiredAction()
        }
    }

    public void RecoveryActionTime()
    {
        _playerActionSManager.RecoveryJumpTime();
        // 他のアクションも回復する
    }

    public void DepleteActionTime()
    {
        _playerActionSManager.DepleteJumpTime();
        // 他のアクションも消費する
    }
}
