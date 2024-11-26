using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private PlayerController _player;
    private PlayerInputsActions _actions;
    private float lastClickTimeL2 = 0f;
    private float lastClickTimeR2 = 0f;
    private float doubleClickThreshold = 0.3f;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _actions = new PlayerInputsActions();
        _actions.Player.Enable();
        _actions.Player.Move.performed += Move_performed;
        _actions.Player.Move.canceled += Move_canceled;
        _actions.Player.Interact.performed += Interact_performed;
        _actions.Player.Jump.performed += Jump_performed;
        _actions.Player.Pause.performed += Pause_performed;
        _actions.Player.Sprint.performed += Sprint_performed;
        _actions.Player.Sprint.canceled += Sprint_canceled;
        _actions.Player.Dash.performed += Dash_performed;
        _actions.Player.Hover.performed += Hover_performed;

    }

    public Vector2 GetMoveDirection() => _actions.Player.Move.ReadValue<Vector2>(); // Left Stick -- WASD

    public Vector2 GetAimMoveDirection() => _actions.Player.BlowerMove.ReadValue<Vector2>(); // Right Stick -- Mouse (?)

    public bool IsMovingJoystick() => GetMoveDirection().magnitude > 0.125f;

    private void Sprint_performed(InputAction.CallbackContext context)
    {
        _player.Movement.isSprinting = !_player.Movement.isSprinting;
    }

    private void Sprint_canceled(InputAction.CallbackContext context)
    {
        //_player.Movement.isSprinting = false;
    }

    private void Move_performed(InputAction.CallbackContext context)
    {
        if(!_player.isStuned)
            _player.ChangeCharacterState(Enums.CharacterState.Moving);
    }
    private void Move_canceled(InputAction.CallbackContext context)
    {
        if (!_player.isStuned)
        {
            _player.Movement.DisableMovement();
        }
    }
    private void Interact_performed(InputAction.CallbackContext context)
    {
        if (!_player.Interactable.canInteract && _player.IsTalking) return;

        _player.Interactable.InteractPerformed();
    }
    private void Jump_performed(InputAction.CallbackContext context)
    {
        _player.Movement.Jump();
    }

    private void Hover_performed(InputAction.CallbackContext context)
    {
        float _currentTime = Time.time;

        if (_currentTime - lastClickTimeR2 >= doubleClickThreshold)
        {
            lastClickTimeR2 = _currentTime;
        }
        else
        {
            _player.Movement.ToggleHover();

            lastClickTimeR2 = _currentTime;
        }
    }

    private void Dash_performed(InputAction.CallbackContext context)
    {
        float _currentTime = Time.time;

        if (_currentTime - lastClickTimeL2 >= doubleClickThreshold)
        {
            lastClickTimeL2 = _currentTime;
        }
        else
        {
            _player.Movement.Dash();

            lastClickTimeL2 = _currentTime;
        }
    }

    private void Pause_performed(InputAction.CallbackContext context) 
    {
        GameManager.Instance.PauseGameHandler();
    }


    private void OnDestroy()
    {
        _actions.Player.Move.performed -= Move_performed;
        _actions.Player.Move.canceled -= Move_canceled;
        _actions.Player.Interact.performed -= Interact_performed;
        _actions.Player.Jump.performed -= Jump_performed;
        _actions.Player.Pause.performed -= Pause_performed;
        _actions.Player.Sprint.performed -= Sprint_performed;
        _actions.Player.Sprint.canceled -= Sprint_canceled;
        _actions.Player.Dash.performed -= Dash_performed;
        _actions.Player.Hover.performed -= Hover_performed;
        _actions.Player.Disable();
    }

}
