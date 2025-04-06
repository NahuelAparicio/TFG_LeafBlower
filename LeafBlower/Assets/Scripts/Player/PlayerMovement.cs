using FMODUnity;
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
    [SerializeField] private float _footstepCooldown = 0.5f; // Tiempo de espera entre pasos
    private float _lastFootstepTime; // Para rastrear el último momento en que se reprodujo un paso
    private bool _wasGrounded; // Para detectar cuando el personaje toca el suelo nuevamente

    public float lastGroundedTime;
    public bool isJumping = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _firstPersonController = GetComponent<PlayerController>();
        _lastFootstepTime = 0f;
        _wasGrounded = false;
    }

    public void HandleAllMovement()
    {
        bool isGrounded = _characterController.isGrounded;

        // Si acaba de tocar el suelo después de estar en el aire
        if (isGrounded && !_wasGrounded)
        {
            _lastFootstepTime = 0f; // Reiniciamos el contador de pasos
        }

        _wasGrounded = isGrounded; // Actualizamos el estado anterior

        if (isGrounded && !isJumping)
        {
            _velocity.y = -_antiBump;
            lastGroundedTime = Time.time;
        }

        Vector3 _move = GetDirectionNormalized();
        _move.y = 0;
        float speed = _firstPersonController.Inputs.IsSprinting ? _sprintSpeed : _moveSpeed;
        _characterController.Move(_move * Time.deltaTime * speed);

        // Solo reproducimos sonidos de pasos si estamos en el suelo
        if (_move != Vector3.zero && isGrounded)
        {
            // Comprueba si ha pasado suficiente tiempo desde el último paso
            if (Time.time >= _lastFootstepTime + _footstepCooldown)
            {
                RuntimeManager.PlayOneShot("event:/Character/FootSteps/FootSteps_Concrete", transform.position);
                _lastFootstepTime = Time.time; // Actualiza el tiempo del último paso
            }

            gameObject.transform.forward = _move;
        }
        else if (!isGrounded)
        {
            // Si no está en contacto con el suelo, mantenemos la orientación pero no reproducimos sonidos
            if (_move != Vector3.zero)
            {
                gameObject.transform.forward = _move;
            }
        }

        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    public void AddExternalJumpForce(float speed)
    {
        RuntimeManager.PlayOneShot("event:/Interactables/Platform/Platform_Press", transform.position);
        _velocity.y = 0;
        _velocity.y += Mathf.Sqrt(speed * -2.0f * _gravity);
    }

    public void Jump()
    {
        RuntimeManager.PlayOneShot("event:/Character/Jump/Jump_Concrete", transform.position);
        if (!_characterController.isGrounded) return;
        _velocity.y += Mathf.Sqrt(_jumpSpeed * -2.0f * _gravity);
    }

    private Vector3 GetDirectionNormalized() => Utils.GetCameraForwardNormalized(_firstPersonController.MainCamera) * _firstPersonController.Inputs.GetPlayerMovement().y + Utils.GetCameraRightNormalized(_firstPersonController.MainCamera) * _firstPersonController.Inputs.GetPlayerMovement().x;
}