using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private PlayerActionManager _playerActionManager;
    [SerializeField] private PlayerViewManager _playerViewManager;
    [SerializeField] private GameObject _player;

    private bool _isActivePlayer;
    private bool _isEnteringDoor;

    // ----- Life Cycle Methods -----

    private void Start()
    {
        _playerViewManager.ViewInitialize(true);
    }

    private void FixedUpdate()
    {
        _playerMovementManager.MovementUpdate();
        _playerActionManager.ActionUpdate();
        _playerViewManager.ViewUpdate();
    }

    // ----- Public Methods -----

    public void Initialize()
    {
        _player.SetActive(true);
    }

    public void Activate()
    {
        _isActivePlayer = true;
        S_SEManager.Instance.Play("p_activate");
    }

    public void Death()
    {
        _player.SetActive(false);
    }

    public void EnterDoor()
    {

    }
}
