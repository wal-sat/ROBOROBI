using UnityEngine;

public class GameSceneClearInput : MonoBehaviour
{
    [SerializeField] private GameSceneUIManager _gameSceneUIManager;

    private int _clearIndex;
    int ClearIndex
    {
        get => _clearIndex;
        set
        {
            _clearIndex = Mathf.Clamp(value, 0, 1);
            _gameSceneUIManager.SelectClearOption(_clearIndex);
        }
    }

    private Vector2 _leftDirectionPast;
    private bool _wasPushingS;
    private bool _isLockS;

    // ----- Life Cycle Methods -----

    // ----- Public Methods -----

    public void InputInitialize()
    {
        ClearIndex = 0;
        _isLockS = S_InputSystemManager.Instance.IsPushingS;
    }

    public void InputUpdate()
    {
        Vector2 leftDirection = S_InputSystemManager.Instance.NormalizedLeftDirection;
        if (leftDirection == Vector2.left && _leftDirectionPast != Vector2.left)
        {
            Left();
        }
        else if (leftDirection == Vector2.right && _leftDirectionPast != Vector2.right)
        {
            Right();
        }
        _leftDirectionPast = leftDirection;

        if (_isLockS && !S_InputSystemManager.Instance.IsPushingS)
        {
            _isLockS = false;
        }
        else if (!_isLockS)
        {
            if (S_InputSystemManager.Instance.IsPushingS && !_wasPushingS)
            {
                _wasPushingS = true;
                Submit();
            }
            else if (!S_InputSystemManager.Instance.IsPushingS && _wasPushingS)
            {
                _wasPushingS = false;
            }
        }
        
    }

    // ----- Private Methods -----

    private void Left()
    {
        ClearIndex--;
        S_SEManager.Instance.Play("u_cursor");
    }

    private void Right()
    {
        ClearIndex++;
        S_SEManager.Instance.Play("u_cursor");
    }

    private void Submit()
    {
        switch (ClearIndex)
        {
            case 0:
                S_LoadSceneManager.Instance.LoadScene(SceneKind.Title).Forget();
                break;
            case 1:
                SceneKind sceneKind = S_LoadSceneManager.Instance.GetCurrentSceneKind();
                S_LoadSceneManager.Instance.LoadScene(sceneKind).Forget();
                break;
        }
        S_SEManager.Instance.Play("u_submit");
    }
}
