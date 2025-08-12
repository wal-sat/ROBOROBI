using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private BackgroundManager backgroundManager;
    [SerializeField] private GameObject cameraFollowingObject;
    [SerializeField] private BackgroundBar[] backgroundBars;

    private Vector3[] _initBarPositions;
    
    // ----- Life Cycle Methods -----

    private void Awake()
    {
        if (backgroundManager != null) backgroundManager.Register(this);

        _initBarPositions = new Vector3[backgroundBars.Length];

        for (int i = 0; i < backgroundBars.Length; i++)
        {
            _initBarPositions[i] = backgroundBars[i].transform.position;
        }
    }

    private void Update()
    {
        for (int i = 0; i < backgroundBars.Length; i++)
        {
            backgroundBars[i].BarUpdate(mainCamera.transform.position.y);
        }  

        cameraFollowingObject.gameObject.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, cameraFollowingObject.gameObject.transform.position.z);
    }
    
    // ----- Public Methods -----

    public void Initialize()
    {
        for (int i = 0; i < backgroundBars.Length; i++)
        {
            backgroundBars[i].transform.position = new Vector3(_initBarPositions[i].x + cameraFollowingObject.transform.position.x, cameraFollowingObject.transform.position.y, _initBarPositions[i].z);
        }
    }
}
