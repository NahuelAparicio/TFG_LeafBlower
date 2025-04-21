using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachinePovExtension : CinemachineExtension
{
    [SerializeField] private float sensX, sensY;
    [SerializeField] private PlayerController _player;
    private Vector3 _startingRotation;
    [SerializeField] private float clampAngle = 80f;


    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (_startingRotation == Vector3.zero)
                    _startingRotation = transform.localRotation.eulerAngles;


                Vector2 deltaInput = _player.Inputs.GetPlayerAim();

                if (Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero)
                {
                    // Es ratón
                    _startingRotation.x += deltaInput.x * sensX * deltaTime;
                    _startingRotation.y += deltaInput.y * sensY * deltaTime;
                }
                else
                {
                    // Es joystick
                    _startingRotation.x += deltaInput.x * sensX;
                    _startingRotation.y += deltaInput.y * sensY;
                }

                _startingRotation.y = Mathf.Clamp(_startingRotation.y, -clampAngle, clampAngle);

                state.RawOrientation = Quaternion.Euler(-_startingRotation.y, _startingRotation.x, 0f);
            }
        }
    }
}
