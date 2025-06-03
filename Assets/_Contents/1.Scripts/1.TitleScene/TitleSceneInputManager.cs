using System;
using UnityEngine;

public enum TitleSceneState { None, PushS, Menu, SaveSlots, Settings, Exit }

public class TitleSceneInputManager : MonoBehaviour
{
    [SerializeField] private TitleScenePushS _titleScenePushS;
    [SerializeField] private TitleSceneMenu _titleSceneMenu;
    [SerializeField] private TitleSceneSaveSlots _titleSceneSaveSlots;
    [SerializeField] private TitleSceneExit _titleSceneExit;
    [SerializeField] private TitleSceneUIToolkit _titleSceneUIToolkit;

    private TitleSceneState _titleSceneState;
    private Vector2 _leftDirectionPast;
    private bool _wasPushingS;
    private bool _wasPushingE;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _titleScenePushS.OnChangeTitleSceneState = ChangeTitleSceneState;
        _titleSceneMenu.OnChangeTitleSceneState = ChangeTitleSceneState;
        _titleSceneSaveSlots.OnChangeTitleSceneState = ChangeTitleSceneState;
        _titleSceneExit.OnChangeTitleSceneState = ChangeTitleSceneState;

        ChangeTitleSceneState(TitleSceneState.PushS);
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
        switch (_titleSceneState)
        {
            case TitleSceneState.PushS:

                break;
            case TitleSceneState.Menu:
                _titleSceneMenu.Up();
                break;
            case TitleSceneState.SaveSlots:
                _titleSceneSaveSlots.Up();
                break;
            case TitleSceneState.Settings:

                break;
            case TitleSceneState.Exit:

                break;
        }
    }
    private void OnPushingDown()
    {
        switch (_titleSceneState)
        {
            case TitleSceneState.PushS:

                break;
            case TitleSceneState.Menu:
                _titleSceneMenu.Down();
                break;
            case TitleSceneState.SaveSlots:
                _titleSceneSaveSlots.Down();
                break;
            case TitleSceneState.Settings:

                break;
            case TitleSceneState.Exit:

                break;
        }
    }
    private void OnPushingLeft()
    {
        switch (_titleSceneState)
        {
            case TitleSceneState.PushS:

                break;
            case TitleSceneState.Menu:

                break;
            case TitleSceneState.SaveSlots:
                _titleSceneSaveSlots.Left();
                break;
            case TitleSceneState.Settings:

                break;
            case TitleSceneState.Exit:
                _titleSceneExit.Left();
                break;
        }
    }
    private void OnPushingRight()
    {
        switch (_titleSceneState)
        {
            case TitleSceneState.PushS:

                break;
            case TitleSceneState.Menu:

                break;
            case TitleSceneState.SaveSlots:
                _titleSceneSaveSlots.Right();
                break;
            case TitleSceneState.Settings:

                break;
            case TitleSceneState.Exit:
                _titleSceneExit.Right();
                break;
        }
    }
    private void OnPushingS()
    {
        switch (_titleSceneState)
        {
            case TitleSceneState.PushS:
                _titleScenePushS.Submit();
                break;
            case TitleSceneState.Menu:
                _titleSceneMenu.Submit();
                break;
            case TitleSceneState.SaveSlots:
                _titleSceneSaveSlots.Submit();
                break;
            case TitleSceneState.Settings:

                break;
            case TitleSceneState.Exit:
                _titleSceneExit.Submit();
                break;
        }
    }
    private void OnPushingE()
    {
        switch (_titleSceneState)
        {
            case TitleSceneState.PushS:

                break;
            case TitleSceneState.Menu:
                _titleSceneMenu.Cancel();
                break;
            case TitleSceneState.SaveSlots:
                _titleSceneSaveSlots.Cancel();
                break;
            case TitleSceneState.Settings:

                break;
            case TitleSceneState.Exit:
                _titleSceneExit.Cancel();
                break;
        }
    }

    private void ChangeTitleSceneState(TitleSceneState newState)
    {
        _titleSceneUIToolkit.ChangeTitleSceneStateUI(newState);

        switch (newState)
        {
            case TitleSceneState.PushS:
                _titleScenePushS.Initialize();
                break;
            case TitleSceneState.Menu:
                _titleSceneMenu.Initialize(_titleSceneState);
                break;
            case TitleSceneState.SaveSlots:
                _titleSceneSaveSlots.Initialize();
                break;
            case TitleSceneState.Settings:
                // Initialize settings if needed
                break;
            case TitleSceneState.Exit:
                _titleSceneExit.Initialize();
                break;
            default:
                break;
        }

        _titleSceneState = newState;
    }
}
