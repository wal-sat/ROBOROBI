using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private PlayerActionManager _playerActionManager;

    // ----- Life Cycle Methods -----

    private void Update()
    {
        _playerMovementManager.MovementUpdate();
        _playerActionManager.ActionUpdate();
    }
}
