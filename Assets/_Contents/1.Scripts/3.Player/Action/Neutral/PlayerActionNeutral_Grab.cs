using UnityEngine;

public class PlayerActionNeutral_Grab : PlayerActionBase, IPlayerMovementPropertyLockable
{
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private PlayerViewManager _playerViewManager;
    [SerializeField] private PlayerOverlapRope _playerOverlapRope;

    private Transform _grabbedRopeTransform;
    private Vector2 _grabbedRopePositionPast;
    private bool _wasGrabRope;

    // ----- Public Methods -----

    public override void InitAction()
    {
        base.InitAction();

        _playerViewManager.IsGrabbing = true;

        if (_playerOverlapRope.IsOverlapRope() && !_wasGrabRope)
        {
            _wasGrabRope = true;
            InitGrab();
        }
    }

    public override void InAction()
    {
        base.InAction();

        if (_playerOverlapRope.IsOverlapRope() && !_wasGrabRope)
        {
            _wasGrabRope = true;
            InitGrab();
        }
        else if (_playerOverlapRope.IsOverlapRope() && _wasGrabRope)
        {
            InGrab();
        }
        else if (!_playerOverlapRope.IsOverlapRope() && _wasGrabRope)
        {
            _wasGrabRope = false;
            EndGrab();
        }

    }

    public override void EndAction()
    {
        base.EndAction();

        _playerViewManager.IsGrabbing = false;

        if (_wasGrabRope)
        {
            _wasGrabRope = false;
            EndGrab();
        }
    }

    public override void InitializeAction()
    {
        base.InitializeAction();

        _playerViewManager.IsGrabbing = false;

        if (_wasGrabRope)
        {
            _wasGrabRope = false;
            EndGrab();
        }
    }

    // ----- Private Methods -----

    private void InitGrab()
    {
        _playerMovementManager.SetPlayerMovementPropertyLock(this, true);
        _playerMovementManager.SetPlayerVelocityZero();

        _grabbedRopeTransform = _playerOverlapRope.GetRopeTransform();
        _grabbedRopePositionPast = _grabbedRopeTransform.position;
        _player.transform.position = new Vector3(_grabbedRopeTransform.position.x, _player.transform.position.y, _player.transform.position.z);

        S_SEManager.Instance.Play("s_rope");
    }
    private void InGrab()
    {
        _playerMovementManager.SetPlayerVelocityZero(); //他の処理と競合する場合は消すかも

        Vector2 differentPosition = (Vector2) _grabbedRopeTransform.position - _grabbedRopePositionPast;
        _player.transform.position += new Vector3(differentPosition.x, differentPosition.y, 0f);

        _grabbedRopePositionPast = _grabbedRopeTransform.position;
    }
    private void EndGrab()
    {
        _playerMovementManager.SetPlayerMovementPropertyLock(this, false);
    }
}
