using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachinePovExtension : CinemachineExtension
{
    [SerializeField] private float clampAngle = 80f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private PlayerController _player;

    private Vector2 _startingRotation;
    private Vector2 _currentRotation;
    private Vector2 _rotationVelocity;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow && stage == CinemachineCore.Stage.Aim)
        {
            if (_startingRotation == Vector2.zero)
            {
                Vector3 initialRot = transform.localRotation.eulerAngles;
                _startingRotation = new Vector2(initialRot.y, -initialRot.x);
                _currentRotation = _startingRotation;
            }

            Vector2 deltaInput = _player.Inputs.GetPlayerAim();

            if (Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero)
            {
                // Es ratón
                _startingRotation.x += deltaInput.x * GameManager.Instance.sensX * deltaTime;
                _startingRotation.y += deltaInput.y * GameManager.Instance.sensY * deltaTime;
            }
            else
            {
                // Es joystick
                _startingRotation.x += deltaInput.x * GameManager.Instance.sensX;
                _startingRotation.y += deltaInput.y * GameManager.Instance.sensY;
            }

            _startingRotation.y = Mathf.Clamp(_startingRotation.y, -clampAngle, clampAngle);

            _currentRotation.x = Mathf.SmoothDampAngle(_currentRotation.x, _startingRotation.x, ref _rotationVelocity.x, smoothTime);
            _currentRotation.y = Mathf.SmoothDampAngle(_currentRotation.y, _startingRotation.y, ref _rotationVelocity.y, smoothTime);

            state.RawOrientation = Quaternion.Euler(-_currentRotation.y, _currentRotation.x, 0f);
        }
    }

}
