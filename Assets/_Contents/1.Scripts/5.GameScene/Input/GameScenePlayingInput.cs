using UnityEngine;

public class GameScenePlayingInput : MonoBehaviour
{
    [HideInInspector] public Vector2 LeftDirection;
    [HideInInspector] public Vector2 NormalizedLeftDirection;
    [HideInInspector] public bool IsPushingS;
    [HideInInspector] public bool IsPushingE;
    [HideInInspector] public bool IsPushingW;
    [HideInInspector] public bool IsPushingN;
    [HideInInspector] public bool IsPushingL1;
    [HideInInspector] public bool IsPushingR1;
    [HideInInspector] public bool IsPushingL2;
    [HideInInspector] public bool IsPushingR2;

    private bool _wasPushingOption;
    private bool _isLockS;
    private bool _isLockE;

    // ----- Life Cycle Methods -----

    // ----- Public Methods -----

    public void InputInitialize()
    {
        LeftDirection = Vector2.zero;
        NormalizedLeftDirection = Vector2.zero;
        IsPushingS = false;
        IsPushingE = false;
        IsPushingW = false;
        IsPushingN = false;
        IsPushingL1 = false;
        IsPushingR1 = false;
        IsPushingL2 = false;
        IsPushingR2 = false;

        _isLockS = S_InputSystemManager.Instance.IsPushingS;
        _isLockE = S_InputSystemManager.Instance.IsPushingE;

        _wasPushingOption = true;
    }

    public void InputUpdate()
    {
        UpdateInputValue();

        if (S_InputSystemManager.Instance.IsPushingOption && !_wasPushingOption)
        {
            _wasPushingOption = true;
            OnPushingOption();
        }
        else if (!S_InputSystemManager.Instance.IsPushingOption && _wasPushingOption)
        {
            _wasPushingOption = false;
        }
    }

    // ----- Private Methods -----

    private void UpdateInputValue()
    {
        LeftDirection = S_InputSystemManager.Instance.LeftDirection;
        NormalizedLeftDirection = S_InputSystemManager.Instance.NormalizedLeftDirection;

        if (_isLockS && !S_InputSystemManager.Instance.IsPushingS)
        {
            _isLockS = false;
        }
        else if (!_isLockS)
        {
            IsPushingS = S_InputSystemManager.Instance.IsPushingS;
        }

        if (_isLockE && !S_InputSystemManager.Instance.IsPushingE)
        {
            _isLockE = false;
        }
        else if (!_isLockE)
        {
            IsPushingE = S_InputSystemManager.Instance.IsPushingE;
        }

        IsPushingW = S_InputSystemManager.Instance.IsPushingW;
        IsPushingN = S_InputSystemManager.Instance.IsPushingN;
        IsPushingL1 = S_InputSystemManager.Instance.IsPushingL1;
        IsPushingR1 = S_InputSystemManager.Instance.IsPushingR1;
        IsPushingL2 = S_InputSystemManager.Instance.IsPushingL2;
        IsPushingR2 = S_InputSystemManager.Instance.IsPushingR2;
    }

    private void OnPushingOption()
    {

    }

}
