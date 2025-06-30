using UnityEngine;

public class PlayerActionBase : MonoBehaviour
{
    [SerializeField] public ActionKind ActionKind;

    [HideInInspector] public bool IsAcquired = true;
    [HideInInspector] public bool IsInAction;
    [HideInInspector] public bool IsLockOtherAction;

    public virtual void InitAction()
    {
        IsInAction= true;
    }
    public virtual void InAction()
    {
        ;
    }
    public virtual void EndAction()
    {
        IsInAction = false;
    }
    public virtual void InitializeAction()
    {
        IsInAction = false;
    }
    public virtual void SwapInAction()
    {
        ;
    }
}
