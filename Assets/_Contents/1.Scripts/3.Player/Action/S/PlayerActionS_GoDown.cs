using UnityEngine;

public class PlayerActionS_GoDown : PlayerActionBase
{
    [SerializeField] OneWayFloorManager _oneWayFloorManager;

    // ----- Public Methods -----

    public override void InitAction()
    {
        base.InitAction();

        _oneWayFloorManager.SetColliderEnable(false);
    }

    public override void EndAction()
    {
        base.EndAction();

        _oneWayFloorManager.SetColliderEnable(true);
    }

    public override void InitializeAction()
    {
        base.InitializeAction();

        _oneWayFloorManager.SetColliderEnable(true);
    }
}
