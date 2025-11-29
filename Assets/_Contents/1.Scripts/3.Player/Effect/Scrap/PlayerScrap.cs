using System;
using UnityEngine;

public class PlayerScrap : MonoBehaviour
{
    [SerializeField] private float _explosionForce;


    private const float PositionOffsetMultiplier = 0.2f;
    private const int FrameSkipNumber = 60;
    private const float RotateMin = 1000f;
    private const float RotateMax = 3000f;
    
    [HideInInspector] public int ScrapIndex;
    public Action<int, PlayerScrap> OnDestroyCallBack;

    private Rigidbody2D _scrapRigidbody2D;
    private Vector2 _destroyDistance;
    private int _frameSkip;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _scrapRigidbody2D = GetComponent<Rigidbody2D>();

        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = halfHeight * Camera.main.aspect;

        float height = halfHeight * 2f;
        float width  = halfWidth * 2f;

        _destroyDistance = new Vector2(width * 2, height * 2);
    }

    private void Update()
    {
        if (_frameSkip++ < FrameSkipNumber) return;
        _frameSkip = 0;

        if (IsOutOfDestroyDistance())
        {
            OnDestroyCallBack?.Invoke(ScrapIndex, this);
        }
    }

    // ----- Public Methods -----

    public void Explosion(float angleZ)
    {
        float randomAngle = UnityEngine.Random.Range(angleZ - 90, angleZ + 90) * Mathf.Deg2Rad;
        Vector3 randomDirection = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f).normalized;

        this.transform.position += randomDirection * PositionOffsetMultiplier;
        _scrapRigidbody2D.AddForce(randomDirection * _explosionForce, ForceMode2D.Impulse);

        int sign = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
        float rotate = UnityEngine.Random.Range(RotateMin, RotateMax);
        var impulse = sign * rotate * Mathf.Deg2Rad * _scrapRigidbody2D.inertia;
        _scrapRigidbody2D.AddTorque(impulse, ForceMode2D.Impulse);
    }

    //  ----- Private Methods -----

    private bool IsOutOfDestroyDistance()
    {
        Vector2 distance = Camera.main.transform.position - this.gameObject.transform.position;
        return Mathf.Abs(distance.x) > _destroyDistance.x || Mathf.Abs(distance.y) > _destroyDistance.y;
    }
}
