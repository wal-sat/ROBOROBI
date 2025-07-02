using UnityEngine;
using Unity.Cinemachine;

public class SleepCameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject _cinemachineCamera_sleep;
    [SerializeField] private float _moveSpeed;

    private CameraConfine _cameraConfine;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _cameraConfine = _cinemachineCamera_sleep.GetComponent<CameraConfine>();
    }

    // ----- Public Methods -----

    public void SleepCameraInit(Vector2 initPosition, StageAreaBase sleepCameraArea)
    {
        _cinemachineCamera_sleep.transform.position = initPosition;
        _cameraConfine.SetMoveRange(sleepCameraArea.MinPosition, sleepCameraArea.MaxPosition);
    }

    public void SleepCameraMove(Vector2 direction)
    {
        _cinemachineCamera_sleep.transform.position += new Vector3(direction.x * _moveSpeed * Time.deltaTime, direction.y * _moveSpeed * Time.deltaTime, 0f);
    }
}
