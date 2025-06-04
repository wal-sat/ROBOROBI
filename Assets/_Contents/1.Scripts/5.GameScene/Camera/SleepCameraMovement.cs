using UnityEngine;
using Unity.Cinemachine;

public class SleepCameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject _cinemachineCamera_sleep;
    [SerializeField] private float _moveSpeed;

    private CinemachineConfiner2D _sleepConfiner2D;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _sleepConfiner2D = _cinemachineCamera_sleep.GetComponent<CinemachineConfiner2D>();
    }

    // ----- Public Methods -----

    public void SleepCameraInit(Vector2 initPosition, Collider2D confinerCollider)
    {
        _cinemachineCamera_sleep.transform.position = initPosition;
        _sleepConfiner2D.BoundingShape2D = confinerCollider;
    }

    public void SleepCameraMove(Vector2 direction)
    {
        _cinemachineCamera_sleep.transform.position += new Vector3(direction.x * _moveSpeed * Time.deltaTime, direction.y * _moveSpeed * Time.deltaTime, 0f);
    }
}
