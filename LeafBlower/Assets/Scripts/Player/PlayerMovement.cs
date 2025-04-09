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
    private float _lastLandSoundTime;
    private const float _landSoundCooldown = 0.2f;

    private bool _wasGrounded;
    private Vector3 _previousPosition;

    public float lastGroundedTime;
    public bool isJumping = false;
    public bool isHovering = false;

    // FMOD hover sound
    private FMOD.Studio.EventInstance _hoverSoundInstance;
    [SerializeField] private string hoverEventPath = "event:/Tools/Hover";
    [SerializeField] private string hoverParameter = "RPM";
    [SerializeField] private float hoverRPMFadeSpeed = 3f;
    private float currentHoverRPM = 0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _player = GetComponent<PlayerController>();
        _lastFootstepTime = 0f;
        _lastLandSoundTime = -_landSoundCooldown;
        _wasGrounded = false;
        _previousPosition = transform.position;

        _hoverSoundInstance = RuntimeManager.CreateInstance(hoverEventPath);
        RuntimeManager.AttachInstanceToGameObject(_hoverSoundInstance, transform, GetComponent<Rigidbody>());
    }

    public void HandleAllMovement()
    {
        bool isGrounded = _characterController.isGrounded;

        if (isGrounded && !_wasGrounded)
        {
            if (Time.time >= _lastLandSoundTime + _landSoundCooldown)
            {
                RuntimeManager.PlayOneShot("event:/Character/Land/Land_Concrete", transform.position);
                _lastLandSoundTime = Time.time;
            }

            // Reset hover state and sound on landing
            isHovering = false;
            currentHoverRPM = 0f;
            _hoverSoundInstance.setParameterByName(hoverParameter, currentHoverRPM);
            _hoverSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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

            if (movedDistance > _minMovementForStep && Time.time >= _lastFootstepTime + _footstepCooldown)
            {
                RuntimeManager.PlayOneShot("event:/Character/FootSteps/FootSteps_Concrete", transform.position);
                _lastFootstepTime = Time.time;
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

                _velocity.y = Mathf.Lerp(_velocity.y, 0f, Time.deltaTime * _hoverStabilizeSpeed);
                isHovering = true;

                if (_hoverSoundInstance.getPlaybackState(out var state) == FMOD.RESULT.OK &&
                    state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    _hoverSoundInstance.start();
                }

                currentHoverRPM = Mathf.Lerp(currentHoverRPM, 2000f, Time.deltaTime * hoverRPMFadeSpeed);
                _hoverSoundInstance.setParameterByName(hoverParameter, currentHoverRPM);
            }
            else
            {
                _player.Stamina.StopConsumingStamina();

                float gravityMultiplier = isHovering ? _hoverGravityMultiplier : 1f;
                _velocity.y += _gravity * gravityMultiplier * Time.deltaTime;

                isHovering = false;

                currentHoverRPM = Mathf.Lerp(currentHoverRPM, 0f, Time.deltaTime * hoverRPMFadeSpeed);
                _hoverSoundInstance.setParameterByName(hoverParameter, currentHoverRPM);

                if (currentHoverRPM <= 10f &&
                    _hoverSoundInstance.getPlaybackState(out var state) == FMOD.RESULT.OK &&
                    state == FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    _hoverSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                }
            }
        }

        _characterController.Move(_velocity * Time.deltaTime);
        _previousPosition = transform.position;
    }

    public void AddExternalJumpForce(float speed)
    {
        _velocity.y = Mathf.Sqrt(speed * -2.0f * _gravity);
        isJumping = true;
        StartCoroutine(DelayedGroundCheck());
    }

    private System.Collections.IEnumerator DelayedGroundCheck()
    {
        yield return new WaitForSeconds(0.05f);

        if (!_characterController.isGrounded)
        {
            RuntimeManager.PlayOneShot("event:/Interactables/Platform/Platform_Press", transform.position);
        }
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

    private void OnDestroy()
    {
        _hoverSoundInstance.release();
    }
}
