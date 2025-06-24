using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerActionSManager : MonoBehaviour
{
    [SerializeField] private PlayerActionBase _jumpAction;
    [SerializeField] private PlayerActionBase _bigJumpAction;
    [SerializeField] private PlayerActionBase _goDownAction;

    private const float InputBuffer = 0.05f;

    private bool _isPushingNone;
    private bool _isPushingUp;
    private bool _isPushingDown;
    private bool _wasJumped;
    private float _bufferTimer;

    private int _maxJumpTime;
    private int _jumpTime;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        SetMaxJumpTime(2);
    }

    // ----- Public Methods -----

    public void ActionUpdate(Vector2 leftDirection)
    {
        // S + Down : Go Down Action
        if (leftDirection == Vector2.down && !_isPushingDown)
        {
            _isPushingDown = true;
            CallInitAction(_goDownAction);
        }
        else if (leftDirection == Vector2.down && _isPushingDown)
        {
            CallInAction(_goDownAction);
        }
        else if (leftDirection != Vector2.down && _isPushingDown)
        {
            _isPushingDown = false;
            _bufferTimer = 0f;
            CallEndAction(_goDownAction);
        }

        _bufferTimer += Time.deltaTime;
        if (_bufferTimer <= InputBuffer) return;

        // S + Up : Big Jump Action
        if (leftDirection == Vector2.up && !_isPushingUp && !_wasJumped && IsJumpable())
        {
            _isPushingUp = true;
            _wasJumped = true;
            CallInitAction(_bigJumpAction);
        }
        else if (leftDirection == Vector2.up && _isPushingUp)
        {
            CallInAction(_bigJumpAction);
        }
        else if (leftDirection != Vector2.up && _isPushingUp)
        {
            _isPushingUp = false;
            _jumpTime--;
            CallEndAction(_bigJumpAction);
        }

        // S : Jump Action
        if ((leftDirection == Vector2.zero || leftDirection == Vector2.left || leftDirection == Vector2.right) && !_isPushingNone && !_wasJumped && IsJumpable())
        {
            _isPushingNone = true;
            _wasJumped = true;
            CallInitAction(_jumpAction);
        }
        else if ((leftDirection == Vector2.zero || leftDirection == Vector2.left || leftDirection == Vector2.right) && _isPushingNone)
        {
            CallInAction(_jumpAction);
        }
        else if ((leftDirection != Vector2.zero && leftDirection != Vector2.left && leftDirection != Vector2.right) && _isPushingNone)
        {
            _isPushingNone = false;
            _jumpTime--;
            CallEndAction(_jumpAction);
        }
    }

    public void ActionEnd()
    {
        _wasJumped = false;
        _bufferTimer = 0f;

        if (_isPushingUp)
        {
            _isPushingUp = false;
            _jumpTime--;
            CallEndAction(_bigJumpAction);
        }
        if (_isPushingDown)
        {
            _isPushingDown = false;
            CallEndAction(_goDownAction);
        }
        if (_isPushingNone)
        {
            _isPushingNone = false;
            _jumpTime--;
            CallEndAction(_jumpAction);
        }
    }

    public void RecureJumpTime()
    {
        _jumpTime = _maxJumpTime;
    }

    public void SetMaxJumpTime(int time)
    {
        _maxJumpTime = time;

        if (_jumpTime > _maxJumpTime) _jumpTime = _maxJumpTime;
    }

    // ----- Private Methods -----

    private void CallInitAction(PlayerActionBase action)
    {
        if (action.IsAcquired)
        {
            action.InitAction();
        }
    }
    private void CallInAction(PlayerActionBase action)
    {
        if (action.IsAcquired)
        {
            action.InAction();
        }
    }
    private void CallEndAction(PlayerActionBase action)
    {
        if (action.IsAcquired)
        {
            action.EndAction();
        }
    }

    /// <summary>
    /// _jumpTimeからジャンプ可能かどうかを返す
    /// </summary>
    private bool IsJumpable()
    {
        if (_jumpTime == -1) return true;
        if (_jumpTime > 0) return true;

        return false;
    }
}
