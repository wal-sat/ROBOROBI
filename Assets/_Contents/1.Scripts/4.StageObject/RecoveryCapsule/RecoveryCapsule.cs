using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RecoveryCapsule : MonoBehaviour
{
    private enum RecoveryCapsuleState { Recovery, Deplete }

    [SerializeField] private PlayerActionManager _playerActionManager;
    [SerializeField] private RecoveryCapsuleManager _recoveryCapsuleManager;
    [SerializeField] private RecoveryCapsuleView _recoveryCapsuleView;
    [SerializeField] private IsTriggerWithPlayer _isTriggerWithPlayer;
    [SerializeField] private RecoveryCapsuleState _recoveryCapsuleState;
    [SerializeField] private float _coolTime;

    private CancellationTokenSource _cancellationTokenSource;
    private FirstCallChecker _firstCallChecker = new FirstCallChecker();

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _recoveryCapsuleManager.Register(this);

        _isTriggerWithPlayer.TriggerEnterCallback += TriggerEnter;
    }

    // ----- Public Methods -----

    public void RecoveryCapsuleInitialize()
    {
        _firstCallChecker.Reset();
        _recoveryCapsuleView.SpriteChange(true);

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }

    // ----- Private Methods -----

    private void TriggerEnter()
    {
        if (_firstCallChecker.Check())
        {
            _recoveryCapsuleView.SpriteChange(false);
            if (_recoveryCapsuleState == RecoveryCapsuleState.Recovery)
            {
                _playerActionManager.RecoveryActionTime();
            }
            else if (_recoveryCapsuleState == RecoveryCapsuleState.Deplete)
            {
                _playerActionManager.DepleteActionTime();
            }

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            RecoveryCapsuleCoolTime(_cancellationTokenSource.Token).Forget();

            S_SEManager.Instance.Play("s_recovery");
        }
    }

    private async UniTaskVoid RecoveryCapsuleCoolTime(CancellationToken cancellationToken)
    {
        await UniTask.WaitForSeconds(_coolTime, cancellationToken: cancellationToken);

        _firstCallChecker.Reset();
        _recoveryCapsuleView.SpriteChange(true);

        _cancellationTokenSource = null;
    }
}
