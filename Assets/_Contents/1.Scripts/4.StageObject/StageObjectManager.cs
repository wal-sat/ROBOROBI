using UnityEngine;

public class StageObjectManager : MonoBehaviour
{
    [SerializeField] private ActionCassetteManager _actionCassetteManager;
    [SerializeField] private BarrelManager _barrelManager;
    [SerializeField] private BreakableBlockManager _breakableBlockManager;
    [SerializeField] private ButtonManager _buttonManager;
    [SerializeField] private ConveyorManager _conveyorManager;
    [SerializeField] private GearManager _gearManager;
    [SerializeField] private GeneratorManager _generatorManager;
    [SerializeField] private LRObjectManager _lrObjectManager;
    [SerializeField] private MovableObjectManager _movableObjectManager;
    [SerializeField] private OneWayFloorManager _oneWayFloorManager;
    [SerializeField] private RecoveryCapsuleManager _recoveryCapsuleManager;
    [SerializeField] private RopeManager _ropeManager;
    [SerializeField] private WarpGateManager _warpGateManager;

    // ----- Public Methods -----

    public void StageObjectInitialize()
    {
        _actionCassetteManager.StageObjectInitialize();
        _barrelManager.StageObjectInitialize();
        _breakableBlockManager.StageObjectInitialize();
        _buttonManager.StageObjectInitialize();
        _conveyorManager.StageObjectInitialize();
        _gearManager.StageObjectInitialize();
        _generatorManager.StageObjectInitialize();
        _lrObjectManager.StageObjectInitialize();
        _movableObjectManager.StageObjectInitialize();
        _recoveryCapsuleManager.StageObjectInitialize();
        _warpGateManager.StageObjectInitialize();
    }
}
