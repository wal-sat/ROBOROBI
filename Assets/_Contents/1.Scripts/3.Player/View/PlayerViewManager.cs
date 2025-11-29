using System.Net.Http.Headers;
using NaughtyAttributes;
using UnityEngine;

public enum PlayerViewState { None, Sleep, Stand, Grab, Crouch, Accelerate, Decelerate, Hold, HoldingGrab, HoldingCrouch, HoldingAccelerate, HoldingDecelerate }

public class PlayerViewManager : MonoBehaviour
{
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerTireAnimation _playerTireAnimation;
    [SerializeField] private PlayerZAnimation _playerZAnimation;

    [HideInInspector] public bool IsSleeping { private get; set; }
    [HideInInspector] public bool IsGrabbing { private get; set; }
    [HideInInspector] public bool IsCrouching { private get; set; }
    [HideInInspector] public bool IsAccelerate { private get; set; }
    [HideInInspector] public bool IsDecelerate { private get; set; }
    [HideInInspector] public bool IsHolding { private get; set; }

    private PlayerViewState _playerViewState;

    // ----- Public Methods -----

    public void ViewInitialize(bool isFacingRight)
    {
        _playerView.ViewInitialize();
        _playerTireAnimation.ViewInitialize();
        _playerZAnimation.ViewInitialize(isFacingRight);

        IsSleeping = true;
        IsGrabbing = false;
        IsCrouching = false;
        IsAccelerate = false;
        IsDecelerate = false;
        IsHolding = false;

        ChangePlayerViewState();
    }

    public void ViewUpdate(bool isActivePlayer)
    {
        ChangePlayerViewState();

        _playerView.ViewUpdate(isActivePlayer);
        _playerTireAnimation.ViewUpdate(isActivePlayer);
        _playerZAnimation.ViewUpdate(isActivePlayer);
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
        return (IsSleeping, IsGrabbing, IsCrouching, IsAccelerate, IsDecelerate, IsHolding) switch
        {
            (true, _, _, _, _, _) => PlayerViewState.Sleep,
            (false, true, _, _, _, true) => PlayerViewState.HoldingGrab,
            (false, _, true, _, _, true) => PlayerViewState.HoldingCrouch,
            (false, _, _, true, _, true) => PlayerViewState.HoldingAccelerate,
            (false, _, _, _, true, true) => PlayerViewState.HoldingDecelerate,
            (false, true, _, _, _, false) => PlayerViewState.Grab,
            (false, _, true, _, _, false) => PlayerViewState.Crouch,
            (false, _, _, true, _, false) => PlayerViewState.Accelerate,
            (false, _, _, _, true, false) => PlayerViewState.Decelerate,
            _ => PlayerViewState.Stand
        };
    }
}
