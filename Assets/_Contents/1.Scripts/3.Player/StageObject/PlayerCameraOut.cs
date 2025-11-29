using UnityEngine;

public class PlayerCameraOut : MonoBehaviour
{
    [SerializeField] private StageManager _stageManager;
    [SerializeField] private CameraAreaManager _cameraAreaManager;

    private const float DeathTime = 0.1f;

    private float _timer;

    // ----- Public Methods -----

    public void CameraUpdate(bool isActivePlayer)
    {
        if (!isActivePlayer) return;

        if (_cameraAreaManager.GetCameraAreaCount() == 0)
        {
            _timer += Time.fixedDeltaTime;
            if (_timer > DeathTime)
            {
                _timer = 0f;
                _stageManager.PlayerDeath().Forget();
            }
        }
        else
        {
            _timer = 0f;
        }
    }
}
