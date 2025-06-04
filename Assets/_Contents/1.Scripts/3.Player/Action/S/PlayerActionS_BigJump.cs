using UnityEngine;

public class PlayerActionS_BigJump : PlayerActionBase
{
    [SerializeField] private Rigidbody2D _playerRigidbody2D;
    [SerializeField] private float _jumpForce;

    public override void InitAction()
    {
        base.InitAction();

        _playerRigidbody2D.linearVelocity = new Vector2(_playerRigidbody2D.linearVelocityX, _jumpForce);
        
        S_SEManager.Instance.Play("p_jump");
    }
}
