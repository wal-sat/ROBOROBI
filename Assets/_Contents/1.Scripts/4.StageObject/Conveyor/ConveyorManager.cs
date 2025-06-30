using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private float _additionalSpeed;

    private const float UnsubscribeBufferTime = 0.1f;

    private List<Conveyor> _rightConveyorList = new List<Conveyor>();
    private List<Conveyor> _leftConveyorList = new List<Conveyor>();
    private CancellationTokenSource _rightCancellationTokenSource;
    private CancellationTokenSource _leftCancellationTokenSource;
    private bool _canUnsubscribeRight;
    private bool _canUnsubscribeLeft;

    // ----- Life Cycle Methods -----

    private void Update()
    {
        if (_playerMovementManager.IsLanding && _canUnsubscribeRight)
        {
            _canUnsubscribeRight = false;
            _playerMovementManager.UnsubscribeSpeedAdjustCallback(RightConveyor);
        }

        if (_playerMovementManager.IsLanding && _canUnsubscribeLeft)
        {
            _canUnsubscribeLeft = false;
            _playerMovementManager.UnsubscribeSpeedAdjustCallback(LeftConveyor);
        }
    }

    // ----- Public Methods -----

    public void StageObjectInitialize()
    {
        _playerMovementManager.UnsubscribeSpeedAdjustCallback(RightConveyor);
        _playerMovementManager.UnsubscribeSpeedAdjustCallback(LeftConveyor);

        _rightConveyorList.Clear();
        _leftConveyorList.Clear();

        _rightCancellationTokenSource?.Cancel();
        _leftCancellationTokenSource?.Cancel();
        _rightCancellationTokenSource?.Dispose();
        _leftCancellationTokenSource?.Dispose();

        _canUnsubscribeRight = false;
        _canUnsubscribeLeft = false;
    }

    public void Register(Conveyor conveyor, bool isRightDirection)
    {
        if (conveyor == null) return;

        if (isRightDirection && !_rightConveyorList.Contains(conveyor))
        {
            _rightConveyorList.Add(conveyor);
            _playerMovementManager.SubscribeSpeedAdjustCallback(RightConveyor);

            _rightCancellationTokenSource?.Cancel();
        }

        if (!isRightDirection && !_leftConveyorList.Contains(conveyor))
        {
            _leftConveyorList.Add(conveyor);
            _playerMovementManager.SubscribeSpeedAdjustCallback(LeftConveyor);

            _leftCancellationTokenSource?.Cancel();
        }
    }

    public void Unregister(Conveyor conveyor, bool isRightDirection)
    {
        if (isRightDirection && _rightConveyorList.Contains(conveyor))
        {
            _rightConveyorList.Remove(conveyor);

            if (_rightConveyorList.Count == 0)
            {
                _rightCancellationTokenSource?.Dispose();
                _rightCancellationTokenSource = new CancellationTokenSource();
                UnsubscribeRightBuffer(_rightCancellationTokenSource.Token).Forget();
            }
        }

        if (!isRightDirection && _leftConveyorList.Contains(conveyor))
        {
            _leftConveyorList.Remove(conveyor);

            if (_leftConveyorList.Count == 0)
            {
                _leftCancellationTokenSource?.Dispose();
                _leftCancellationTokenSource = new CancellationTokenSource();
                UnsubscribeLeftBuffer(_leftCancellationTokenSource.Token).Forget();
            }
        }
    }

    // ----- Private Methods -----

    private float RightConveyor()
    {
        return _additionalSpeed;
    }

    private float LeftConveyor()
    {
        return -_additionalSpeed;
    }

    private async UniTaskVoid UnsubscribeRightBuffer(CancellationToken cancellationToken)
    {
        await UniTask.WaitForSeconds(UnsubscribeBufferTime, cancellationToken: cancellationToken);

        _canUnsubscribeRight = true;
    }

    private async UniTaskVoid UnsubscribeLeftBuffer(CancellationToken cancellationToken)
    {
        await UniTask.WaitForSeconds(UnsubscribeBufferTime, cancellationToken: cancellationToken);

        _canUnsubscribeLeft = true;
    }
}
