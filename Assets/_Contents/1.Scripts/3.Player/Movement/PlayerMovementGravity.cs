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

    public void MovementUpdate(bool isDisable)
    {
        if (isDisable)
        {
            _playerRigidbody2D.gravityScale = 0f;
            return;
        }

        _playerRigidbody2D.gravityScale = _defaultGravityScale;
    }
}
