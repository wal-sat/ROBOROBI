using UnityEngine;

public class PlayerMovementTerminalVelocity : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRigidbody2D;
    [SerializeField] private float _terminalVelocity;

    // ----- Public Methods -----

    public void TerminalVelocityUpdate()
    {
        if (_playerRigidbody2D.linearVelocity.y < -_terminalVelocity * Time.fixedDeltaTime)
        {
            _playerRigidbody2D.linearVelocity = new Vector2(_playerRigidbody2D.linearVelocity.x, -_terminalVelocity * Time.fixedDeltaTime);
        }
    }
}
