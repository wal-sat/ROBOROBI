using UnityEngine;

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

    public void RecoveryActionTime()
    {
        _playerActionSManager.RecoveryJumpTime();
    }

    public void DepleteActionTime()
    {
        _playerActionSManager.DepleteJumpTime();
    }

    // ----- Private Methods -----
}
