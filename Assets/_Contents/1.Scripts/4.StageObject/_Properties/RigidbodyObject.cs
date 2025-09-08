using UnityEngine;

public class RigidbodyObject : MonoBehaviour
{
    public Rigidbody2D Rigidbody { get; private set; }
    public Collider2D Collider { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // ----- Public Methods -----

    public float GetBoundsBottomY()
    {
        return Collider.bounds.min.y;
    }
}
