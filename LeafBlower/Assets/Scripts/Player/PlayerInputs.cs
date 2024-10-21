using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private PlayerController _player;
    private PlayerInputsActions _actions;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _actions = new PlayerInputsActions();
        _actions.Player.Enable();
        _actions.Player.Interact.performed += Interact_performed;
        _actions.Player.Dash.performed += Dash_performed;
        _actions.Player.Jump.performed += Jump_performed;
        _actions.Player.Pause.performed += Pause_performed;
    }

    public Vector2 GetMoveDirection() => _actions.Player.Move.ReadValue<Vector2>(); // Left Stick -- WASD

    private void Interact_performed(InputAction.CallbackContext context)
    {
        if(_player.Interactable.canInteract)
            _player.Interactable.InteractPerformed();
    }
    private void Jump_performed(InputAction.CallbackContext context)
    {
        _player.Movement.HandleJumping();
    }

    private void Dash_performed(InputAction.CallbackContext context)
    {
        _player.Movement.HandleDash(); // If jump + Blow HandleDash()
    }

    private void Pause_performed(InputAction.CallbackContext context) 
    {
        GameManager.Instance.PauseGameHandler();
    }


    private void OnDestroy()
    {
        _actions.Player.Interact.performed -= Interact_performed;
        _actions.Player.Dash.performed -= Dash_performed;
        _actions.Player.Pause.performed -= Pause_performed;

        _actions.Player.Disable();
    }

}
