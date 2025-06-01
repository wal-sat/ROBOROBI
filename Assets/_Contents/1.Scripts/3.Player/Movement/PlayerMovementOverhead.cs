using System;
using UnityEngine;

public class PlayerMovementOverhead : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRigidbody2D;
    [SerializeField] private Transform _overheadCheckerTransform;
    [SerializeField] private LayerMask _groundLayer;

    private const float CircleSize = 0.01f;
    private const float StandCirclePositionY = 0.45f;
    private const float CrouchCirclePositionY = 0.2f;

    private Vector3 _overheadCirclePosition;

    // ------ Public Methods -----

    public void OverheadUpdate()
    {
        if (Physics2D.OverlapCircle(_overheadCheckerTransform.position, CircleSize, _groundLayer) != null && _playerRigidbody2D.linearVelocityY > 0f)
        {
            _playerRigidbody2D.linearVelocity = new Vector2(_playerRigidbody2D.linearVelocity.x, (float)Math.Sqrt(_playerRigidbody2D.linearVelocityY));
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
    
    public void Initialize()
    {
        ChangeOverheadTransform(false);
    }
}
