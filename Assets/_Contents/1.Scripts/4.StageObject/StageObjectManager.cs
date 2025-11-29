using UnityEngine;

public class StageObjectManager : MonoBehaviour
{
    [SerializeField] private ActionCassetteManager _actionCassetteManager;
    [SerializeField] private BarrelManager _barrelManager;
    [SerializeField] private ButtonManager _buttonManager;
    [SerializeField] private ConveyorManager _conveyorManager;
    [SerializeField] private GeneratorManager _generatorManager;
    [SerializeField] private LRObjectManager _lrObjectManager;
    [SerializeField] private MovableObjectManager _movableObjectManager;
    [SerializeField] private OneWayFloorManager _oneWayFloorManager;
    [SerializeField] private RecoveryCapsuleManager _recoveryCapsuleManager;
    [SerializeField] private WarpGateManager _warpGateManager;

    // ----- Public Methods -----

    public void StageObjectInitialize()
    {
        _actionCassetteManager.StageObjectInitialize();
        _barrelManager.StageObjectInitialize();
        _buttonManager.StageObjectInitialize();
        _conveyorManager.StageObjectInitialize();
        _generatorManager.StageObjectInitialize();
        _lrObjectManager.StageObjectInitialize();
        _movableObjectManager.StageObjectInitialize();
        _recoveryCapsuleManager.StageObjectInitialize();
        _warpGateManager.StageObjectInitialize();
    }
}
