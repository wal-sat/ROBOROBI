using UnityEngine;

public enum GameSceneState { None, Playing, Sleep, Clear, Option }

public class GameSceneInputManger : MonoBehaviour, IInputLockable
{
    [SerializeField] private GameScenePlayingInput _gameScenePlayingInput;
    [SerializeField] private GameSceneOptionInput _gameSceneOptionInput;
    [SerializeField] private GameSceneSleepInput _gameSceneSleepInput;
    [SerializeField] private GameSceneClearInput _gameSceneClearInput;
    [SerializeField] private StageManager _stageManager;

    private GameSceneState _gameSceneState;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _stageManager.ChangeGameSceneState = ChangeGameSceneState;   
    }

    private void Update()
    {
        switch (_gameSceneState)
        {
            case GameSceneState.Playing:
                _gameScenePlayingInput.InputUpdate();
                break;
            case GameSceneState.Sleep:
                _gameSceneSleepInput.InputUpdate();
                break;
            case GameSceneState.Clear:
                _gameSceneClearInput.InputUpdate();
                break;
            case GameSceneState.Option:
                _gameSceneOptionInput.InputUpdate();
                break;
            default:
                break;
        }
    }

    // ----- Private Methods -----

    private void ChangeGameSceneState(GameSceneState gameSceneState)
    {
        _gameSceneState = gameSceneState;

        switch (_gameSceneState)
        {
            case GameSceneState.Playing:
                _gameScenePlayingInput.InputInitialize();
                break;
            case GameSceneState.Sleep:
                _gameSceneSleepInput.InputInitialize();
                break;
            case GameSceneState.Clear:
                break;
            case GameSceneState.Option:
                break;
            default:
                break;
        }
    }
}
