using FMODUnity;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerController _player;
    private Vector3 _velocity;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _hoverStabilizeSpeed = 5f;
    [SerializeField] private float _gravity;
    [SerializeField] private float _hoverGravityMultiplier;
    [SerializeField] private float _antiBump;
    [SerializeField] private float _speedExternalForceToZero = 10f;
    [SerializeField] private float _footstepCooldown = 0.5f;
    [SerializeField] private float _minMovementForStep = 0.01f;

    private float _lastFootstepTime;
    private float _lastLandSoundTime; // Nueva variable
    private const float _landSoundCooldown = 0.2f; // Mínimo tiempo entre sonidos de caída

    private bool _wasGrounded;
    private Vector3 _previousPosition;

    public float lastGroundedTime;
    public bool isJumping = false;
    public bool isHovering = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _player = GetComponent<PlayerController>();
        _lastFootstepTime = 0f;
        _lastLandSoundTime = -_landSoundCooldown; // Para permitir que suene en la primera caída
        _wasGrounded = false;
        _previousPosition = transform.position;
    }

    public void HandleAllMovement()
    {
        bool isGrounded = _characterController.isGrounded;

        // Detectar aterrizaje con cooldown
        if (isGrounded && !_wasGrounded)
        {
            if (Time.time >= _lastLandSoundTime + _landSoundCooldown)
            {
                RuntimeManager.PlayOneShot("event:/Character/Land/Land_Concrete", transform.position);
                _lastLandSoundTime = Time.time;
            }
        }

        _wasGrounded = isGrounded;

        if (isGrounded && !isJumping)
        {
            _velocity.y = -_antiBump;
            lastGroundedTime = Time.time;
        }

        Vector3 _move = GetDirectionNormalized();
        _move.y = 0;
        float speed = _player.Inputs.isSprinting ? _sprintSpeed : _moveSpeed;
        _characterController.Move(_move * Time.deltaTime * speed);

        if (_move != Vector3.zero && isGrounded)
        {
            float movedDistance = Vector3.Distance(transform.position, _previousPosition);

            if (movedDistance > _minMovementForStep)
            {
                if (Time.time >= _lastFootstepTime + _footstepCooldown)
                {
                    RuntimeManager.PlayOneShot("event:/Character/FootSteps/FootSteps_Concrete", transform.position);
                    _lastFootstepTime = Time.time;
                }
            }

            gameObject.transform.forward = _move;
        }
        else if (!isGrounded && _move != Vector3.zero)
        {
            gameObject.transform.forward = _move;
        }
        if (!isGrounded)
        {
            if (isJumping && _player.Stamina.HasStamina())
            {
                _player.Stamina.StartConsumingStamina();

                // Hover real: mantén vertical estable
                _velocity.y = Mathf.Lerp(_velocity.y, 0f, Time.deltaTime * _hoverStabilizeSpeed);

                isHovering = true;
            }
            else
            {
                _player.Stamina.StopConsumingStamina();

                float gravityMultiplier = isHovering ? _hoverGravityMultiplier : 1f;
                _velocity.y += _gravity * gravityMultiplier * Time.deltaTime;

                isHovering = false;
            }
        }


        _characterController.Move(_velocity * Time.deltaTime);

        _previousPosition = transform.position;
    }

    public void AddExternalJumpForce(float speed)
    {
        if (_characterController.isGrounded) return;

        _velocity.y = 0;
        _velocity.y += Mathf.Sqrt(speed * -2.0f * _gravity);
    }

    public void Jump()
    {
        if (!_characterController.isGrounded) return;
        RuntimeManager.PlayOneShot("event:/Character/Jump/Jump_Concrete", transform.position);
        _velocity.y += Mathf.Sqrt(_jumpSpeed * -2.0f * _gravity);
    }

    private Vector3 GetDirectionNormalized() =>
        Utils.GetCameraForwardNormalized(_player.MainCamera) * _player.Inputs.GetPlayerMovement().y +
        Utils.GetCameraRightNormalized(_player.MainCamera) * _player.Inputs.GetPlayerMovement().x;
}
