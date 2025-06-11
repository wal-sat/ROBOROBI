using UnityEngine;

public class BreakableBlockView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _defaultSprite;

    // ----- Public Methods -----

    public void ChangeView(bool isEnable)
    {
        if (isEnable)
        {
            _spriteRenderer.sprite = _defaultSprite;
        }
        else
        {
            _spriteRenderer.sprite = null;
        }
    }
}
