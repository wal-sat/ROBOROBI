using UnityEngine;
using DG.Tweening;

public class SleepCameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject _cinemachineCamera_sleep;
    [SerializeField] private float _moveSpeed;

    private CameraConfine _cameraConfine;
    private Tween _cameraMoveTween;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _cameraConfine = _cinemachineCamera_sleep.GetComponent<CameraConfine>();
    }

    // ----- Public Methods -----

    public void SetSleepCameraArea(StageAreaBase sleepCameraArea)
    {
        _cameraConfine.SetMoveRange(sleepCameraArea.MinPosition, sleepCameraArea.MaxPosition);
    }

    public void MoveSleepCameraInitPosition(Vector2 initPosition)
    {
        _cameraMoveTween = _cinemachineCamera_sleep.transform.DOMove(new Vector3(initPosition.x, initPosition.y, _cinemachineCamera_sleep.transform.position.z), 0.5f)
            .SetEase(Ease.OutCubic)
            .SetLink(this.gameObject)
            .OnComplete(() => _cameraMoveTween = null);
    }

    public void SleepCameraUpdate(Vector2 direction)
    {
        if (_cameraMoveTween != null)
        {
            _cameraMoveTween.Kill();
            _cameraMoveTween = null;
        }

        _cinemachineCamera_sleep.transform.position += new Vector3(direction.x * _moveSpeed * Time.deltaTime, direction.y * _moveSpeed * Time.deltaTime, 0f);
    }
}
