using UnityEngine;

public class PlayerMovementGravity : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRigidbody2D;
    [SerializeField] private float _defaultGravityScale;

    // ----- Public Methods -----

    public void MovementInitialize()
    {
        _playerRigidbody2D.gravityScale = _defaultGravityScale;
    }

    public void MovementUpdate(bool isGravityLockable)
    {
        if (isGravityLockable)
        {
            _playerRigidbody2D.gravityScale = 0f;
        }
        else
        {
            _playerRigidbody2D.gravityScale = _defaultGravityScale;
        }
    }
}
