using UnityEngine;

public class RigidbodyObject : MonoBehaviour
{
    [SerializeField] private float _terminalVelocity;

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

    private void Update()
    {
        if (Rigidbody.linearVelocityY < -_terminalVelocity)
        {
            Rigidbody.linearVelocity = new Vector2(Rigidbody.linearVelocityX, -_terminalVelocity);
        }
    }

    // ----- Public Methods -----

    public float GetBoundsBottomY()
    {
        return Collider.bounds.min.y;
    }

    public float GetRadius()
    {
        return Collider.bounds.extents.x;
    }
}
