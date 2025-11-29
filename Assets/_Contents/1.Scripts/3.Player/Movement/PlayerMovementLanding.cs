using NUnit.Framework;
using UnityEngine;

public class PlayerMovementLanding : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Transform _landingCheckerTransform;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _throughGroundLayer;

    private const float CapsulePositionY = -0.5f;
    private const float CapsuleSizeX = 0.2f;
    private const float CapsuleSizeY = 0.005f;
    private const float LandingSEBufferTime = 0.2f;
    private const float VelocityYThreshold = 0.005f;

    private Vector3 _capsuleSize;
    private float _landingSEBufferTimer;
    private bool _isLandingLockable;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _landingCheckerTransform.localPosition = new Vector3(0f, CapsulePositionY, 0f);
        _capsuleSize = new Vector3(CapsuleSizeX, CapsuleSizeY, 0f);
    }

    // ----- Public Methods -----

    public void MovementUpdate(bool isLandingLockable)
    {
        _isLandingLockable = isLandingLockable;
        if (_isLandingLockable) return;

        if (IsLanding())
        {
            if (_landingSEBufferTimer > LandingSEBufferTime)
            {
                S_SEManager.Instance.Play("p_landing");
            }
            _landingSEBufferTimer = 0f;
        }
        else
        {
            _landingSEBufferTimer += Time.deltaTime;
        }
    }

    public bool IsLanding()
    {
        if (_isLandingLockable)
        {
            return false;
        }

        if ((IsLandGround() || IsLandThroughGround()) && _rigidbody2D.linearVelocityY <= VelocityYThreshold)
        {
            _rigidbody2D.linearVelocityY = 0;
            return true;
        }

        return false;
    }

    // ----- Private Methods -----

    private bool IsLandGround()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCapsuleAll(_landingCheckerTransform.position, _capsuleSize, CapsuleDirection2D.Horizontal, 0, _groundLayer);
        if (hitColliders.Length > 0)
        {
            return true;
        }
        return false;
    }

    private bool IsLandThroughGround()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCapsuleAll(_landingCheckerTransform.position, _capsuleSize, CapsuleDirection2D.Horizontal, 0, _throughGroundLayer);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponentInParent<OneWayFloor>()?.IsEnableLandingCheck ?? false)
            {
                return true;
            }
        }
        return false;
    }
}
