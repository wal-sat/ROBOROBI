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

    private Tween _warpTween;
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
        _warpGateView.EnableView(true);

        CancellationTokenSource?.Cancel();
        CancellationTokenSource?.Dispose();
        CancellationTokenSource = null;
    }

    public async UniTaskVoid CoolTime(CancellationToken cancellationToken)
    {
        if (_firstCallChecker.Check())
        {
            _warpGateView.EnableView(false);
        }

        await UniTask.WaitForSeconds(_coolTime, cancellationToken: cancellationToken);

        _firstCallChecker.Reset();
        _warpGateView.EnableView(true);

        CancellationTokenSource = null;
    }


    // ----- Private Methods -----

    private void OnTriggerEnter2D(Collider2D collider)
    {
        WarpedObject warpedObject = collider.GetComponent<WarpedObject>();
        if (warpedObject != null)
        {
            if (_firstCallChecker.Check())
            {
                CancellationTokenSource?.Cancel();
                CancellationTokenSource?.Dispose();
                CancellationTokenSource = new CancellationTokenSource();
                WarpAsync(warpedObject.gameObject, CancellationTokenSource.Token).Forget();

                S_SEManager.Instance.Play("s_warp");
            }
        }
    }

    private async UniTaskVoid WarpAsync(GameObject gameObject, CancellationToken cancellationToken)
    {
        _destinationWarpGate.CancellationTokenSource?.Cancel();
        _destinationWarpGate.CancellationTokenSource?.Dispose();
        _destinationWarpGate.CancellationTokenSource = new CancellationTokenSource();
        _destinationWarpGate.CoolTime(_destinationWarpGate.CancellationTokenSource.Token).Forget();

        await UniTask.Yield(cancellationToken);

        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        Vector3 velocity = Vector3.zero;
        if (rigidbody != null)
        {
            velocity = rigidbody.linearVelocity;
            rigidbody.linearVelocity = Vector3.zero;
        }
        if (collider != null)
        {
            collider.enabled = false;
        }
        if (gameObject.CompareTag("Player"))
        {
            _playerMovementManager.SetPlayerMovementPropertyLockable(this, true);
            _cameraManager.ChangeCameraState(CameraState.Transition);
        }
        gameObject.transform.DOMove(_destinationWarpGate.transform.position, _warpTweenDuration).SetEase(Ease.Linear).SetLink(gameObject);

        await UniTask.WaitForSeconds(_warpTweenDuration, cancellationToken: cancellationToken);

        if (rigidbody != null)
        {
            rigidbody.linearVelocity = velocity;
        }
        if (collider != null)
        {
            collider.enabled = true;
        }
        if (gameObject.CompareTag("Player"))
        {
            _playerMovementManager.SetPlayerMovementPropertyLockable(this, false);
            _cameraManager.ChangeCameraState(CameraState.Main);
        }
        _warpGateView.EnableView(false);
        CoolTime(cancellationToken).Forget();
    }
}
