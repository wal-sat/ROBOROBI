using Cysharp.Threading.Tasks;
using UnityEngine;

public class SaveGateView : MonoBehaviour
{
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _glossSprite;
    [SerializeField] private float _glossTime;

    private SpriteRenderer _spriteRenderer;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = _defaultSprite;
    }

    // ----- Public Methods -----

    public async UniTaskVoid GlossSprite()
    {
        _spriteRenderer.sprite = _glossSprite;

        await UniTask.WaitForSeconds(_glossTime);

        _spriteRenderer.sprite = _defaultSprite;
    }
}
