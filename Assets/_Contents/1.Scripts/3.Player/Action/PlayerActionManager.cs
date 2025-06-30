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
    [SerializeField] private PlayerActionNeutralManager _playerActionNeutralManager;
    [SerializeField] private PlayerActionSManager _playerActionSManager;
    [SerializeField] private PlayerMovementManager _playerMovementManager;

    private Vector2 _leftDirection;
    private Vector2 _normalizedLeftDirection;
    private bool _isPushingS;
    private bool _isPushingE;
    private bool _isPushingW;
    private bool _isPushingN;

    // ----- Public Methods -----

    public void ActionInitialize()
    {

    }

    public void ActionUpdate()
    {
        _leftDirection = S_InputSystemManager.Instance.LeftDirection;
        _normalizedLeftDirection = S_InputSystemManager.Instance.NormalizedLeftDirection;

        // Recovery Action Time
        if (_playerMovementManager.IsLanding)
        {
            RecoveryActionTime();
        }

        // Neutral
        _playerActionNeutralManager.ActionUpdate(_normalizedLeftDirection);

        // S Button
        if (S_InputSystemManager.Instance.IsPushingS)
        {
            _isPushingS = true;
            _playerActionSManager.ActionUpdate(_normalizedLeftDirection);
        }
        else if (_isPushingS)
        {
            _isPushingS = false;
            _playerActionSManager.ActionEnd();
        }
    }

    public void SetAcquiredAction(AcquiredActionData acquiredActionDate)
    {
        foreach (var acquiredAction in acquiredActionDate.AcquiredActionDictionary)
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

    public void ForgottenAction(ActionKind actionKind)
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

    // ----- Private Methods -----

    [SerializeField] AcquiredActionData acquiredActionData;

    [Button]
    private void Start()
    {
        SetAcquiredAction(acquiredActionData);
    }
}
