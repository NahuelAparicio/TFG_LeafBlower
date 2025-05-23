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
    [SerializeField] private float _footstepCooldown = 0.3f;
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
        _previousPosition = transform.position;
        _lastLandSoundTime = -_landSoundCooldown;

        _hoverSoundInstance = RuntimeManager.CreateInstance(hoverEventPath);
        RuntimeManager.AttachInstanceToGameObject(_hoverSoundInstance, transform, GetComponent<Rigidbody>());
    }

    public void HandleAllMovement()
    {
        Vector3 currentPosition = transform.position;
        float currentTime = Time.time;
        bool isGrounded = _characterController.isGrounded;

        HandleLanding(isGrounded, currentTime);
        HandleGroundState(isGrounded, currentTime);
        HandleMovement(isGrounded, currentPosition, currentTime);
        HandleHoverAndGravity(isGrounded, currentTime);

        _characterController.Move(_velocity * Time.deltaTime);
        _previousPosition = currentPosition;
        _wasGrounded = isGrounded;

        // Asegura que el RPM se ponga a 0 si est�s en el suelo
        if (isGrounded)
        {
            currentHoverRPM = 0f;
            _hoverSoundInstance.setParameterByName(hoverParameter, currentHoverRPM);
        }
    }

    private void HandleLanding(bool isGrounded, float currentTime)
    {
        if (isGrounded && !_wasGrounded && currentTime >= _lastLandSoundTime + _landSoundCooldown)
        {
            RaycastHit hit;
            string landEvent = "event:/Character/Land/Land_Dirt"; // evento por defecto

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
            {
                Debug.Log("Ground tag at land: " + hit.collider.tag);

                switch (hit.collider.tag)
                {
                    case "Metal":
                        landEvent = "event:/Character/Land/Land_Metal";
                        break;
                    case "Grass":
                        landEvent = "event:/Character/Land/Land_Grass";
                        break;
                    case "Concrete":
                        landEvent = "event:/Character/Land/Land_Concrete";
                        break;
                    case "Wood":
                        landEvent = "event:/Character/Land/Land_Wood";
                        break;
                }
            }
            RuntimeManager.PlayOneShot(landEvent, transform.position);
            _lastLandSoundTime = currentTime;


            // Aseg�rate de detener completamente el sonido del hover
            isHovering = false;
            currentHoverRPM = 0f;
            _hoverSoundInstance.setParameterByName(hoverParameter, currentHoverRPM);

            if (IsHoverSoundPlaying())
            {
                _hoverSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
    }

    private void HandleGroundState(bool isGrounded, float currentTime)
    {
        if (isGrounded && !isJumping)
        {
            _velocity.y = -_antiBump;
            lastGroundedTime = currentTime;
        }
    }

    private void HandleMovement(bool isGrounded, Vector3 currentPosition, float currentTime)
    {
        Vector3 moveDir = GetDirectionNormalized();
        moveDir.y = 0f;

        float speed = _player.Inputs.isSprinting ? _sprintSpeed : _moveSpeed;
        _characterController.Move(moveDir * Time.deltaTime * speed);

        if (moveDir != Vector3.zero)
        {
            if(!_player.animator.GetBool("IsMoving"))
                _player.animator.SetBool("IsMoving", true);

            transform.forward = moveDir;

            if (isGrounded)
            {
                float movedDistance = Vector3.Distance(currentPosition, _previousPosition);

                if (movedDistance > _minMovementForStep && currentTime >= _lastFootstepTime + _footstepCooldown)
                {
                    if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f))
                    {
                        Debug.Log("Ground tag: " + hit.collider.tag);

                        string eventPath = "event:/Character/FootSteps/FootSteps_Dirt"; // valor por defecto

                        switch (hit.collider.tag)
                        {
                            case "Metal":
                                eventPath = "event:/Character/FootSteps/FootSteps_Metal";
                                break;
                            case "Grass":
                                eventPath = "event:/Character/FootSteps/FootSteps_Grass";
                                break;
                            case "Concrete":
                                eventPath = "event:/Character/FootSteps/FootSteps_Concrete";
                                break;
                            case "Wood":
                                eventPath = "event:/Character/FootSteps/FootSteps_Wood";
                                break;
                        }

                        RuntimeManager.PlayOneShot(eventPath, transform.position);
                    }
                    _lastFootstepTime = currentTime;
                }

            }
        }
        else
        {
            if (_player.animator.GetBool("IsMoving"))
                _player.animator.SetBool("IsMoving", false);

        }
    }

    private void HandleHoverAndGravity(bool isGrounded, float currentTime)
    {
        if (!isGrounded)
        {
            if (_player.Inputs.ShouldHover() && _player.Stamina.HasStamina())
            {
                _player.Stamina.StartConsumingStamina();
                _velocity.y = Mathf.Lerp(_velocity.y, 0f, Time.deltaTime * _hoverStabilizeSpeed);
                isHovering = true;
                _player.Inputs.isSprinting = false;

                if (!IsHoverSoundPlaying())
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

                if (currentHoverRPM <= 10f && IsHoverSoundPlaying())
                {
                    _hoverSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                }
            }
        }
    }

    private bool IsHoverSoundPlaying()
    {
        _hoverSoundInstance.getPlaybackState(out var state);
        return state == FMOD.Studio.PLAYBACK_STATE.PLAYING;
    }

    public void AddExternalJumpForce(float speed)
    {
        _velocity.y = Mathf.Sqrt(speed * -2f * _gravity);
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

        RaycastHit hit;
        string jumpEvent = "event:/Character/Jump/Jump_Dirt"; // predeterminado

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            Debug.Log("Ground tag at jump: " + hit.collider.tag);

            switch (hit.collider.tag)
            {
                case "Metal":
                    jumpEvent = "event:/Character/Jump/Jump_Metal";
                    break;
                case "Grass":
                    jumpEvent = "event:/Character/Jump/Jump_Grass";
                    break;
                case "Concrete":
                    jumpEvent = "event:/Character/Jump/Jump_Concrete";
                    break;
                case "Wood":
                    jumpEvent = "event:/Character/Jump/Jump_Wood";
                    break;
            }
        }

        RuntimeManager.PlayOneShot(jumpEvent, transform.position);
        _velocity.y = Mathf.Sqrt(_jumpSpeed * -2f * _gravity);

    }

    private Vector3 GetDirectionNormalized()
    {
        return Utils.GetCameraForwardNormalized(_player.MainCamera) * _player.Inputs.GetPlayerMovement().y +
               Utils.GetCameraRightNormalized(_player.MainCamera) * _player.Inputs.GetPlayerMovement().x;
    }

    private void OnDestroy()
    {
        _hoverSoundInstance.release();
    }
}
