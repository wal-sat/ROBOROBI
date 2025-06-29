using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerMovementOverhead : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRigidbody2D;
    [SerializeField] private Transform _overheadCheckerTransform;
    [SerializeField] private LayerMask _groundLayer;

    private const float CircleSize = 0.01f;
    private const float StandCirclePositionY = 0.45f;
    private const float CrouchCirclePositionY = 0.2f;
    private const float CoolTime = 0.1f;

    private bool _isCoolTime;

    // ------ Public Methods -----

    public void MovementUpdate(bool isOverheadLockable)
    {
        if (isOverheadLockable || _playerRigidbody2D.linearVelocityY <= 0f || _isCoolTime) return;

        if (Physics2D.OverlapCircle(_overheadCheckerTransform.position, CircleSize, _groundLayer) != null)
        {
            if (_playerRigidbody2D.linearVelocityY > 1f)
            {
                _playerRigidbody2D.linearVelocity = new Vector2(_playerRigidbody2D.linearVelocityX, (float)Math.Sqrt(_playerRigidbody2D.linearVelocityY));
            }

            _isCoolTime = true;
            CoolTimer(destroyCancellationToken).Forget();
        }
    }

    public void ChangeOverheadTransform(bool isCrouching)
    {
        if (isCrouching)
        {
            _overheadCheckerTransform.localPosition = new Vector3(0f, CrouchCirclePositionY, 0f);
        }
        else
        {
            _overheadCheckerTransform.localPosition = new Vector3(0f, StandCirclePositionY, 0f);
        }
    }

    // ----- Private Methods -----

    private async UniTaskVoid CoolTimer(CancellationToken cancellationToken)
    {
        await UniTask.WaitForSeconds(CoolTime, cancellationToken: cancellationToken);

        _isCoolTime = false;
    }
}
