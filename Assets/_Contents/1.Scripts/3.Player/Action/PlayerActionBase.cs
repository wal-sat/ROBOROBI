using UnityEngine;

public class PlayerActionBase : MonoBehaviour
{
    [HideInInspector] public bool IsAcquired = true;
    [HideInInspector] public bool IsInAction;
    [HideInInspector] public bool IsLockOhterAction;

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
    public virtual void Initialize()
    {
        IsInAction = false;
    }
    public virtual void SwapInAction()
    {
        ;
    }
}
