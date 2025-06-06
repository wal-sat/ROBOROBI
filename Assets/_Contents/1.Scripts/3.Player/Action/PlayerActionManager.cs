using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    [SerializeField] private PlayerActionNeutralManager _playerActionNeutralManager;
    [SerializeField] private PlayerActionSManager _playerActionSManager;

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

        // Neutral
        _playerActionNeutralManager.ActionUpdte(_normalizedLeftDirection);

        // S Button
        if (S_InputSystemManager.Instance.IsPushingS)
        {
            _isPushingS = true;
            _playerActionSManager.ActionUpdte(_normalizedLeftDirection);
        }
        else if (_isPushingS)
        {
            _isPushingS = false;
            _playerActionSManager.ActionEnd();
        }
    }
}
