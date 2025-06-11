using System;
using UnityEngine;

public class PlayerScrap : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Rigidbody2D _scrapRigidbody2D;
    [SerializeField] private float _explosionForce;
    [SerializeField] private Vector2 _destroyDistance;

    private const float PositionOffsetMultiplier = 0.1f;
    private const int FrameSkipNumber = 60;

    private Action<GameObject> OnDestroyCallBack;
    private int _frameSkip;

    // ----- Life Cycle Methods -----

    private void Update()
    {
        if (_frameSkip++ < FrameSkipNumber) return;
        _frameSkip = 0;

        if (IsOutOfDestroyDistance())
        {
            OnDestroyCallBack?.Invoke(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    // ----- Public Methods -----

    public void Explosion(float angleZ, Action<GameObject> action)
    {
        float randomAngle = UnityEngine.Random.Range(angleZ, angleZ + 180) * Mathf.Deg2Rad;
        Vector3 randomDirection = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f).normalized;

        this.transform.position += randomDirection * PositionOffsetMultiplier;
        _scrapRigidbody2D.AddForce(randomDirection * _explosionForce, ForceMode2D.Impulse);

        OnDestroyCallBack = action;
    }

    // ----- Private Methods -----

    private bool IsOutOfDestroyDistance()
    {
        Vector2 distance = _mainCamera.transform.position - this.gameObject.transform.position;
        return Mathf.Abs(distance.x) > _destroyDistance.x || Mathf.Abs(distance.y) > _destroyDistance.y;
    }
}
