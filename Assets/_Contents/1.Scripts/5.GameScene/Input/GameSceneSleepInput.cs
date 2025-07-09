using UnityEngine;

public enum GameSceneSleepState { Menu, Camera }

public class GameSceneSleepInput : MonoBehaviour
{
    [SerializeField] private StageManager _stageManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private SavePointManager _savePointManager;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private SleepCameraMovement _sleepCameraMovement;
    [SerializeField] private GameSceneUIManager _gameSceneUIManager;

    private GameSceneSleepState _gameSceneSleepState;
    private bool _wasPushingS;
    private bool _wasPushingE;

    private Vector2 _sleepCameraPosition;

    // ----- Life Cycle Methods -----

    // ----- Public Methods -----

    public void InputInitialize()
    {
        _sleepCameraMovement.SetSleepCameraArea(_savePointManager.CurrentSavePoint.SleepCameraArea);

        _sleepCameraPosition = new Vector2(_savePointManager.CurrentSavePoint.transform.position.x, _savePointManager.CurrentSavePoint.transform.position.y);
        _sleepCameraMovement.MoveSleepCameraInitPosition(_sleepCameraPosition);
    }

    public void InputUpdate()
    {
        LeftDirection(S_InputSystemManager.Instance.LeftDirection);

        if (S_InputSystemManager.Instance.IsPushingS && !_wasPushingS)
        {
            _wasPushingS = true;
            OnPushingS();
        }
        else if (!S_InputSystemManager.Instance.IsPushingS && _wasPushingS)
        {
            _wasPushingS = false;
        }

        if (S_InputSystemManager.Instance.IsPushingE && !_wasPushingE)
        {
            _wasPushingE = true;
            OnPushingE();
        }
        else if (!S_InputSystemManager.Instance.IsPushingE && _wasPushingE)
        {
            _wasPushingE = false;
        }
    }

    // ----- Private Methods -----

    private void LeftDirection(Vector2 leftDirection)
    {
        switch (_gameSceneSleepState)
        {
            case GameSceneSleepState.Menu:
                if (leftDirection != Vector2.zero)
                {
                    _gameSceneSleepState = GameSceneSleepState.Camera;
                    _cameraManager.ChangeCameraKind(CameraKind.Sleep);
                    _gameSceneUIManager.DisplayUI(GameSceneUIState.SleepCamera);
                }
                break;
            case GameSceneSleepState.Camera:
                _sleepCameraMovement.SleepCameraUpdate(leftDirection);
                break;
        }
    }

    private void OnPushingS()
    {
        switch (_gameSceneSleepState)
        {
            case GameSceneSleepState.Menu:
                _stageManager.PlayerActivate();
                break;
            case GameSceneSleepState.Camera:
                _gameSceneSleepState = GameSceneSleepState.Menu;
                _sleepCameraMovement.MoveSleepCameraInitPosition(_sleepCameraPosition);
                _cameraManager.ChangeCameraKind(CameraKind.Main);
                _gameSceneUIManager.DisplayUI(GameSceneUIState.Sleep);
                break;
        }
    }

    private void OnPushingE()
    {
        switch (_gameSceneSleepState)
        {
            case GameSceneSleepState.Menu:
                _playerManager.DeleteScrap();
                break;
            case GameSceneSleepState.Camera:
                _gameSceneSleepState = GameSceneSleepState.Menu;
                _sleepCameraMovement.MoveSleepCameraInitPosition(_sleepCameraPosition);
                _cameraManager.ChangeCameraKind(CameraKind.Main);
                _gameSceneUIManager.DisplayUI(GameSceneUIState.Sleep);
                break;
        }
    }
}
