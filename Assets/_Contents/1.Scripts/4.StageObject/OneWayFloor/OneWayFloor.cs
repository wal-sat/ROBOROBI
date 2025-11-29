using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

public class OneWayFloor : MonoBehaviour
{
    [SerializeField] private OneWayFloorManager _oneWayFloorManager;
    [SerializeField] private Collider2D _playerCollider2D;
    [SerializeField] private Collider2D _selfCollider2D;

    private const float BoundsSizeOffset = 2f;
    private const float ThresholdOffset = 0.25f;

    public bool IsEnableLandingCheck { get; private set; }

    private Vector2 _boundsCenter;
    private Vector2 _boundsSize;
    private bool _isPlayerGoDown;


    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _oneWayFloorManager.Register(this);

        _boundsCenter = _selfCollider2D.bounds.center;
        _boundsSize = new Vector2(_selfCollider2D.bounds.size.x + BoundsSizeOffset, _selfCollider2D.bounds.size.y + BoundsSizeOffset);
    }

    private void Update()
    {
        if (_isPlayerGoDown) return;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(_boundsCenter, _boundsSize, 0f);

        foreach (var hitCollider in hitColliders)
        {
            RigidbodyObject rigidbodyObject = hitCollider.GetComponent<RigidbodyObject>();
            if (rigidbodyObject != null)
            {
                if (_selfCollider2D.bounds.max.y - ThresholdOffset > rigidbodyObject.GetBoundsBottomY())
                {
                    Physics2D.IgnoreCollision(_selfCollider2D, rigidbodyObject.Collider, true);
                    if (rigidbodyObject.CompareTag("Player"))
                    {
                        IsEnableLandingCheck = false;
                    }
                }
                else
                {
                    Physics2D.IgnoreCollision(_selfCollider2D, rigidbodyObject.Collider, false);
                    if (rigidbodyObject.CompareTag("Player"))
                    {
                        IsEnableLandingCheck = true;
                    }
                }
            }
        }
    }

    // ----- Public Methods -----

    public void PlayerGoDown(bool isGoDown)
    {
        _isPlayerGoDown = isGoDown;
        if (_selfCollider2D.bounds.max.y - ThresholdOffset > _playerCollider2D.bounds.min.y) return;

        Physics2D.IgnoreCollision(_selfCollider2D, _playerCollider2D, _isPlayerGoDown);
        IsEnableLandingCheck = !_isPlayerGoDown;
    }
}
