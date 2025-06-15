using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    [SerializeField] private BreakableBlockManager _breakableBlockManager;
    [SerializeField] private BreakableBlockView _breakableBlockView;
    [SerializeField] private Transform _landingCheckerTransform;

    private const float BreakTime = 0.25f;
    private const float OffsetY = 0.2f;

    private Collider2D _breakableBlockCollider;
    private FirstCallChecker _firstCallChecker = new FirstCallChecker();
    private bool _isEnable;


    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _breakableBlockCollider = this.gameObject.GetComponent<Collider2D>();

        _breakableBlockManager.Register(this);
    }

    // ----- Public Methods -----

    public void Initialize()
    {
        _breakableBlockCollider.enabled = true;
        _breakableBlockView.ChangeView(true);

        _firstCallChecker.Reset();
    }

    // ----- Private Methods -----

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (this.transform.position.y + this.gameObject.transform.localScale.y / 2 - OffsetY <= _landingCheckerTransform.position.y) 
            {
                if (_firstCallChecker.Check())
                {
                    Break(destroyCancellationToken).Forget();
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
    }
}
