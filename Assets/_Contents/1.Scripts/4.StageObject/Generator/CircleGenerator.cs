using UnityEngine;

public class CircleGenerator : GeneratorBase
{
    [SerializeField] private float _radius;

    protected override GameObject InstantiateObject(GameObject instantiateObject)
    {
        float angle = Random.Range(0f, 360f);
        Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
        Vector3 randomLocalPosition = direction * _radius;

        Vector3 randomPosition = transform.TransformPoint(randomLocalPosition);

        Quaternion rotation = instantiateObject.transform.localRotation;
        return Instantiate(instantiateObject, randomPosition + Vector3.forward, rotation);
    }

    // ----- Gizmo Settings -----

    private Color _gizmoColor = Color.cyan;
    public int segments = 64;

    void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;

        Vector3 center = transform.position;
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
