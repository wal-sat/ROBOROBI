using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Rendering;

public enum CameraState { Main, Transition, Sleep }

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _cinemachineBrain;
    [SerializeField] private GameObject _cinemachineCameraMain_0;
    [SerializeField] private GameObject _cinemachineCameraMain_1;
    [SerializeField] private GameObject _cinemachineCameraTransition_0;
    [SerializeField] private GameObject _cinemachineCameraTransition_1;
    [SerializeField] private GameObject _cinemachineCameraSleep;

    private const float BlendTime = 0.5f;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        ChangeCameraState(CameraState.Main);
    }

    // ----- Public Methods -----

    public void ChangeCameraState(CameraState cameraState)
    {
        _cinemachineBrain.DefaultBlend.Time = BlendTime;
        _cinemachineBrain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.HardOut;
        _cinemachineCameraMain_0.SetActive(false);
        _cinemachineCameraMain_1.SetActive(false);
        _cinemachineCameraTransition_0.SetActive(false);
        _cinemachineCameraTransition_1.SetActive(false);
        _cinemachineCameraSleep.SetActive(false);

        switch (cameraState)
        {
            case CameraState.Main:
                _cinemachineCameraMain_0.SetActive(true);
                _cinemachineCameraMain_1.SetActive(true);
                break;
            case CameraState.Transition:
                _cinemachineBrain.DefaultBlend.Time = 0.01f;
                _cinemachineBrain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
                _cinemachineCameraTransition_0.SetActive(true);
                _cinemachineCameraTransition_1.SetActive(true);
                break;
            case CameraState.Sleep:
                _cinemachineCameraSleep.SetActive(true);
                break;
        }
    }
}
