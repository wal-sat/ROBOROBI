using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerActionS_Jump : PlayerActionBase
{
    [SerializeField] private Rigidbody2D _playerRigidbody2D;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCancelMultiple;
    [SerializeField] private float _jumpCancelTime;

    private bool _canCancelAction;
    private bool _isInputCancel;

    // ----- Life Cycle Methods -----

    private void Update()
    {
        if (IsInAction && _canCancelAction && _isInputCancel)
        {
            JumpCancel();
        }
    }

    // ----- Public Methods -----

    public override void InitAction()
    {
        base.InitAction();

        _canCancelAction = false;
        _isInputCancel = false;
        MakeCancelable(destroyCancellationToken).Forget();

        _playerRigidbody2D.linearVelocity = new Vector2(_playerRigidbody2D.linearVelocityX, _jumpForce);

        S_SEManager.Instance.Play("p_jump");
    }

    public override void EndAction()
    {
        _isInputCancel = true;
    }

    public override void InitializeAction()
    {
        base.InitializeAction();

        JumpCancel();
    }

    // ----- Private Methods -----

    private void JumpCancel()
    {
        IsInAction = false;

        if (_playerRigidbody2D.linearVelocityY > 0)
        {
            _playerRigidbody2D.linearVelocity = new Vector2(_playerRigidbody2D.linearVelocityX, _playerRigidbody2D.linearVelocityY / _jumpCancelMultiple);
        }
    }

    private async UniTaskVoid MakeCancelable(CancellationToken cancellationToken)
    {
        await UniTask.WaitForSeconds(_jumpCancelTime, cancellationToken: cancellationToken);

        _canCancelAction = true;
    }
}
