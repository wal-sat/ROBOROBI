using UnityEngine;
using Unity.Cinemachine;

[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")]
public class CameraConfine : CinemachineExtension 
{
    private const float PositionZ = -20f;
    private Vector2 _minPosition;
    private Vector2 _maxPosition;

    // ----- Public Methods -----

    public void SetMoveRange(Vector2 minPosition, Vector2 maxPosition)
    {
        _minPosition = minPosition;
        _maxPosition = maxPosition;
    }

    // ----- Private Methods -----

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var newPos = state.RawPosition;

            float camHalfHeight = vcam.State.Lens.OrthographicSize;
            float camHalfWidth = camHalfHeight * vcam.State.Lens.Aspect;
            float camWidth = camHalfWidth * 2f;
            float camHeight = camHalfHeight * 2f;

            float regionWidth = _maxPosition.x - _minPosition.x;
            float regionHeight = _maxPosition.y - _minPosition.y;

            newPos.z = PositionZ;

            if (regionWidth >= camWidth)
            {
                float minX = _minPosition.x + camHalfWidth;
                float maxX = _maxPosition.x - camHalfWidth;
                newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            }
            else
            {
                newPos.x = (_minPosition.x + _maxPosition.x) * 0.5f;
            }

            if (regionHeight >= camHeight)
            {
                float minY = _minPosition.y + camHalfHeight;
                float maxY = _maxPosition.y - camHalfHeight;
                newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
            }
            else
            {
                newPos.y = (_minPosition.y + _maxPosition.y) * 0.5f;
            }

            state.RawPosition = newPos;
            this.transform.position = newPos;
        }
    }
}

