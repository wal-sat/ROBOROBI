using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;

public enum CameraKind { Main, Transition, Sleep }

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _cinemachineBrain;
    [SerializeField] private GameObject _cinemachineCameraMain_0;
    [SerializeField] private GameObject _cinemachineCameraMain_1;
    [SerializeField] private GameObject _cinemachineCameraTransition_0;
    [SerializeField] private GameObject _cinemachineCameraTransition_1;
    [SerializeField] private GameObject _cinemachineCameraSleep;

    private const float DefaultBlendTime = 0.5f;
    private const float ShortBlendTime = 0.15f;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        ChangeCameraKind(CameraKind.Main);
    }

    // ----- Public Methods -----

    public void ChangeCameraKind(CameraKind cameraKind)
    {
        _cinemachineBrain.DefaultBlend.Time = DefaultBlendTime;
        _cinemachineCameraTransition_0.SetActive(false);
        _cinemachineCameraTransition_1.SetActive(false);
        _cinemachineCameraSleep.SetActive(false);

        switch (cameraKind)
        {
            case CameraKind.Main:
                break;
            case CameraKind.Transition:
                _cinemachineBrain.DefaultBlend.Time = ShortBlendTime;
                _cinemachineCameraTransition_0.SetActive(true);
                _cinemachineCameraTransition_1.SetActive(true);
                break;
            case CameraKind.Sleep:
                _cinemachineCameraSleep.SetActive(true);
                break;
        }
    }
}
