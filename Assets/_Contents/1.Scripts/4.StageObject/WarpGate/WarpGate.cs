using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using DG.Tweening;

public class WarpGate : MonoBehaviour, IPlayerMovementPropertyLockable
{
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private WarpGateManager _warpGateManager;
    [SerializeField] private WarpGateView _warpGateView;
    [SerializeField] private float _warpTweenDuration;
    [SerializeField] private float _coolTime;

    [SerializeField] private WarpGate _destinationWarpGate;

    public CancellationTokenSource CancellationTokenSource { get; private set; }

    private FirstCallChecker _firstCallChecker = new FirstCallChecker();

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _warpGateManager.Register(this);
    }

    // ----- Public Methods -----

    public void WarpGateInitialize()
    {
        _firstCallChecker.Reset();
        _warpGateView.SpriteChange(true);

        CancellationTokenSource?.Cancel();
        CancellationTokenSource?.Dispose();
        CancellationTokenSource = null;
    }

    public async UniTaskVoid CoolTime(float coolTime, CancellationToken cancellationToken)
    {
        if (_firstCallChecker.Check())
        {
            _warpGateView.SpriteChange(false);
        }

        await UniTask.WaitForSeconds(coolTime, cancellationToken: cancellationToken);

        _firstCallChecker.Reset();
        _warpGateView.SpriteChange(true);

        CancellationTokenSource?.Dispose();
        CancellationTokenSource = null;
    }


    // ----- Private Methods -----

    private void OnTriggerEnter2D(Collider2D collider)
    {
        RigidbodyObject warpedObject = collider.GetComponent<RigidbodyObject>();
        if (warpedObject != null)
        {
            if (_firstCallChecker.Check())
            {
                CancellationTokenSource?.Cancel();
                CancellationTokenSource?.Dispose();
                CancellationTokenSource = new CancellationTokenSource();
                WarpAsync(warpedObject, CancellationTokenSource.Token).Forget();

                S_SEManager.Instance.Play("s_warp");
            }
        }
    }

    private async UniTaskVoid WarpAsync(RigidbodyObject warpedObject, CancellationToken cancellationToken)
    {
        _destinationWarpGate.CancellationTokenSource?.Cancel();
        _destinationWarpGate.CancellationTokenSource?.Dispose();
        _destinationWarpGate.CancellationTokenSource = new CancellationTokenSource();
        _destinationWarpGate.CoolTime(_coolTime, _destinationWarpGate.CancellationTokenSource.Token).Forget();

        await UniTask.Yield(cancellationToken);

        Vector3 velocity = warpedObject.Rigidbody.linearVelocity;
        warpedObject.Rigidbody.linearVelocity = Vector3.zero;
        warpedObject.Collider.enabled = false;
        if (warpedObject.CompareTag("Player"))
        {
            _playerMovementManager.SetPlayerMovementPropertyLock(this, true);
            _cameraManager.ChangeCameraKind(CameraKind.Transition);
        }
        warpedObject.transform.DOMove(_destinationWarpGate.transform.position, _warpTweenDuration).SetEase(Ease.Linear).SetLink(warpedObject.gameObject);
        _warpGateView.SpriteChange(false);

        await UniTask.WaitForSeconds(_warpTweenDuration, cancellationToken: cancellationToken);

        warpedObject.Rigidbody.linearVelocity = velocity;
        warpedObject.Collider.enabled = true;
        if (warpedObject.CompareTag("Player"))
        {
            _playerMovementManager.SetPlayerMovementPropertyLock(this, false);
            _cameraManager.ChangeCameraKind(CameraKind.Main);
        }
        CoolTime(_coolTime - _warpTweenDuration, cancellationToken).Forget();
    }
}
