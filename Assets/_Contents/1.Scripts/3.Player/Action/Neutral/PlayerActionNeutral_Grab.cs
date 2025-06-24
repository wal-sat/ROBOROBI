using UnityEngine;

public class PlayerActionNeutral_Grab : PlayerActionBase, IPlayerRunningLockable, IPlayerGravityDisable
{
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private PlayerViewManager _playerViewManager;
    [SerializeField] private RopeManager _ropeManager;

    private Transform _grabbedRopeTransform;
    private Vector2 _grabbedRopePositionPast;
    private bool _wasGrabRope;

    // ----- Public Methods -----

    public override void InitAction()
    {
        base.InitAction();

        _playerViewManager.IsGrabbing = true;

        if (_ropeManager.IsOverlapRope() && !_wasGrabRope)
        {
            _wasGrabRope = true;
            InitGrab();
        }
    }

    public override void InAction()
    {
        base.InAction();

        if (_ropeManager.IsOverlapRope() && !_wasGrabRope)
        {
            _wasGrabRope = true;
            InitGrab();
        }
        else if (_ropeManager.IsOverlapRope() && _wasGrabRope)
        {
            InGrab();
        }
        else if (!_ropeManager.IsOverlapRope() && _wasGrabRope)
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
        _playerMovementManager.SetPlayerRunningLock(this, true);
        _playerMovementManager.SetPlayerGravityDisable(this, true);
        _playerMovementManager.SetPlayerVelocityZero();

        _grabbedRopeTransform = _ropeManager.GetRopeTransform();
        _grabbedRopePositionPast = _grabbedRopeTransform.position;
        _player.transform.position = new Vector3(_grabbedRopeTransform.position.x, _player.transform.position.y, _player.transform.position.z);

        S_SEManager.Instance.Play("s_grabRope");
    }
    private void InGrab()
    {
        Vector2 differentPosition = (Vector2) _grabbedRopeTransform.position - _grabbedRopePositionPast;
        _player.transform.position += new Vector3(differentPosition.x, differentPosition.y, 0f);

        _grabbedRopePositionPast = _grabbedRopeTransform.position;
    }
    private void EndGrab()
    {
        _playerMovementManager.SetPlayerRunningLock(this, false);
        _playerMovementManager.SetPlayerGravityDisable(this, false);
    }
}
