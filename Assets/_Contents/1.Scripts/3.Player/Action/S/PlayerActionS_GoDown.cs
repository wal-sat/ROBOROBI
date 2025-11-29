using UnityEngine;

public class PlayerActionS_GoDown : PlayerActionBase
{
    [SerializeField] OneWayFloorManager _oneWayFloorManager;

    // ----- Public Methods -----

    public override void InitAction()
    {
        base.InitAction();

        _oneWayFloorManager.PlayerGoDown(true);
    }

    public override void EndAction()
    {
        base.EndAction();

        _oneWayFloorManager.PlayerGoDown(false);
    }

    public override void InitializeAction()
    {
        base.InitializeAction();

        _oneWayFloorManager.PlayerGoDown(false);
    }
}
