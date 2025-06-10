using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private PlayerActionManager _playerActionManager;
    [SerializeField] private PlayerViewManager _playerViewManager;

    // ----- Life Cycle Methods -----

    private void Start()
    {
        _playerViewManager.ViewInitialize(true);
    }

    private void Update()
    {
        _playerMovementManager.MovementUpdate();
        _playerActionManager.ActionUpdate();
        _playerViewManager.ViewUpdate();
    }
}
