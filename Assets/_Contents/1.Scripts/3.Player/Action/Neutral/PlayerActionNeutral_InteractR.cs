using UnityEngine;

public class PlayerActionNeutral_InteractR : PlayerActionBase
{
    [SerializeField] private LRObjectManager _lrObjectManager;

    // ----- Public Methods -----

    public override void InitAction()
    {
        base.InitAction();

        _lrObjectManager.LRObjectMove(LRObjectState.R);
    }
}
