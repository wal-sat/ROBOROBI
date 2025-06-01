using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private Camera _mianCamera;
    [SerializeField] private BackgroundManager _backgroundManager;
    [SerializeField] private GameObject _cameraFollowingObject;
    [SerializeField] private BackgroundBar[] _backgroundBars;

    private Vector3[] _initBarPositions;
    
    // ----- Life Cycle Methods -----

    private void Awake()
    {
        if (_backgroundManager != null) _backgroundManager.Register(this);

        _initBarPositions = new Vector3[_backgroundBars.Length];

        for (int i = 0; i < _backgroundBars.Length; i++)
        {
            _initBarPositions[i] = _backgroundBars[i].transform.position;
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _backgroundBars.Length; i++)
        {
            _backgroundBars[i].BarUpdate(_mianCamera.transform.position.y);
        }  

        _cameraFollowingObject.gameObject.transform.position = new Vector3(_mianCamera.transform.position.x, _mianCamera.transform.position.y, _cameraFollowingObject.gameObject.transform.position.z);
    }
    
    // ----- Public Methods -----

    public void Initialize()
    {
        for (int i = 0; i < _backgroundBars.Length; i++)
        {
            _backgroundBars[i].transform.position = new Vector3(_initBarPositions[i].x + _cameraFollowingObject.transform.position.x, _cameraFollowingObject.transform.position.y, _initBarPositions[i].z);
        }
    }
}
