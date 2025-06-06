using UnityEngine;

public class PlayerActionNeutralManager : MonoBehaviour
{
    [SerializeField] private PlayerActionBase _swapAction;

    private bool _isPushingL1;
    private bool _isPushingR1;

    // ----- Public Methods -----

    public void ActionUpdte(Vector2 leftDirection)
    {
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
    }

    // ----- Private Methods -----

    private void CallInitAction(PlayerActionBase action)
    {
        if (action == null) return;

        action.InitAction();
    }
    private void CallInAction(PlayerActionBase action)
    {
        if (action == null) return;

        action.InAction();
    }
    private void CallEndAction(PlayerActionBase action)
    {
        if (action == null) return;

        action.EndAction();
    }
}
