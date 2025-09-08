using UnityEngine;

public class JumpRamp : MonoBehaviour
{
    [SerializeField] private OnCollisionWithRigidbodyObject _onCollisionWithRigidbodyObject;
    [SerializeField] private float _JumpPower;

    private const float OffsetY = 0.2f;
    private BoolDictionary<RigidbodyObject> _rigidbodyDictionary = new BoolDictionary<RigidbodyObject>();

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _onCollisionWithRigidbodyObject.CollisionEnterCallback += CollisionEnter;
        _onCollisionWithRigidbodyObject.CollisionExitCallback += CollisionExit;
    }

    private void OnDestroy()
    {
        _onCollisionWithRigidbodyObject.CollisionEnterCallback -= CollisionEnter;
        _onCollisionWithRigidbodyObject.CollisionExitCallback -= CollisionExit;
    }

    // ----- Private Methods -----

    private void CollisionEnter(RigidbodyObject rigidbodyObject)
    {
        if (this.transform.position.y + this.gameObject.transform.localScale.y / 2 - OffsetY <= rigidbodyObject.GetBoundsBottomY())
        {
            if (_rigidbodyDictionary.CheckValue(rigidbodyObject))
            {
                rigidbodyObject.Rigidbody.linearVelocity = new Vector3(rigidbodyObject.Rigidbody.linearVelocityX, _JumpPower, 0);

                S_SEManager.Instance.Play("s_jumpRamp");
            }
        }
    }

    private void CollisionExit(RigidbodyObject rigidbodyObject)
    {
        _rigidbodyDictionary.ResetValue(rigidbodyObject);
    }
}
