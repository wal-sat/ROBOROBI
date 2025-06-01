using UnityEngine;

public class BackgroundBar : MonoBehaviour
{
    // [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform _leftTransform;
    [SerializeField] private Transform _rightTransform;
    [SerializeField] private float _moveSpeed;

    private float _LeftRightDistance;

    // ----- Life Cycle Methods -----

    private void Start()
    {
        _LeftRightDistance = _rightTransform.position.x - _leftTransform.position.x;
    }

    // ----- Public Methods -----

    public void BarUpdate(float mainCameraPositionY)
    {
        // if (playerMovement == null)
        {
            this.gameObject.transform.position += new Vector3(-_moveSpeed * Time.deltaTime, 0f, 0f);
        }
        // else
        {
            // if (playerMovement.isFacingRight) this.gameObject.transform.position += new Vector3(-SPEED * Time.deltaTime, 0f ,0f); 
            // else this.gameObject.transform.position += new Vector3(SPEED * Time.deltaTime, 0f ,0f); 
        }

        if (this.gameObject.transform.position.x < _leftTransform.position.x)
        {
            this.gameObject.transform.position += new Vector3(_LeftRightDistance, 0f, 0f);
        }
        if (_rightTransform.position.x < this.gameObject.transform.position.x)
        {
            this.gameObject.transform.position += new Vector3(-_LeftRightDistance, 0f, 0f);
        }

        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, mainCameraPositionY, this.gameObject.transform.position.z);
    }
}
