using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private ConveyorManager _conveyorManager;
    [SerializeField] private Transform _landingCheckerTransform;
    [SerializeField] private bool _isRightDirection;

    private const float OffsetY = 0.2f;

    private FirstCallChecker _firstCallChecker = new FirstCallChecker();

    // ----- Private Methods -----

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (this.transform.position.y + this.gameObject.transform.localScale.y / 2 - OffsetY <= _landingCheckerTransform.position.y)
            {
                if (_firstCallChecker.Check())
                {
                    _conveyorManager.Register(this, _isRightDirection);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _firstCallChecker.Reset();
            _conveyorManager.Unregister(this, _isRightDirection);
        }
    }
}
