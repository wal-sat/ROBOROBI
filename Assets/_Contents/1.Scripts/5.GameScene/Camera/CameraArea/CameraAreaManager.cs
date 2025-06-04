using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEditor.Purchasing;
using UnityEngine;

public class CameraAreaManager : MonoBehaviour
{
    private enum CameraType { Main_0, Main_1, Sleep }

    private class CameraInfo
    {
        public CinemachineCamera cinemachineCamera { get; private set; }
        public CinemachineConfiner2D cinemachineConfiner2D { get; private set; }

        public CameraInfo(CinemachineCamera camera, CinemachineConfiner2D confiner)
        {
            cinemachineCamera = camera;
            cinemachineConfiner2D = confiner;
        }
    }

    [SerializeField] private GameObject _cinemachineCamera_0;
    [SerializeField] private GameObject _cinemachineCamera_1;
    [SerializeField] private GameObject _cinemachineCamera_sleep;

    private Dictionary<CameraType, CameraInfo> _cameraDictionary = new Dictionary<CameraType, CameraInfo>();
    private List<CameraArea> _cameraAreaList = new List<CameraArea>();
    private CameraArea _currentCameraArea;
    private CameraType _currentCameraType;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _cameraDictionary.Add(CameraType.Main_0, new CameraInfo(_cinemachineCamera_0.GetComponent<CinemachineCamera>(), _cinemachineCamera_0.GetComponent<CinemachineConfiner2D>()));
        _cameraDictionary.Add(CameraType.Main_1, new CameraInfo(_cinemachineCamera_1.GetComponent<CinemachineCamera>(), _cinemachineCamera_1.GetComponent<CinemachineConfiner2D>()));
        _cameraDictionary.Add(CameraType.Sleep, new CameraInfo(_cinemachineCamera_sleep.GetComponent<CinemachineCamera>(), _cinemachineCamera_sleep.GetComponent<CinemachineConfiner2D>()));

        _currentCameraType = CameraType.Main_0;
    }

    // ----- Public Methods -----

    public void Register(CameraArea cameraArea)
    {
        if (!_cameraAreaList.Contains(cameraArea))
        {
            _cameraAreaList.Add(cameraArea);
            ChangeCameraArea();
        }
    }
    public void Unregister(CameraArea cameraArea)
    {
        if (_cameraAreaList.Contains(cameraArea))
        {
            _cameraAreaList.Remove(cameraArea);
            ChangeCameraArea(cameraArea);
        }
    }

    // ----- Private Methods -----

    private void ChangeCameraArea(CameraArea removedCameraArea = null)
    {
        if (removedCameraArea != null && _currentCameraArea != removedCameraArea) return;

        CameraArea cameraArea = _cameraAreaList.AsEnumerable().Reverse().OrderByDescending(item => item.CameraAreaPriority).FirstOrDefault();
        if (cameraArea == null) return;

        if (_currentCameraArea == null || _currentCameraArea != cameraArea)
        {
            _cameraDictionary[_currentCameraType].cinemachineCamera.Priority = cameraArea.CameraAreaPriority;
            _cameraDictionary[_currentCameraType].cinemachineCamera.Lens.OrthographicSize = cameraArea.CameraSize;
            _cameraDictionary[_currentCameraType].cinemachineConfiner2D.BoundingShape2D = cameraArea.Collider2D;

            _cameraDictionary[CameraType.Sleep].cinemachineCamera.Priority = cameraArea.CameraAreaPriority + 1;
            _cameraDictionary[CameraType.Sleep].cinemachineCamera.Lens.OrthographicSize = cameraArea.CameraSize;

            _currentCameraArea = cameraArea;
            if (_currentCameraType == CameraType.Main_0)
            {
                _currentCameraType = CameraType.Main_1;
            }
            else if (_currentCameraType == CameraType.Main_1)
            {
                _currentCameraType = CameraType.Main_0;
            }

            _cameraDictionary[_currentCameraType].cinemachineCamera.Priority = -1;
        }
    }
}
