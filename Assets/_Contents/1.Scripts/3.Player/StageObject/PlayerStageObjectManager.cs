using UnityEngine;

public class PlayerStageObjectManager : MonoBehaviour
{
    [SerializeField] private PlayerOverlapRope _playerOverlapRope;
    [SerializeField] private PlayerBreakBlock _playerBreakBlock;

    // ----- Public Methods -----

    public void StageObjectInitialize()
    {
        _playerOverlapRope.Initialize();
        _playerBreakBlock.Initialize();
    }
}
