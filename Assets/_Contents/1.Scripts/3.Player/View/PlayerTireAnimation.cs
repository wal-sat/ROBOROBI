using UnityEngine;

public class PlayerTireAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _animationTime;

    private float _timer;
    private int _index;

    // ----- Public Methods -----

    public void ViewInitialize()
    {
        _timer = 0;
        _index = 0;
        _spriteRenderer.sprite = _sprites[_index];
    }

    public void ViewUpdate(bool isActivePlayer)
    {
        if (!isActivePlayer) return;

        _timer += Time.deltaTime;

        if (_timer >= _animationTime)
        {
            _timer = 0;
            _index ++;

            if (_index >= _sprites.Length) _index = 0;

            _spriteRenderer.sprite = _sprites[_index];
        }
    }
}
