using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float _zoomSmoothTime;

    private CinemachineFreeLook _camera;

    private float _originalFov;
    private float _targetFOV;
    private float _currentVelocity = 0f;

    private void Awake()
    {
        _camera = GetComponent<CinemachineFreeLook>();
        _originalFov = _camera.m_Lens.FieldOfView;
        _targetFOV = _originalFov;
    }

    private void Start()
    {
        GameEventManager.Instance.cameraEvents.onZoom += OnZoom;
        GameEventManager.Instance.cameraEvents.onResetZoom += OnResetZoom;
    }

    private void Update()
    {
        if (Mathf.Abs(_camera.m_Lens.FieldOfView - _targetFOV) > 0.1f)
        {
            _camera.m_Lens.FieldOfView = Mathf.SmoothDamp(
                _camera.m_Lens.FieldOfView, _targetFOV, ref _currentVelocity, _zoomSmoothTime);
        }
        else
        {
            _currentVelocity = 0;
        }
    }

    private void OnZoom(int zoomTarget)
    {
        //From Current Fov to zoomTarget
        _targetFOV = zoomTarget;
        _currentVelocity = 0;
    }

    private void OnResetZoom()
    {
        //Return to original fov
        _targetFOV = _originalFov;
        _currentVelocity = 0;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.cameraEvents.onZoom -= OnZoom;
        GameEventManager.Instance.cameraEvents.onResetZoom -= OnResetZoom;
    }
}
