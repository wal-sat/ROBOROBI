using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;

public enum CameraState { Main, Sleep }

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _cinemachineBrain;
    [SerializeField] private CinemachinePositionComposer[] _cinemachinePositionComposers;
    [SerializeField] private GameObject _cinemachineCamera_sleep;

    private const float BlendTime = 0.5f;
    private const float LockaheadTime = 0.4f;
    private const float LockaheadSmoothing = 4f;
    private const float DampingX = 0.5f;
    private const float DampingY = 0.5f;


    // ----- Life Cycle Methods -----

    private void Awake()
    {
        ChangeCameraState(CameraState.Main);
    }

    // ----- Public Methods -----

    public void ChangeCameraState(CameraState cameraState)
    {
        switch (cameraState)
        {
            case CameraState.Main:
                _cinemachineCamera_sleep.SetActive(false);
                break;
            case CameraState.Sleep:
                _cinemachineCamera_sleep.SetActive(true);
                break;
        }
    }

    public void EnableDamping(bool enable)
    {
        if (enable)
        {
            _cinemachineBrain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.HardOut, BlendTime);
            foreach (var positionComposer in _cinemachinePositionComposers)
            {
                positionComposer.Lookahead.Time = LockaheadTime;
                positionComposer.Lookahead.Smoothing = LockaheadSmoothing;
                positionComposer.Damping = new Vector3(DampingX, DampingY, 0f);
            }
        }
        else
        {
            _cinemachineBrain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 0f);
            foreach (var positionComposer in _cinemachinePositionComposers)
            {
                positionComposer.Lookahead.Time = 0f;
                positionComposer.Lookahead.Smoothing = 0f;
                positionComposer.Damping = Vector3.zero;
            }
        }
    }
}
