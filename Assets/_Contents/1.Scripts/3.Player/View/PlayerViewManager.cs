using System.Net.Http.Headers;
using NaughtyAttributes;
using UnityEngine;

public enum PlayerViewState { None, Sleep, Stand, Crouch, Grab, Hold, HoldingCrouch }

public class PlayerViewManager : MonoBehaviour
{
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerTireAnimation _playerTireAnimation;
    [SerializeField] private PlayerZAnimation _playerZAnimation;

    [HideInInspector] public bool IsSleeping { private get; set; }
    [HideInInspector] public bool IsCrouching { private get; set; }
    [HideInInspector] public bool IsGrabbing { private get; set; }
    [HideInInspector] public bool IsHolding { private get; set; }

    private PlayerViewState _playerViewState;

    // ----- Public Methods -----

    public void ViewInitialize(bool isFacingRight)
    {
        _playerView.ViewInitialize();
        _playerTireAnimation.ViewInitialize();
        _playerZAnimation.ViewInitialize(isFacingRight);

        IsSleeping = true;
        IsCrouching = false;
        IsGrabbing = false;
        IsHolding = false;

        ChangePlayerViewState();
    }

    public void ViewUpdate()
    {
        ChangePlayerViewState();

        _playerView.ViewUpdate();
        _playerTireAnimation.ViewUpdate();
        _playerZAnimation.ViewUpdate();
    }

    // ----- Private Methods -----

    private void ChangePlayerViewState()
    {
        PlayerViewState playerViewState = CurrentPlayerViewState();

        if (playerViewState == _playerViewState) return;

        _playerView.SetPlayerView(playerViewState);
        if (_playerViewState == PlayerViewState.Sleep)
        {
            _playerZAnimation.ViewEnd();
        }

        _playerViewState = playerViewState;
    }

    private PlayerViewState CurrentPlayerViewState()
    {
        return (IsSleeping, IsCrouching, IsGrabbing, IsHolding) switch
        {
            (true, _, _, _) => PlayerViewState.Sleep,
            (false, true, _, true) => PlayerViewState.HoldingCrouch,
            (false, true, _, _) => PlayerViewState.Crouch,
            (false, _, true, _) => PlayerViewState.Grab,
            (false, _, _, true) => PlayerViewState.Hold,
            _ => PlayerViewState.Stand
        };
    }

    [Button]
    private void Activate()
    {
        IsSleeping = false;
    }
}
