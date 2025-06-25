using UnityEngine;

public class PointGenerator : GeneratorBase
{
    protected override GameObject InstantiateObject(GameObject instantiateObject)
    {
        Quaternion rotation = instantiateObject.transform.localRotation;
        return Instantiate(instantiateObject, transform.position + Vector3.forward, rotation);
    }

    // ----- Gizmo Settings -----

    private Color _gizmoColor = Color.cyan;
    private float _pointSize = 0.1f;

    void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawSphere(transform.position, _pointSize);
    }
}
