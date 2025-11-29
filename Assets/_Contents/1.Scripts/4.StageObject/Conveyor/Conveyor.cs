using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private enum ConveyorDirection
    {
        Right = 1,
        Left = -1
    }

    private enum ConveyorSpeed
    {
        Slow = 15,
        Normal = 25,
        Fast = 35
    }

    [SerializeField] private ConveyorManager _conveyorManager;
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private OnCollisionWithRigidbodyObject _onCollisionWithRigidbodyObject;
    [SerializeField] private ConveyorDirection _conveyorDirection;
    [SerializeField] private ConveyorSpeed _conveyorSpeed;
    [SerializeField] private float _conveyorForce;

    private const float kp = 20f; // トルクの変数 (比例ゲイン)
    private const float kd = 2f;  // トルクの変数 (微分ゲイン)
    private const float OffsetY = 0.2f;
    private const float UnsubscribeBuffer = 0.05f;

    private CancellationTokenSource _cancellationTokenSource;
    private float _boundMaxY;
    private bool _isPlayerOnConveyor;
    private bool _canUnsubscribePlayerSpeed;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _conveyorManager.Register(this);
        _onCollisionWithRigidbodyObject.CollisionStayCallback += CollisionStay;
        _onCollisionWithRigidbodyObject.CollisionExitCallback += CollisionExit;

        _boundMaxY = _collider2D.bounds.max.y;
    }

    private void OnDestroy()
    {
        _onCollisionWithRigidbodyObject.CollisionStayCallback -= CollisionStay;
        _onCollisionWithRigidbodyObject.CollisionExitCallback -= CollisionExit;

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }

    private void Update()
    {
        if (_canUnsubscribePlayerSpeed && _playerMovementManager.IsLanding && !_isPlayerOnConveyor)
        {
            _canUnsubscribePlayerSpeed = false;
            _playerMovementManager.UnsubscribeSpeedAdjustCallback(GetAdditionalSpeed);
        }
    }

    // ----- Public Methods -----

    public void ConveyorInitialize()
    {
        _playerMovementManager.UnsubscribeSpeedAdjustCallback(GetAdditionalSpeed);
        _isPlayerOnConveyor = false;
        _canUnsubscribePlayerSpeed = false;
        
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }

    // ----- Private Methods -----

    private void CollisionStay(RigidbodyObject rigidbodyObject)
    {
        if (_boundMaxY - OffsetY <= rigidbodyObject.GetBoundsBottomY())
        {
            if (rigidbodyObject.CompareTag("Player"))
            {
                if (!_isPlayerOnConveyor)
                {
                    _isPlayerOnConveyor = true;
                    _canUnsubscribePlayerSpeed = false;
                    _playerMovementManager.SubscribeSpeedAdjustCallback(GetAdditionalSpeed);
                }
            }
            else
            {
                float speedDiff = GetAdditionalSpeed() - rigidbodyObject.Rigidbody.linearVelocityX;
                if (Mathf.Sign(speedDiff) == Mathf.Sign(GetAdditionalSpeed()))
                {
                    rigidbodyObject.Rigidbody.AddForce(new Vector2(speedDiff, 0), ForceMode2D.Force);
                }

                float omegaTarget = GetAdditionalSpeed() / rigidbodyObject.GetRadius();
                float omegaCurrent = rigidbodyObject.Rigidbody.angularVelocity * Mathf.Deg2Rad;
                float omegaDiff = omegaTarget - omegaCurrent;
                float torque = rigidbodyObject.Rigidbody.inertia * (kp * omegaDiff - kd * omegaCurrent);
                rigidbodyObject.Rigidbody.AddTorque(torque, ForceMode2D.Force);
            }
        }
    }

    private void CollisionExit(RigidbodyObject rigidbodyObject)
    {
        if (rigidbodyObject.CompareTag("Player") && _isPlayerOnConveyor)
        {
            _isPlayerOnConveyor = false;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            CanUnsubscribeBuffer(_cancellationTokenSource.Token).Forget();
        }
    }

    private async UniTaskVoid CanUnsubscribeBuffer(CancellationToken cancellationToken)
    {
        await UniTask.WaitForSeconds(UnsubscribeBuffer, cancellationToken: cancellationToken);

        _canUnsubscribePlayerSpeed = true;
        _cancellationTokenSource = null;
    }

    private float GetAdditionalSpeed()
    {
        return (float)_conveyorDirection * (float)_conveyorSpeed / 10f;
    }
}
