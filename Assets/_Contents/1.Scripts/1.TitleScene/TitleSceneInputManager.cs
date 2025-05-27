using System;
using UnityEngine;

public enum TitleState { None, PushS, Menu, SaveSlots, Settings, Exit }

public class TitleSceneInputManager : MonoBehaviour
{
    [SerializeField] private TitleScenePushS _titleScenePushS;
    [SerializeField] private TitleSceneMenu _titleSceneMenu;
    [SerializeField] private TitleSceneSaveSlots _titleSceneSaveSlots;
    [SerializeField] private TitleSceneExit _titleSceneExit;
    [SerializeField] private TitleSceneUIToolkit _titleSceneUIToolkit;

    private TitleState _titleState;
    private Vector2 _leftDirectionPast;
    private bool _wasPushingS;
    private bool _wasPushingE;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _titleSceneMenu.OnChangeTitleState = ChangeTitleState;

        ChangeTitleState(TitleState.Menu);
    }

    private void Update()
    {
        // Input Left Direction
        if (S_InputSystemManager.Instance.NormalizedLeftDirection == Vector2.up && _leftDirectionPast != Vector2.up)
        {
            _leftDirectionPast = Vector2.up;
            OnPushingUp();
        }
        else if (S_InputSystemManager.Instance.NormalizedLeftDirection == Vector2.down && _leftDirectionPast != Vector2.down)
        {
            _leftDirectionPast = Vector2.down;
            OnPushingDown();
        }
        else if (S_InputSystemManager.Instance.NormalizedLeftDirection == Vector2.left && _leftDirectionPast != Vector2.left)
        {
            _leftDirectionPast = Vector2.left;
            OnPushingLeft();
        }
        else if (S_InputSystemManager.Instance.NormalizedLeftDirection == Vector2.right && _leftDirectionPast != Vector2.right)
        {
            _leftDirectionPast = Vector2.right;
            OnPushingRight();
        }
        else if (S_InputSystemManager.Instance.NormalizedLeftDirection == Vector2.zero && _leftDirectionPast != Vector2.zero)
        {
            _leftDirectionPast = Vector2.zero;
        }

        // Input S Button
        if (S_InputSystemManager.Instance.IsPushingS && !_wasPushingS)
        {
            _wasPushingS = true;
            OnPushingS();
        }
        else if (!S_InputSystemManager.Instance.IsPushingS && _wasPushingS)
        {
            _wasPushingS = false;
        }

        // Input E Button
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

    private void OnPushingUp()
    {
        switch (_titleState)
        {
            case TitleState.PushS:

                break;
            case TitleState.Menu:
                _titleSceneMenu.Up();
                break;
            case TitleState.SaveSlots:

                break;
            case TitleState.Settings:

                break;
            case TitleState.Exit:

                break;
        }
    }
    private void OnPushingDown()
    {
        switch (_titleState)
        {
            case TitleState.PushS:

                break;
            case TitleState.Menu:
                _titleSceneMenu.Down();
                break;
            case TitleState.SaveSlots:

                break;
            case TitleState.Settings:

                break;
            case TitleState.Exit:

                break;
        }
    }
    private void OnPushingLeft()
    {
        switch (_titleState)
        {
            case TitleState.PushS:

                break;
            case TitleState.Menu:

                break;
            case TitleState.SaveSlots:

                break;
            case TitleState.Settings:

                break;
            case TitleState.Exit:

                break;
        }
    }
    private void OnPushingRight()
    {
        switch (_titleState)
        {
            case TitleState.PushS:

                break;
            case TitleState.Menu:

                break;
            case TitleState.SaveSlots:

                break;
            case TitleState.Settings:

                break;
            case TitleState.Exit:

                break;
        }
    }
    private void OnPushingS()
    {
        switch (_titleState)
        {
            case TitleState.PushS:

                break;
            case TitleState.Menu:

                break;
            case TitleState.SaveSlots:

                break;
            case TitleState.Settings:

                break;
            case TitleState.Exit:

                break;
        }
    }
    private void OnPushingE()
    {
        switch (_titleState)
        {
            case TitleState.PushS:

                break;
            case TitleState.Menu:

                break;
            case TitleState.SaveSlots:

                break;
            case TitleState.Settings:

                break;
            case TitleState.Exit:

                break;
        }
    }
    
    private void ChangeTitleState(TitleState newState)
    {
        _titleState = newState;
    }
}
