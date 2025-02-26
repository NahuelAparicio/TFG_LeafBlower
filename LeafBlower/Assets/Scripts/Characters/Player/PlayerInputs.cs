using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private PlayerController _player;
    private PlayerInputsActions _actions;
    [SerializeField] private float _maxHoldTime = 0.25f;
    private float _currentHoldTime;
    private float _jumpPressTime = -1f;
    [SerializeField] private float _hoverThreshold = 0.75f;

    [SerializeField] private float _jumpBufferTime = 0.125f;
    private float _currentTime = 0f;
    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _actions = new PlayerInputsActions();
        _actions.Player.Enable();
        _actions.Player.Move.performed += Move_performed;
        _actions.Player.Move.canceled += Move_canceled;
        _actions.Player.Interact.performed += Interact_performed;
        _actions.Player.Pause.performed += Pause_performed;
        _actions.Player.Sprint.performed += Sprint_performed;
        _actions.Player.Sprint.canceled += Sprint_canceled;
        _actions.Player.Jump.performed += Jump_performed;
        _actions.Player.Jump.canceled += Jump_canceled;
    }
    private void Update()
    {
        if(_actions.Player.Jump.IsPressed() && _player.CheckCollisions.IsGrounded)
        {
            _currentTime += Time.deltaTime;
            if(_currentTime >= _maxHoldTime)
            {
                _player.Movement.Jump(1);
                _currentTime = 0f;
            }
        }
    }
    public Vector2 GetMoveDirection() => _actions.Player.Move.ReadValue<Vector2>(); // Left Stick -- WASD
    public Vector2 GetAimMoveDirection() => _actions.Player.BlowerMove.ReadValue<Vector2>(); // Right Stick -- Mouse (?)

    public bool IsMovingLeftJoystick() => GetMoveDirection().magnitude > 0.05f;
    public bool IsMovingRightJoystick() => GetAimMoveDirection().magnitude > 0.05f;

    public bool IsHoveringInputPressed() => (_jumpPressTime > 0 && (Time.time - _jumpPressTime) >= _hoverThreshold);

    private void Sprint_performed(InputAction.CallbackContext context)
    {
        if (!_player.CanMovePlayer()) return;
        _player.Movement.isSprinting = !_player.Movement.isSprinting;
    }

    private void Sprint_canceled(InputAction.CallbackContext context)
    {
    }

    private void Move_performed(InputAction.CallbackContext context)
    {
        if (!_player.CanMovePlayer()) return;

        _player.ChangeCharacterState(Enums.CharacterState.Moving);
    }
    private void Move_canceled(InputAction.CallbackContext context)
    {
        if (!_player.CanMovePlayer()) return;

        _player.Movement.DisableMovement();
    }
    private void Interact_performed(InputAction.CallbackContext context)
    {
        if (!_player.Interactable.canInteract && _player.IsTalking) return;
        
        _player.Interactable.InteractPerformed();
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        if (!_player.CanMovePlayer()) return;

        _jumpPressTime = Time.time;
        _currentHoldTime = 0f;
        _player.Movement.onStartHovering = true;

        if (_player.Movement.isJumping || _player.CheckCollisions.IsGrounded) return;        

        if (Time.time - _player.Movement.lastGroundedTime <= _jumpBufferTime && Time.time - _jumpPressTime <= _jumpBufferTime)
        {
            _player.Movement.Jump(1);
        }
    }

    private void Jump_canceled(InputAction.CallbackContext context)
    {
        if(!_player.Movement.isJumping && _player.CheckCollisions.IsGrounded)
        {
            _currentHoldTime = Mathf.Min(Time.time - _jumpPressTime, _maxHoldTime);
            _player.Movement.Jump(_currentHoldTime / _maxHoldTime);
        }

        _player.Movement.onStartHovering = false;
        _player.Movement.OnUpdateHovering();

        _jumpPressTime = -1f;
    } 

    private void Pause_performed(InputAction.CallbackContext context) 
    {
        GameManager.Instance.UpdateState(Enums.GameState.PauseMenu);
    }


    private void OnDestroy()
    {
        _actions.Player.Move.performed -= Move_performed;
        _actions.Player.Move.canceled -= Move_canceled;
        _actions.Player.Interact.performed -= Interact_performed;
        _actions.Player.Pause.performed -= Pause_performed;
        _actions.Player.Sprint.performed -= Sprint_performed;
        _actions.Player.Sprint.canceled -= Sprint_canceled;
        _actions.Player.Disable();
    }

}
