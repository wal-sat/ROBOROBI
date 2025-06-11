using UnityEngine;

public class JumpRamp : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRigidbody2D;
    [SerializeField] private Transform _landingCheckerTransform;
    [SerializeField] private float _JumpPower;

    private const float OffsetY = 0.2f;
    private FirstCallChecker _firstCallChecker = new FirstCallChecker();
    
    // ----- Private Methods -----

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (this.transform.position.y + this.gameObject.transform.localScale.y / 2 - OffsetY <= _landingCheckerTransform.position.y)
            {
                if (_firstCallChecker.Check())
                {
                    _playerRigidbody2D.linearVelocity = new Vector3(_playerRigidbody2D.linearVelocityX, _JumpPower, 0);

                    S_SEManager.Instance.Play("s_jumpRamp");
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        _firstCallChecker.Reset();
    }
}
