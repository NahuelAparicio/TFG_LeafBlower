using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerController _firstPersonController;

    private Vector3 _velocity;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _antiBump;
    [SerializeField] private float _speedExternalForceToZero = 10f;
    private Vector3 _externalForce;
    private bool _applyExternalForce = false;

    public float lastGroundedTime;
    public bool isJumping = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _firstPersonController = GetComponent<PlayerController>();
    }

    public void HandleAllMovement()
    {

        if (_characterController.isGrounded && !isJumping)
        {
            _velocity.y = -_antiBump;
            lastGroundedTime = Time.time;
        }
        Vector3 _move = GetDirectionNormalized();
        _move.y = 0;

        float speed = _firstPersonController.Inputs.IsSprinting ? _sprintSpeed : _moveSpeed;

        _characterController.Move(_move * Time.deltaTime * speed);

        if (_move != Vector3.zero)
        {
            gameObject.transform.forward = _move;
        }

        _velocity.y += _gravity * Time.deltaTime;
        if(_applyExternalForce)
        {
            _velocity += _externalForce;
            _applyExternalForce = false;
        }

        _characterController.Move(_velocity * Time.deltaTime);

        if(_externalForce != Vector3.zero)
        {
            _externalForce = Vector3.Lerp(_externalForce, Vector3.zero, Time.deltaTime * _speedExternalForceToZero);
        }
    }

    public void AddExternalForce(Vector3 addVelocity)
    {
        _applyExternalForce = true;
        _externalForce += addVelocity;
    }

    public void Jump()
    {
        if (!_characterController.isGrounded) return;
        _velocity.y += Mathf.Sqrt(_jumpSpeed * -2.0f * _gravity);
    }

    private Vector3 GetDirectionNormalized() => Utils.GetCameraForwardNormalized(_firstPersonController.MainCamera) * _firstPersonController.Inputs.GetPlayerMovement().y + Utils.GetCameraRightNormalized(_firstPersonController.MainCamera) * _firstPersonController.Inputs.GetPlayerMovement().x;
}
