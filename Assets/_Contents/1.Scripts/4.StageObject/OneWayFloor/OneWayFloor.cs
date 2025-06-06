using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class OneWayFloor : MonoBehaviour
{
    [SerializeField] private OneWayFloorManager _oneWayFloorManager;
    [SerializeField] private Transform _landingCheckerTransform;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private float _offsetY;
    [SerializeField] private float _bufferTime;

    [HideInInspector] public bool IsColliderEnable;

    private CancellationTokenSource _cancellationTokenSource;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _oneWayFloorManager.Register(this);

        IsColliderEnable = true;
    }

    private void Update()
    {
        if (_landingCheckerTransform.position.y < this.transform.position.y + _offsetY || !IsColliderEnable)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
            _collider2D.enabled = false;
        }
        else if (_cancellationTokenSource == null)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            EnableCollider(_cancellationTokenSource.Token).Forget();
        }
    }

    // ----- Private Methods ----

    private async UniTaskVoid EnableCollider(CancellationToken cancellationToken)
    {
        await UniTask.WaitForSeconds(_bufferTime, cancellationToken: cancellationToken);

        _collider2D.enabled = true;
    }
}
