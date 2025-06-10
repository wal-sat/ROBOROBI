using System.Net.Http.Headers;
using NaughtyAttributes;
using UnityEngine;

public enum PlayerViewState { None, Sleep, Stand, Crouch, Grab, Hold, HoldingCrouch }

public class PlayerViewManager : MonoBehaviour
{
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerTireAnimation _playerTireAnimation;
    [SerializeField] private PlayerZAnimation _playerZAnimation;

    private PlayerViewState _playerViewState;

    // ----- Public Methods -----

    public void ViewInitialize(bool isFacingRight)
    {
        _playerView.ViewInitialize();
        _playerTireAnimation.ViewInitialize();
        _playerZAnimation.ViewInitialize(isFacingRight);

        ChangePlayerViewState(PlayerViewState.Sleep);
    }

    public void ViewUpdate()
    {
        _playerView.ViewUpdate();
        _playerTireAnimation.ViewUpdate();
        _playerZAnimation.ViewUpdate();
    }

    public void ChangePlayerViewState(PlayerViewState playerViewState)
    {
        if (playerViewState == _playerViewState) return;

        _playerView.SetPlayerView(playerViewState);

        if (_playerViewState == PlayerViewState.Sleep)
        {
            _playerZAnimation.ViewEnd();
        }

        _playerViewState = playerViewState;
    }

    [SerializeField] PlayerViewState aaa;

    [Button]
    private void A()
    {
        ChangePlayerViewState(aaa);
    }
}
