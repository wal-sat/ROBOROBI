using UnityEngine;

public enum GameSceneSleepState { None, Menu, Camera }

public class GameSceneSleepInput : MonoBehaviour
{
    private GameSceneSleepState _gameSceneSleepState;
    private Vector2 _leftDirectionPast;
    private bool _wasPushingS;
    private bool _wasPushingE;

    // ----- Life Cycle Methods -----

    // ----- Public Methods -----

    public void InputUpdate()
    {
        if (S_InputSystemManager.Instance.LeftDirection != Vector2.zero && _leftDirectionPast == Vector2.zero)
        {
            _leftDirectionPast = S_InputSystemManager.Instance.LeftDirection;
        }
        else if (S_InputSystemManager.Instance.LeftDirection == Vector2.zero && _leftDirectionPast != Vector2.zero)
        {
            _leftDirectionPast = Vector2.zero;
        }

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

    private void OnPushingS()
    {
        switch (_gameSceneSleepState)
        {
            case GameSceneSleepState.Menu:
                // Handle Menu state S button press
                break;
            case GameSceneSleepState.Camera:
                // Handle Camera state S button press
                break;
        }
    }

    private void OnPushingE()
    {
        switch (_gameSceneSleepState)
        {
            case GameSceneSleepState.Menu:
                // Handle Menu state E button press
                break;
            case GameSceneSleepState.Camera:
                // Handle Camera state E button press
                break;
        }
    }
}
