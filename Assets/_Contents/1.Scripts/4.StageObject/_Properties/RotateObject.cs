using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] float _rotateSpeed;

    private void FixedUpdate()
    {
        this.gameObject.transform.Rotate(0, 0, _rotateSpeed * Time.fixedDeltaTime, Space.World);
    }
}
