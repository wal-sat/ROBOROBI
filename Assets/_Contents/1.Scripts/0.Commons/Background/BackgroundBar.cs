using UnityEngine;

public class BackgroundBar : MonoBehaviour
{
    [SerializeField] private PlayerMovementManager playerMovementManager;
    [SerializeField] private Transform leftTransform;
    [SerializeField] private Transform rightTransform;
    [SerializeField] private float moveSpeed;

    private float _LeftRightDistance;

    // ----- Life Cycle Methods -----

    private void Start()
    {
        _LeftRightDistance = rightTransform.position.x - leftTransform.position.x;
    }

    // ----- Public Methods -----

    public void BarUpdate(float mainCameraPositionY)
    {
        if (playerMovementManager == null || playerMovementManager.IsFacingRight)
        {
            this.gameObject.transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);
        }
        else
        {
            this.gameObject.transform.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
        }

        if (this.gameObject.transform.position.x < leftTransform.position.x)
        {
            this.gameObject.transform.position += new Vector3(_LeftRightDistance, 0f, 0f);
        }
        if (this.gameObject.transform.position.x > rightTransform.position.x)
        {
            this.gameObject.transform.position += new Vector3(-_LeftRightDistance, 0f, 0f);
        }

        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, mainCameraPositionY, this.gameObject.transform.position.z);
    }
}
