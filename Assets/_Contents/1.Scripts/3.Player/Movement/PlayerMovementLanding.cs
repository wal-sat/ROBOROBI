using System;
using UnityEngine;

public class PlayerMovementLanding : MonoBehaviour
{
    [SerializeField] private Transform _landingCheckerTransform;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _throughGroundLayer;

    // public Action OnLandingCallback;

    private const float CapsulePositionY = -0.5f;
    private const float CapsuleSizeX = 0.2f;
    private const float CapsuleSizeY = 0.01f;
    private const float LandingSEBufferTime = 0.2f;

    private Vector3 _capsuleSize;
    private float _landingSEBufferTimer;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _landingCheckerTransform.localPosition = new Vector3(0f, CapsulePositionY, 0f);
        _capsuleSize = new Vector3(CapsuleSizeX, CapsuleSizeY, 0f);
    }

    // ----- Public Methods -----

    public void LandingUpdate()
    {
        if (IsLanding())
        {
            //OnLandingCallback?.Invoke();

            if (_landingSEBufferTimer > LandingSEBufferTime)
            {
                S_SEManager.Instance.Play("p_land");
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
        if (Physics2D.OverlapCapsule(_landingCheckerTransform.position, _capsuleSize, CapsuleDirection2D.Horizontal, 0, _groundLayer) != null ||
            Physics2D.OverlapCapsule(_landingCheckerTransform.position, _capsuleSize, CapsuleDirection2D.Horizontal, 0, _throughGroundLayer) != null)
        {
            return true;
        }

        return false;
    }
}
