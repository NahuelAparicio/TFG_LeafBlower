using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonInputs : MonoBehaviour
{
    private PlayerInputsActions _inputs;
    private FirstPersonController _controller;

    private bool _isSprinting;

    public bool IsSprinting => _isSprinting;

    private void Awake()
    {
        _controller = GetComponent<FirstPersonController>();
        _inputs = new PlayerInputsActions();
        _inputs.Player.Enable();

        _inputs.Player.Interact.performed += Interact_performed;
        _inputs.Player.Sprint.performed += Sprint_performed;
        _inputs.Player.Pause.performed += Pause_performed;
        _inputs.Player.Jump.performed += Jump_performed;
    }

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
        _controller.Movement.Jump();
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
