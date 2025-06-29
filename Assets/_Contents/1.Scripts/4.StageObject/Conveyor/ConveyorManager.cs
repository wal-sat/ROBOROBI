using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementManager _playerMovementManager;
    [SerializeField] private float _additionalSpeed;

    private const float UnsubscribeBufferTime = 0.25f;

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
            _playerMovementManager.UnsubscribeSpeedAdjustCallback(RightConveyor);
        }

        if (_playerMovementManager.IsLanding && _canUnsubscribeLeft)
        {
            _playerMovementManager.UnsubscribeSpeedAdjustCallback(LeftConveyor);
        }
    }

    // ----- Public Methods -----

    public void Register(Conveyor conveyor, bool isRightDirection)
    {
        if (conveyor == null) return;

        if (isRightDirection && !_rightConveyorList.Contains(conveyor))
        {
            _rightConveyorList.Add(conveyor);
            _playerMovementManager.SubscribeSpeedAdjustCallback(RightConveyor);

            if (_rightCancellationTokenSource != null) _rightCancellationTokenSource.Cancel();
            _rightCancellationTokenSource = new CancellationTokenSource();
            _canUnsubscribeRight = false;
        }

        if (!isRightDirection && !_leftConveyorList.Contains(conveyor))
        {
            _leftConveyorList.Add(conveyor);
            _playerMovementManager.SubscribeSpeedAdjustCallback(LeftConveyor);

            if (_leftCancellationTokenSource != null) _leftCancellationTokenSource.Cancel();
            _leftCancellationTokenSource = new CancellationTokenSource();
            _canUnsubscribeLeft = false;
        }
    }

    public void Unregister(Conveyor conveyor, bool isRightDirection)
    {
        if (isRightDirection && _rightConveyorList.Contains(conveyor))
        {
            _rightConveyorList.Remove(conveyor);

            if (_rightConveyorList.Count == 0)
            {
                UnsubscribeRightBuffer(_rightCancellationTokenSource.Token).Forget();
            }
        }

        if (!isRightDirection && _leftConveyorList.Contains(conveyor))
        {
            _leftConveyorList.Remove(conveyor);

            if (_leftConveyorList.Count == 0)
            {
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

        if (_rightConveyorList.Count == 0) _canUnsubscribeRight = true;
    }

    private async UniTaskVoid UnsubscribeLeftBuffer(CancellationToken cancellationToken)
    {
        await UniTask.WaitForSeconds(UnsubscribeBufferTime, cancellationToken: cancellationToken);

        if (_leftConveyorList.Count == 0) _canUnsubscribeLeft = true;
    }
}
