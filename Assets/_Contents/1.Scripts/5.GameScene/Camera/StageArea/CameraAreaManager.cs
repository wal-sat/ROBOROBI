using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEditor.Purchasing;
using UnityEngine;

public class CameraAreaManager : MonoBehaviour
{
    private enum CameraType { Main_0, Main_1, Transition_0, Transition_1, Sleep }

    private class CameraInfo
    {
        public CinemachineCamera cinemachineCamera { get; private set; }
        public CameraConfine cameraConfine { get; private set; }

        public CameraInfo(CinemachineCamera camera, CameraConfine confine)
        {
            cinemachineCamera = camera;
            cameraConfine = confine;
        }
    }

    [SerializeField] private GameObject _cinemachineCameraMain_0;
    [SerializeField] private GameObject _cinemachineCameraMain_1;
    [SerializeField] private GameObject _cinemachineCameraTransition_0;
    [SerializeField] private GameObject _cinemachineCameraTransition_1;
    [SerializeField] private GameObject _cinemachineCameraSleep;

    private Dictionary<CameraType, CameraInfo> _cameraDictionary = new Dictionary<CameraType, CameraInfo>();
    private List<CameraArea> _cameraAreaList = new List<CameraArea>();
    private CameraArea _currentCameraArea;
    private CameraType _currentCameraType;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _cameraDictionary.Add(CameraType.Main_0, new CameraInfo(_cinemachineCameraMain_0.GetComponent<CinemachineCamera>(), _cinemachineCameraMain_0.GetComponent<CameraConfine>()));
        _cameraDictionary.Add(CameraType.Main_1, new CameraInfo(_cinemachineCameraMain_1.GetComponent<CinemachineCamera>(), _cinemachineCameraMain_1.GetComponent<CameraConfine>()));
        _cameraDictionary.Add(CameraType.Transition_0, new CameraInfo(_cinemachineCameraTransition_0.GetComponent<CinemachineCamera>(), _cinemachineCameraTransition_0.GetComponent<CameraConfine>()));
        _cameraDictionary.Add(CameraType.Transition_1, new CameraInfo(_cinemachineCameraTransition_1.GetComponent<CinemachineCamera>(), _cinemachineCameraTransition_1.GetComponent<CameraConfine>()));
        _cameraDictionary.Add(CameraType.Sleep, new CameraInfo(_cinemachineCameraSleep.GetComponent<CinemachineCamera>(), _cinemachineCameraSleep.GetComponent<CameraConfine>()));

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

    public int GetCameraAreaCount()
    {
        return _cameraAreaList.Count;
    }

    // ----- Private Methods -----

    private void ChangeCameraArea(CameraArea removedCameraArea = null)
    {
        if (removedCameraArea != null && _currentCameraArea != removedCameraArea) return;

        CameraArea cameraArea = _cameraAreaList.AsEnumerable().Reverse().OrderByDescending(item => item.CameraAreaPriority).FirstOrDefault();
        if (cameraArea == null) return;


        if (_currentCameraArea == null || _currentCameraArea != cameraArea)
        {
            if (_currentCameraType == CameraType.Main_0)
            {
                _cameraDictionary[CameraType.Main_0].cinemachineCamera.Priority = cameraArea.CameraAreaPriority;
                _cameraDictionary[CameraType.Main_0].cinemachineCamera.Lens.OrthographicSize = cameraArea.CameraSize;
                _cameraDictionary[CameraType.Main_0].cameraConfine.SetMoveRange(cameraArea.MinPosition, cameraArea.MaxPosition);

                _cameraDictionary[CameraType.Transition_0].cinemachineCamera.Priority = cameraArea.CameraAreaPriority + 1;
                _cameraDictionary[CameraType.Transition_0].cinemachineCamera.Lens.OrthographicSize = cameraArea.CameraSize;
                _cameraDictionary[CameraType.Transition_0].cameraConfine.SetMoveRange(cameraArea.MinPosition, cameraArea.MaxPosition);

                _cameraDictionary[CameraType.Main_1].cinemachineCamera.Priority = -1;
                _cameraDictionary[CameraType.Transition_1].cinemachineCamera.Priority = -1;

                _currentCameraArea = cameraArea;
                _currentCameraType = CameraType.Main_1;    
            }
            else if (_currentCameraType == CameraType.Main_1)
            {
                _cameraDictionary[CameraType.Main_1].cinemachineCamera.Priority = cameraArea.CameraAreaPriority;
                _cameraDictionary[CameraType.Main_1].cinemachineCamera.Lens.OrthographicSize = cameraArea.CameraSize;
                _cameraDictionary[CameraType.Main_1].cameraConfine.SetMoveRange(cameraArea.MinPosition, cameraArea.MaxPosition);

                _cameraDictionary[CameraType.Transition_1].cinemachineCamera.Priority = cameraArea.CameraAreaPriority + 1;
                _cameraDictionary[CameraType.Transition_1].cinemachineCamera.Lens.OrthographicSize = cameraArea.CameraSize;
                _cameraDictionary[CameraType.Transition_1].cameraConfine.SetMoveRange(cameraArea.MinPosition, cameraArea.MaxPosition);

                _cameraDictionary[CameraType.Main_0].cinemachineCamera.Priority = -1;
                _cameraDictionary[CameraType.Transition_0].cinemachineCamera.Priority = -1;

                _currentCameraArea = cameraArea;
                _currentCameraType = CameraType.Main_0;
            }

            _cameraDictionary[CameraType.Sleep].cinemachineCamera.Priority = cameraArea.CameraAreaPriority + 2;
            _cameraDictionary[CameraType.Sleep].cinemachineCamera.Lens.OrthographicSize = cameraArea.CameraSize;
        }
    }
}
