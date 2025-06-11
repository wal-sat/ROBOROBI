using System;
using UnityEngine;

public class PlayerMovementRunning : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRigidbody2D;
    [SerializeField] private float _defaultRunSpeed;

    public Func<float> SpeedAdjustCallback;

    private float _runSpeed;

    // ----- Public Methods -----

    public void MovementUpdate(bool isFacingRight, bool isPlayerRunningLock)
    {
        if (isPlayerRunningLock) return;

        _runSpeed = isFacingRight ? _defaultRunSpeed : -_defaultRunSpeed;

        if (SpeedAdjustCallback != null)
        {
            _runSpeed += SpeedAdjustCallback.Invoke();
        }

        _playerRigidbody2D.linearVelocity = new Vector2(_runSpeed * Time.fixedDeltaTime, _playerRigidbody2D.linearVelocityY);
    }
}

