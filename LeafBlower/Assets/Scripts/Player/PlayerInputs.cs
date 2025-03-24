using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerInputs : MonoBehaviour
{
    private PlayerInputsActions _inputs;
    private PlayerController _controller;

    private bool _isSprinting;

    public bool IsSprinting => _isSprinting;

    private float _jumpPressTime = -1f;
    [SerializeField] private float _jumpBufferTime = 0.125f;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _inputs = new PlayerInputsActions();
        _inputs.Player.Enable();

        _inputs.Player.Interact.performed += Interact_performed;
        _inputs.Player.Sprint.performed += Sprint_performed;
        _inputs.Player.Pause.performed += Pause_performed;
        _inputs.Player.Jump.performed += Jump_performed;
        _inputs.Player.Jump.canceled += Jump_canceled;
        _inputs.Player.Aspire.performed += Aspire_performed;
        _inputs.Player.Aspire.canceled += Aspire_canceled;
    }
    private void Aspire_performed(InputAction.CallbackContext obj)
    {
        if(_controller.LeafBlower.ObjectAttached)
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
        _isSprinting = !_isSprinting;
    }

    public Vector2 GetPlayerMovement() => _inputs.Player.Move.ReadValue<Vector2>();
    public Vector2 GetPlayerAim() => _inputs.Player.LookAt.ReadValue<Vector2>();

    private void Interact_performed(InputAction.CallbackContext context)
    {
        if (_controller.isTalking) return;

        _controller.Interactable.InteractPerformed();
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsPaused) return;

        _jumpPressTime = Time.time;

        if (Time.time - _controller.Movement.lastGroundedTime <= _jumpBufferTime && Time.time - _jumpPressTime <= _jumpBufferTime)
        {
            _controller.Movement.Jump();
            _controller.Movement.isJumping = true;
        }

    }
    private void Jump_canceled(InputAction.CallbackContext context)
    {
        _controller.Movement.isJumping = false;
    }

    private void Pause_performed(InputAction.CallbackContext context)
    {
        GameManager.Instance.UpdateState(Enums.GameState.PauseMenu);
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
