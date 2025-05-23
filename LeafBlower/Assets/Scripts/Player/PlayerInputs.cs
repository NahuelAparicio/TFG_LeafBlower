using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private PlayerInputsActions _inputs;
    private PlayerController _player;

    public bool isSprinting;

    private float _jumpPressTime = -1f;
    [SerializeField] private float _jumpBufferTime = 0.125f;

    private bool _jumpHeld = false;
    private float _jumpHoldStartTime;
    [SerializeField] private float _hoverActivationTime = 0.5f;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _inputs = new PlayerInputsActions();
        _inputs.Player.Enable();

        _inputs.Player.Interact.performed += Interact_performed;
        _inputs.Player.Sprint.performed += Sprint_performed;
        _inputs.Player.Pause.performed += Pause_performed;
        _inputs.Player.Jump.performed += Jump_performed;
        _inputs.Player.Jump.canceled += Jump_canceled;
        _inputs.Player.Aspire.performed += Aspire_performed;
        _inputs.Player.Aspire.canceled += Aspire_canceled;
        _inputs.Player.ResetRotation.performed += ResetRotation_performed;
    }

    private void ResetRotation_performed(InputAction.CallbackContext obj)
    {
        if (!_player.LeafBlower.IsObjectAttached) return;

        _player.LeafBlower.ObjectAttached.ResetObjectRotation();
    }

    private void Aspire_performed(InputAction.CallbackContext obj)
    {
        if(_player.LeafBlower.IsObjectAttached)
        {
            _aspiredPerformed = true;
        }
    }
    private void Aspire_canceled(InputAction.CallbackContext obj)
    {
        _aspiredPerformed = false;
    }

    public void SetIsAspiring(bool b) => _aspiredPerformed = b;


    private bool _aspiredPerformed;

    public bool IsBlowing() => _inputs.Player.Blow.IsPressed();
    public bool IsAspiring() => _inputs.Player.Aspire.IsPressed();

    public bool IsAspirePressed() => _aspiredPerformed;

    private void Sprint_performed(InputAction.CallbackContext context)
    {
        if (!_player.jordanUnlocked) return;
        if (_player.Movement.isHovering) return;

        isSprinting = !isSprinting;

        if(isSprinting)
        {
            _player.Stamina.StartConsumingStamina();
        }
        else
        {
            _player.Stamina.StopConsumingStamina();
        }
    }

    public Vector2 GetPlayerMovement() => _inputs.Player.Move.ReadValue<Vector2>();
    public Vector2 GetPlayerAim() => _inputs.Player.LookAt.ReadValue<Vector2>();

    private void Interact_performed(InputAction.CallbackContext context)
    {
        if (_player.isTalking) return;

        _player.Interactable.InteractPerformed();
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsPaused) return;

        _jumpPressTime = Time.time;
        _jumpHeld = true;
        _jumpHoldStartTime = Time.time;

        if (Time.time - _player.Movement.lastGroundedTime <= _jumpBufferTime && Time.time - _jumpPressTime <= _jumpBufferTime)
        {
            _player.Movement.Jump();
            _player.Movement.isJumping = true;
        }

    }
    private void Jump_canceled(InputAction.CallbackContext context)
    {
        _jumpHeld = false;
        _player.Movement.isJumping = false;
    }

    private void Pause_performed(InputAction.CallbackContext context)
    {
        if(GameManager.Instance.IsPaused) return;
        GameManager.Instance.UpdateState(Enums.GameState.PauseMenu);
    }

    public bool ShouldHover()
    {
        if (!_player.hoverUnlocked) return false;

        return _jumpHeld && (Time.time - _jumpHoldStartTime >= _hoverActivationTime);
    }

    private void OnDestroy()
    {
        if (_inputs == null) return;

        _inputs.Player.Interact.performed -= Interact_performed;
        _inputs.Player.Pause.performed -= Pause_performed;
        _inputs.Player.Jump.performed -= Jump_performed;
        _inputs.Player.Disable();
    }
}
