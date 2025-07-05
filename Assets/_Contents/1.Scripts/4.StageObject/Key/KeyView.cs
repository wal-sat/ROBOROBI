using UnityEngine;

public class KeyView : MonoBehaviour
{
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _disableSprite;

    private SpriteRenderer _spriteRenderer;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = _defaultSprite;
    }

    // ----- Public Methods -----

    public void ChangeSprite(bool isEnable)
    {
        if (isEnable)
        {
            _spriteRenderer.sprite = _defaultSprite;
        }
        else
        {
            _spriteRenderer.sprite = _disableSprite;
        }
    }
}
