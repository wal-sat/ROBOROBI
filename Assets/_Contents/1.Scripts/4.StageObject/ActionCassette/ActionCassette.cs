using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ActionCassette : MonoBehaviour
{
    private enum ActionCassetteType { Acquire, Forget }

    [SerializeField] private ActionCassetteManager _actionCassetteManager;
    [SerializeField] private ActionCassetteView _actionCassetteView;
    [SerializeField] private IsTriggerWithPlayer _isTriggerWithPlayer;

    [SerializeField] private ActionCassetteType _actionCassetteType;
    [SerializeField] private ActionKind _actionKind;

    private const float CoolTime = 5f;

    private CancellationTokenSource _cancellationTokenSource;
    private FirstCallChecker _firstCallChecker = new FirstCallChecker();

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _actionCassetteManager.Register(this);

        _isTriggerWithPlayer.TriggerEnterCallback += TriggerEnter;
    }

    // ----- Public Methods -----

    public void ActionCassetteInitialize()
    {
        _firstCallChecker.Reset();
        _actionCassetteView.ChangeSprite(true);

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }

    // ----- Private Methods -----

    private void TriggerEnter()
    {
        if (_firstCallChecker.Check())
        {
            _actionCassetteView.ChangeSprite(false);
            if (_actionCassetteType == ActionCassetteType.Acquire)
            {
                _actionCassetteManager.AcquireAction(_actionKind);
                S_SEManager.Instance.Play("s_actionCassette");
            }
            else if (_actionCassetteType == ActionCassetteType.Forget)
            {
                _actionCassetteManager.ForgetAction(_actionKind);
                S_SEManager.Instance.Play("s_actionCassetteMinus");
            }

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            ActionCassetteCoolTime(_cancellationTokenSource.Token).Forget();

        }
    }

    private async UniTaskVoid ActionCassetteCoolTime(CancellationToken cancellationToken)
    {
        await UniTask.WaitForSeconds(CoolTime, cancellationToken: cancellationToken);

        _firstCallChecker.Reset();
        _actionCassetteView.ChangeSprite(true);

        _cancellationTokenSource = null;
    }
}
