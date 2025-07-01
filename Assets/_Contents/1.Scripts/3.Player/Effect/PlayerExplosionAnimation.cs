using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerExplosionAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _animationTime;

    private SpriteRenderer _spriteRenderer;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = null;
    }

    // ----- Public Methods -----

    public async UniTaskVoid DeathExplosion(Vector3 playerDeathPosition)
    {
        this.transform.position = new Vector3(playerDeathPosition.x, playerDeathPosition.y, this.transform.position.z);

        for (int i = 0; i < _sprites.Length; i++)
        {
            _spriteRenderer.sprite = _sprites[i];

            await UniTask.WaitForSeconds(_animationTime, cancellationToken: destroyCancellationToken);
        }

        _spriteRenderer.sprite = null;
    }
}
