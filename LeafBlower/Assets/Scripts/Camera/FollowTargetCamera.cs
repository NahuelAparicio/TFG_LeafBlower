using UnityEngine;

public class FollowTargetCamera : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private Transform _followTarget;

    [SerializeField] private float _rotationalSpeed = 30f;
    [SerializeField] private float _topClamp = 70f;
    [SerializeField] private float _bottomClamp = -40f;

    private float _targetYaw;
    private float _targetPitch;

    private void LateUpdate()
    {
        CameraLogic();
    }

    private void ApplyRotations()
    {
        _followTarget.rotation = Quaternion.Euler(_targetPitch, _targetYaw, _followTarget.eulerAngles.z);
    }
    private void CameraLogic()
    {
        _targetPitch = UpdateRotation(_targetPitch, _player.Inputs.GetAimMoveDirection().y, _bottomClamp, _topClamp, true);
        _targetYaw = UpdateRotation(_targetYaw, _player.Inputs.GetAimMoveDirection().x, float.MinValue, float.MaxValue, false);
        ApplyRotations();
    }

    private float UpdateRotation(float currentRotation, float input,float min, float max, bool isXaxis)
    {
        currentRotation += isXaxis ? -input : input;
        return Mathf.Clamp(currentRotation, min, max);
    }
}
