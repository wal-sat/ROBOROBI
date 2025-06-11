using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _sleepSprites;
    [SerializeField] private Sprite _standSprite;
    [SerializeField] private Sprite _crouchSprite;
    [SerializeField] private Sprite _grabSprite;
    [SerializeField] private Sprite _holdSprite;
    [SerializeField] private Sprite _holdingCrouchSprite;
    [SerializeField] private float _sleepAnimationTime;

    private bool _isSleeping;
    private float _timer;
    private int _index;

    // ----- Public Methods -----

    public void ViewInitialize()
    {
        _timer = 0;
        _index = 0;
        _spriteRenderer.sprite = _sleepSprites[_index];
    }

    public void ViewUpdate()
    {
        if (!_isSleeping) return;

        _timer += Time.deltaTime;

        if (_timer >= _sleepAnimationTime)
        {
            _timer = 0;
            _index++;

            if (_index >= _sleepSprites.Length) _index = 0;

            _spriteRenderer.sprite = _sleepSprites[_index];
        }
    }

    public void SetPlayerView(PlayerViewState playerViewState)
    {
        _isSleeping = false;

        switch (playerViewState)
        {
            case PlayerViewState.Sleep:
                _isSleeping = true;
                break;
            case PlayerViewState.Stand:
                _spriteRenderer.sprite = _standSprite;
                break;
            case PlayerViewState.Crouch:
                _spriteRenderer.sprite = _crouchSprite;
                break;
            case PlayerViewState.Grab:
                _spriteRenderer.sprite = _grabSprite;
                break;
            case PlayerViewState.Hold:
                _spriteRenderer.sprite = _holdSprite;
                break;
            case PlayerViewState.HoldingCrouch:
                _spriteRenderer.sprite = _holdingCrouchSprite;
                break;
        }
    }
}
