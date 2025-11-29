using UnityEngine;

public class JumpRamp : MonoBehaviour
{
    private enum JumpRampPower
    {
        Small = 15,
        Medium = 20,
        Large = 25
    }

    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private OnCollisionWithRigidbodyObject _onCollisionWithRigidbodyObject;
    [SerializeField] private JumpRampPower _jumpRampPower;

    private const float OffsetY = 0.2f;
    private BoolDictionary<RigidbodyObject> _rigidbodyDictionary = new BoolDictionary<RigidbodyObject>();
    private float _boundMaxY;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _onCollisionWithRigidbodyObject.CollisionStayCallback += CollisionStay;
        _onCollisionWithRigidbodyObject.CollisionExitCallback += CollisionExit;

        _boundMaxY = _collider2D.bounds.max.y;
    }

    private void OnDestroy()
    {
        _onCollisionWithRigidbodyObject.CollisionStayCallback -= CollisionStay;
        _onCollisionWithRigidbodyObject.CollisionExitCallback -= CollisionExit;
    }

    // ----- Private Methods -----

    private void CollisionStay(RigidbodyObject rigidbodyObject)
    {
        if (_boundMaxY - OffsetY <= rigidbodyObject.GetBoundsBottomY())
        {
            if (_rigidbodyDictionary.CheckValue(rigidbodyObject))
            {
                rigidbodyObject.Rigidbody.linearVelocity = new Vector3(rigidbodyObject.Rigidbody.linearVelocityX, (float)_jumpRampPower, 0);

                S_SEManager.Instance.Play("s_jumpRamp");
            }
        }
    }

    private void CollisionExit(RigidbodyObject rigidbodyObject)
    {
        _rigidbodyDictionary.ResetValue(rigidbodyObject);
        _rigidbodyDictionary.RemoveKey(rigidbodyObject);
    }
}
