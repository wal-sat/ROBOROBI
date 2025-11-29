using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private OnTriggerWithRigidbodyObject _onTriggerWithRigidbodyObject;
    [SerializeField] private float _positionZ;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _onTriggerWithRigidbodyObject.TriggerEnterCallback = TriggerEnter;
        _onTriggerWithRigidbodyObject.TriggerExitCallback = TriggerExit;
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, _positionZ);
    }

    // ----- Private Methods -----

    private void TriggerEnter(RigidbodyObject rigidbodyObject = null)
    {
        IOverlapRope overlapRope = rigidbodyObject.GetComponent<IOverlapRope>();
        if (overlapRope != null)
        {
            overlapRope.Register(this);
        }
    }

    private void TriggerExit(RigidbodyObject rigidbodyObject = null)
    {
        IOverlapRope overlapRope = rigidbodyObject.GetComponent<IOverlapRope>();
        if (overlapRope != null)
        {
            overlapRope.Unregister(this);
        }
    }
}
