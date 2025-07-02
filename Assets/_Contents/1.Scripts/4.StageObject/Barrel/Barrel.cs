using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class Barrel : MonoBehaviour, IPlayerMovementPropertyLockable
{
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private BarrelManager _barrelManager;
    [SerializeField] private BarrelView _barrelView;
    [SerializeField] private float _blownUpPower;

    private const float StayTime = 1f;

    private CancellationTokenSource _cancellationTokenSource;
    private FirstCallChecker _firstCallChecker = new FirstCallChecker();
    private bool _isBlownUpPlayer;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _barrelManager.Register(this);
    }

    private void Update()
    {
        if (!_isBlownUpPlayer) return;

        if (_playerMovementManager.IsLanding || _playerMovementManager.IsSwapping)
        {
            _isBlownUpPlayer = false;
            _playerMovementManager.SetPlayerRunningLock(this, false);
        }
    }

    // ----- Public Methods -----

    public void BarrelInitialize()
    {
        _firstCallChecker.Reset();
        _barrelView.SpriteChange(true);

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;

        if (_isBlownUpPlayer)
        {
            _isBlownUpPlayer = false;
            _playerMovementManager.SetPlayerRunningLock(this, false);
        }
    }

    // ----- Private Methods -----

    private void OnTriggerEnter2D(Collider2D collider)
    {
        RigidbodyObject barrelObject = collider.GetComponent<RigidbodyObject>();
        if (barrelObject != null)
        {
            if (_firstCallChecker.Check())
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
                BarrelAsync(barrelObject, _cancellationTokenSource.Token).Forget();
            }
        }
    }

    private async UniTaskVoid BarrelAsync(RigidbodyObject barrelObject, CancellationToken cancellationToken)
    {
        _barrelView.SpriteChange(false);
        barrelObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, barrelObject.transform.position.z);
        barrelObject.Rigidbody.linearVelocity = Vector2.zero;
        barrelObject.gameObject.SetActive(false);
        if (barrelObject.CompareTag("Player"))
        {
            _isBlownUpPlayer = true;
            _playerMovementManager.SetPlayerRunningLock(this, true);
            _cameraManager.ChangeCameraKind(CameraKind.Transition);
        }

        await UniTask.WaitForSeconds(StayTime, cancellationToken: cancellationToken);

        _barrelView.SpriteChange(true);
        barrelObject.gameObject.SetActive(true);
        float rad = -this.transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)).normalized;
        barrelObject.Rigidbody.linearVelocity = new Vector3(direction.x * _blownUpPower, direction.y * _blownUpPower, 0f);
        if (barrelObject.CompareTag("Player"))
        {
            _cameraManager.ChangeCameraKind(CameraKind.Main);
        }

        await UniTask.WaitForSeconds(0.1f, cancellationToken: cancellationToken);

        _firstCallChecker.Reset();
        _barrelView.SpriteChange(true);
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }
}
