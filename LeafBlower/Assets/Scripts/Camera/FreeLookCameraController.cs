using Cinemachine;
using UnityEngine;

public class FreeLookCameraController : MonoBehaviour
{
    private CinemachineFreeLook _cinemachine;
    private PlayerInputsActions _actions;

    [SerializeField] private float _rotationSpeed = 150f;
    [SerializeField] private float _deceleration = 3f;

    private float _xAxisValue = 0f;
    private float _currentRotationSpeed;

    private void Awake()
    {
        _cinemachine = GetComponent<CinemachineFreeLook>();
        _actions = new PlayerInputsActions();
        _actions.Camera.Enable();
    }

    void Update()
    {
        HandleCameraInputs();
    }

    private void HandleCameraInputs()
    {
        if (_actions.Camera.CameraRight.IsPressed())
        {
            _currentRotationSpeed = -_rotationSpeed;
        }
        else if (_actions.Camera.CameraLeft.IsPressed())
        {
            _currentRotationSpeed = _rotationSpeed;
        }
        else
        {
            _currentRotationSpeed = Mathf.Lerp(_currentRotationSpeed, 0f, _deceleration * Time.deltaTime);
        }
        HandleCameraRotation();
    }

    //Rotates the camera in X axis if Current Rotation is significant (is button pressed or is decelerating)
    private void HandleCameraRotation()
    {
        if(Mathf.Abs(_currentRotationSpeed) > 0.1f)
        {
            _xAxisValue += _currentRotationSpeed * Time.deltaTime;
            SetCinemachineXAxis();
        }
    }

    //Applies the rotation to Cinemachine Camera
    private void SetCinemachineXAxis()
    {
        _xAxisValue = (_xAxisValue + 180f) % 360f - 180f; // Keep angle between -180 and 180
        _cinemachine.m_XAxis.Value = _xAxisValue;
    }

    private void OnDestroy()
    {
        _actions.Camera.Disable();
    }

}
