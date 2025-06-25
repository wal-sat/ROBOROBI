using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] private MovableObjectManager _movableObjectManager;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private MovableObjectDirection _initialDirection;

    [HideInInspector] public MovableObjectDirection CurrentDirection { get; private set; }
    [HideInInspector] public bool IsChangedDirection { get; set; }

    private bool _isGeneratedObject;
    private Vector3 _initialPosition;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _isGeneratedObject = GetComponent<GeneratedObject>() != null;
        if (_isGeneratedObject) return;

        _movableObjectManager.Register(this);

        _initialPosition = transform.position;
        CurrentDirection = _initialDirection;
    }

    private void Update()
    {
        switch (CurrentDirection)
        {
            case MovableObjectDirection.Up:
                _rigidbody2D.linearVelocity = Vector2.up * _moveSpeed * Time.fixedDeltaTime;
                break;
            case MovableObjectDirection.Down:
                _rigidbody2D.linearVelocity = Vector2.down * _moveSpeed * Time.fixedDeltaTime;
                break;
            case MovableObjectDirection.Left:
                _rigidbody2D.linearVelocity = Vector2.left * _moveSpeed * Time.fixedDeltaTime;
                break;
            case MovableObjectDirection.Right:
                _rigidbody2D.linearVelocity = Vector2.right * _moveSpeed * Time.fixedDeltaTime;
                break;
            case MovableObjectDirection.UpLeft:
                _rigidbody2D.linearVelocity = (Vector2.up + Vector2.left).normalized * _moveSpeed * Time.fixedDeltaTime;
                break;
            case MovableObjectDirection.UpRight:
                _rigidbody2D.linearVelocity = (Vector2.up + Vector2.right).normalized * _moveSpeed * Time.fixedDeltaTime;
                break;
            case MovableObjectDirection.DownLeft:
                _rigidbody2D.linearVelocity = (Vector2.down + Vector2.left).normalized * _moveSpeed * Time.fixedDeltaTime;
                break;
            case MovableObjectDirection.DownRight:
                _rigidbody2D.linearVelocity = (Vector2.down + Vector2.right).normalized * _moveSpeed * Time.fixedDeltaTime;
                break;
        }
    }

    // ----- Public Methods -----

    public void MovableObjectInitialize()
    {
        CurrentDirection = _initialDirection;
        transform.position = _initialPosition;
    }
    
    public void SetDirection(MovableObjectDirection direction)
    {
        CurrentDirection = direction;
    }   

    // ----- Private Methods -----
}
