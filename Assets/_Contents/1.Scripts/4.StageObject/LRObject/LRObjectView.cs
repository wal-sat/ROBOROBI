using UnityEngine;

public class LRObjectView : MonoBehaviour
{
    [SerializeField] private Sprite _lSprite;
    [SerializeField] private Sprite _rSprite;

    private SpriteRenderer _spriteRenderer;

    // ----- Life Cycle Methods -----
    
    private void Awake()
    {
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = _lSprite;
    }

    // ----- Public Methods -----

    public void SpriteChange(LRObjectState lrObjectState)
    {
        if (lrObjectState == LRObjectState.L) _spriteRenderer.sprite = _lSprite;
        else if (lrObjectState == LRObjectState.R) _spriteRenderer.sprite = _rSprite;
    }
}
