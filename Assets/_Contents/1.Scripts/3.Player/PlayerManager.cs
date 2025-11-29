using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private PlayerActionManager _playerActionManager;
    [SerializeField] private PlayerViewManager _playerViewManager;
    [SerializeField] private PlayerStageObjectManager _playerStageObjectManager;
    [SerializeField] private PlayerScrapManager _playerScrapManager;
    [SerializeField] private PlayerExplosionAnimation _playerExplosionAnimation;
    [SerializeField] private PlayerCameraOut _playerCameraOut;
    [SerializeField] private GameObject _player;

    private bool _isActivePlayer;
    private bool _isClearSection;

    // ----- Life Cycle Methods -----

    private void Start()
    {
        _playerViewManager.ViewInitialize(true);
    }

    private void FixedUpdate()
    {
        if (_isClearSection) return;

        _playerMovementManager.MovementUpdate(_isActivePlayer);
        _playerActionManager.ActionUpdate(_isActivePlayer);
        _playerViewManager.ViewUpdate(_isActivePlayer);
        _playerCameraOut.CameraUpdate(_isActivePlayer);
    }

    // ----- Public Methods -----

    public void Initialize(bool isFacingRight)
    {
        _player.SetActive(true);

        _isActivePlayer = false;
        _isClearSection = false;

        _playerMovementManager.MovementInitialize(isFacingRight);
        _playerActionManager.ActionInitialize();
        _playerViewManager.ViewInitialize(isFacingRight);
        _playerStageObjectManager.StageObjectInitialize();
        _playerScrapManager.ScrapInitialize();
    }

    public void Activate(AcquiredActionData acquiredActionData)
    {
        _isActivePlayer = true;
        _playerActionManager.SetAcquiredAction(acquiredActionData);
        _playerViewManager.IsSleeping = false;

        S_SEManager.Instance.Play("p_activate");
    }

    public void Death(float angleZ)
    {
        _player.SetActive(false);

        _playerScrapManager.DeathExplosion(_player.transform.position, angleZ);
        _playerExplosionAnimation.DeathExplosion(_player.transform.position).Forget();

        S_SEManager.Instance.Play("p_explosion");
    }

    public void SectionClear()
    {
        _isClearSection = true;

        _playerActionManager.ActionInitialize();
        _playerMovementManager.SetPlayerVelocityZero();
    }

    public void DeleteScrap()
    {
        _playerScrapManager.DestroyAllScraps();
    }
}
