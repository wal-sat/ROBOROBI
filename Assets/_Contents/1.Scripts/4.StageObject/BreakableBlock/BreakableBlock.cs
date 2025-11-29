using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    [SerializeField] private BreakableBlockView _breakableBlockView;
    [SerializeField] private OnCollisionWithRigidbodyObject _onCollisionWithRigidbodyObject;

    private const float BreakTime = 0.25f;
    private const float OffsetY = 0.2f;

    private Collider2D _breakableBlockCollider;
    private FirstCallChecker _firstCallChecker = new FirstCallChecker();
    private CancellationTokenSource _cancellationTokenSource;


    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _breakableBlockCollider = this.gameObject.GetComponent<Collider2D>();

        _onCollisionWithRigidbodyObject.CollisionStayCallback += CollisionStay;
    }

    // ----- Public Methods -----

    public void BreakableBlockInitialize()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;

        _breakableBlockCollider.enabled = true;
        _breakableBlockView.ChangeView(true);
        _firstCallChecker.Reset();
    }

    // ----- Private Methods -----

    private void CollisionStay(RigidbodyObject rigidbodyObject)
    {
        IBreakBlock breakBlock = rigidbodyObject.GetComponent<IBreakBlock>();
        if (breakBlock != null)
        {
            if (this.transform.position.y + this.gameObject.transform.localScale.y / 2 - OffsetY <= rigidbodyObject.GetBoundsBottomY()) 
            {
                if (_firstCallChecker.Check())
                {
                    breakBlock.Register(this);

                    _cancellationTokenSource?.Cancel();
                    _cancellationTokenSource?.Dispose();
                    _cancellationTokenSource = new CancellationTokenSource();
                    Break(_cancellationTokenSource.Token).Forget();
                }
            }
        }
    }

    private async UniTaskVoid Break(CancellationToken cancellationToken)
    {
        await UniTask.WaitForSeconds(BreakTime, cancellationToken: cancellationToken);

        _breakableBlockCollider.enabled = false;
        _breakableBlockView.ChangeView(false);
        S_SEManager.Instance.Play("s_breakableBlock");
        
        _cancellationTokenSource = null;
    }
}
