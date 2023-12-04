using UnityEngine;
using Cinemachine;

[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class lockZcam : CinemachineExtension
{
    [Tooltip("Lock the camera's Z position to this value")]
    public float m_ZPosition = 10;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (enabled && stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            pos.z = m_ZPosition;
            state.RawPosition = pos;
        }
    }
}
