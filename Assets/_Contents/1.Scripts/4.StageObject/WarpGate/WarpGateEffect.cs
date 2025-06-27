using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WarpGateEffect : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _animateDelay;

    private SpriteRenderer _spriteRenderer;
    private int _currentSpriteIndex = 0;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = _sprites[_currentSpriteIndex];
        AnimateLoop(destroyCancellationToken).Forget();
    }

    // ----- Private Methods -----

    private async UniTaskVoid AnimateLoop(CancellationToken cancellationToken)
    {
        while (true)
        {
            await UniTask.WaitForSeconds(_animateDelay, cancellationToken: cancellationToken);

            _currentSpriteIndex = (_currentSpriteIndex + 1) % _sprites.Length;
            _spriteRenderer.sprite = _sprites[_currentSpriteIndex];
        }
    }
}
