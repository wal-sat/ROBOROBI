using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerMovementBackside : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRigidbody2D;
    [SerializeField] private Transform _backsideCheckerTransform;
    [SerializeField] private LayerMask _groundLayer;

    private const float CapsulePositionX = -0.2f;
    private const float StandCapsulePositionY = -0.05f;
    private const float CrouchCapsulePositionY = -0.175f;
    private const float CapsuleSizeX = 0.1f;
    private const float StandCapsuleSizeY = 0.35f;
    private const float CrouchCapsuleSizeY = 0.225f;
    private const float CoolTime = 0.1f;

    private bool _isCoolTime;

    private Vector3 _swapCapsuleSize;

    // ----- Public Methods -----

    public void MovementUpdate(bool isFacingRight, bool isBacksideLockable)
    {
        if (isBacksideLockable || _isCoolTime) return;
        if ((isFacingRight && _playerRigidbody2D.linearVelocityX >= 0f) || (!isFacingRight && _playerRigidbody2D.linearVelocityX <= 0f)) return;

        Vector3 bufferX = new Vector3(_playerRigidbody2D.linearVelocityX * Time.fixedDeltaTime, 0f, 0f);

        if (Physics2D.OverlapCapsule(_backsideCheckerTransform.position + bufferX, _swapCapsuleSize, CapsuleDirection2D.Vertical, 0, _groundLayer) != null)
        {
            float abstractVelocityX = Math.Abs(_playerRigidbody2D.linearVelocityX);
            if (abstractVelocityX > 1f)
            {
                float velocityX = (float)Math.Sqrt(abstractVelocityX) * Mathf.Sign(_playerRigidbody2D.linearVelocityX);
                _playerRigidbody2D.linearVelocity = new Vector2(velocityX, _playerRigidbody2D.linearVelocityY);
            }

            _isCoolTime = true;
            CoolTimer(destroyCancellationToken).Forget();
        }
    }

    public void ChangeBacksideTransform(bool isCrouching)
    {
        if (isCrouching)
        {
            _backsideCheckerTransform.localPosition = new Vector3(CapsulePositionX, CrouchCapsulePositionY, 0f);
            _swapCapsuleSize = new Vector3(CapsuleSizeX, CrouchCapsuleSizeY, 0f);
        }
        else
        {
            _backsideCheckerTransform.localPosition = new Vector3(CapsulePositionX, StandCapsulePositionY, 0f);
            _swapCapsuleSize = new Vector3(CapsuleSizeX, StandCapsuleSizeY, 0f);
        }
    }

    // ----- Private Methods -----

    private async UniTaskVoid CoolTimer(CancellationToken cancellationToken)
    {
        await UniTask.WaitForSeconds(CoolTime, cancellationToken: cancellationToken);

        _isCoolTime = false;
    }
}
