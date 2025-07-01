using UnityEngine;

public class PlayerMovementDieToGetStuck : MonoBehaviour
{
    [SerializeField] private StageManager _stageManager;
    [SerializeField] private Transform _getStuckCheckerTransform;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _radius = 0.05f;

    private const float StandCirclePositionY = -0.05f;
    private const float CrouchCirclePositionY = -0.175f;

    // ----- Public Methods -----

    public void MovementUpdate(bool isDieToGetStuckLockable)
    {
        if (!isDieToGetStuckLockable && Physics2D.OverlapCircle(_getStuckCheckerTransform.position, _radius, _groundLayer) != null)
        {
            _stageManager.PlayerDeath().Forget();
        }
    }

    public void ChangeGetStuckTransform(bool isCrouching)
    {
        if (isCrouching)
        {
            _getStuckCheckerTransform.localPosition = new Vector3(0, CrouchCirclePositionY, 0f);
        }
        else
        {
            _getStuckCheckerTransform.localPosition = new Vector3(0, StandCirclePositionY, 0f);
        }
    }

    // ----- Gizmo Settings -----

    private Color gizmoColor = Color.red;
    private int segments = 16;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Vector3 center = _getStuckCheckerTransform.position;
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + Quaternion.Euler(0f, 0f, 0f) * Vector3.right * _radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i;
            Vector3 nextPoint = center + Quaternion.Euler(0f, 0f, angle) * Vector3.right * _radius;

            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
