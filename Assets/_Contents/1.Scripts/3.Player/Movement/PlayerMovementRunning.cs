using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementRunning : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRigidbody2D;
    [SerializeField] private float _defaultRunSpeed;

    public List< Func<float> > SpeedAdjustCallbackList = new List< Func<float> >();

    private float _runSpeed;

    // ----- Public Methods -----

    public void MovementUpdate(bool isFacingRight, bool isPlayerRunningLock)
    {
        if (isPlayerRunningLock) return;

        _runSpeed = isFacingRight ? _defaultRunSpeed : -_defaultRunSpeed;

        foreach (var speedAdjustCallback in SpeedAdjustCallbackList)
        {
            _runSpeed += speedAdjustCallback();
        }

        _playerRigidbody2D.linearVelocity = new Vector2(_runSpeed, _playerRigidbody2D.linearVelocityY);
    }
}

