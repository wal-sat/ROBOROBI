using UnityEngine;

public class PlayerZAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _animationTime;

    private const float PositionX = 0.5f;
    private const float ScaleX = 1;

    private float _timer;
    private int _index;

    // ----- Public Methods -----

    public void ViewInitialize(bool isFacingRight)
    {
        _timer = 0;
        _index = 0;
        _spriteRenderer.sprite = _sprites[_index];

        if (isFacingRight)
        {
            this.transform.localPosition = new Vector3(PositionX, this.transform.localPosition.y, this.transform.localPosition.z);
            this.transform.localScale = new Vector3(ScaleX, this.transform.localScale.y, this.transform.localScale.z);
        }
        else
        {
            this.transform.localPosition = new Vector3(-PositionX, this.transform.localPosition.y, this.transform.localPosition.z);
            this.transform.localScale = new Vector3(-ScaleX, this.transform.localScale.y, this.transform.localScale.z);
        }

        this.gameObject.SetActive(true);
    }

    public void ViewEnd()
    {
        this.gameObject.SetActive(false);
    }

    public void ViewUpdate()
    {
        if (!this.gameObject.activeSelf) return;

        _timer += Time.deltaTime;

        if (_timer >= _animationTime)
        {
            _timer = 0;
            _index++;

            if (_index >= _sprites.Length) _index = 0;

            _spriteRenderer.sprite = _sprites[_index];
        }
    }
}
