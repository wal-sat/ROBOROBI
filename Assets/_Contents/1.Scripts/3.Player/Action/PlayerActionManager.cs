using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    [SerializeField] private PlayerActionNeutralManager _playerActionNeutralManager;
    [SerializeField] private PlayerActionSManager _playerActionSManager;
    [SerializeField] private PlayerMovementLanding _playerMovementLanding;

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

        // Recure Action Time
        if (_playerMovementLanding.IsLanding())
        {
            RecureActionTime();
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

    // ----- Private Methods -----

    private void RecureActionTime()
    {
        _playerActionSManager.RecureJumpTime();
    }
}
