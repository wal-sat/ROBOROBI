using UnityEngine;

public class LineGenerator : GeneratorBase
{
    [SerializeField] private float _lineSize;

    protected override GameObject InstantiateObject(GameObject instantiateObject)
    {
        float randomX = UnityEngine.Random.Range(-_lineSize / 2f, _lineSize / 2f);
        Vector3 randomLocalPosition = new Vector3(randomX, 0, 0);

        Vector3 randomPosition = transform.TransformPoint(randomLocalPosition);

        Quaternion rotation = instantiateObject.transform.localRotation;
        return Instantiate(instantiateObject, randomPosition + Vector3.forward, rotation);
    }

    // ----- Gizmo Settings -----

    private Color _gizmoColor = Color.cyan;
    private float _lineThickness = 0.1f;

    void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;

        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        Gizmos.DrawCube(Vector3.zero, new Vector2(_lineSize, _lineThickness));

        Gizmos.matrix = oldMatrix;
    }
}
