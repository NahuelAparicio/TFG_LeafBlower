using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private PlayerController _player;
    private PlayerInputsActions _actions;

    private float _jumpPressTime = -1f;
    [SerializeField] private float _hoverThreshold = 0.75f;

    [SerializeField] private float _jumpBufferTime = 0.125f;

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

    private void OnEnable()
    {
        if (_actions == null)
            _actions = new PlayerInputsActions();

        _actions.Player.Enable();
    }

    private void OnDisable()
    {
        _actions.Player.Disable();
    }

    //public Vector2 GetMoveDirection() => _actions.Player.Move.ReadValue<Vector2>(); // Left Stick -- WASD
    public Vector2 GetMoveDirection() => _moveDir; // Left Stick -- WASD
    public Vector2 GetAimMoveDirection() => _actions.Player.BlowerMove.ReadValue<Vector2>(); // Right Stick -- Mouse (?)
    private Vector2 _moveDir;
    public bool IsMovingLeftJoystick() => GetMoveDirection().magnitude > 0.05f;
    public bool IsMovingRightJoystick() => GetAimMoveDirection().magnitude > 0.05f;

    public bool IsHoveringInputPressed() => (_jumpPressTime > 0 && (Time.time - _jumpPressTime) >= _hoverThreshold);

    private void Sprint_performed(InputAction.CallbackContext context)
    {
        //if (!_player.CanMovePlayer()) return;
        //_player.Movement.isSprinting = !_player.Movement.isSprinting;
    }

    private void Sprint_canceled(InputAction.CallbackContext context)
    {
    }

    private void Move_performed(InputAction.CallbackContext context)
    {
        if (!_player.CanMovePlayer()) return;

        _moveDir = context.ReadValue<Vector2>();
        _player.ChangeCharacterState(Enums.CharacterState.Moving);
    }
    private void Move_canceled(InputAction.CallbackContext context)
    {
        if (!_player.CanMovePlayer()) return;
        _moveDir = context.ReadValue<Vector2>();
        _player.Movement.DisableMovement();
    }



    private void Interact_performed(InputAction.CallbackContext context)
    {
        if (!_player.Interactable.canInteract || _player.IsTalking) return;
        
        _player.Interactable.InteractPerformed();

    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        if (!_player.CanMovePlayer()) return;

        _jumpPressTime = Time.time;

        _player.Movement.onStartHovering = true;

        if (_player.Movement.isJumping) return;        

        if (Time.time - _player.Movement.lastGroundedTime <= _jumpBufferTime && Time.time - _jumpPressTime <= _jumpBufferTime)
        {
            _player.Movement.Jump();
        }
    }

    private void Jump_canceled(InputAction.CallbackContext context)
    {
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
        if (_actions == null) return;

        _actions.Player.Move.performed -= Move_performed;
        _actions.Player.Move.canceled -= Move_canceled;
        _actions.Player.Interact.performed -= Interact_performed;
        _actions.Player.Pause.performed -= Pause_performed;
        _actions.Player.Sprint.performed -= Sprint_performed;
        _actions.Player.Sprint.canceled -= Sprint_canceled;
        _actions.Player.Jump.performed -= Jump_performed;
        _actions.Player.Jump.canceled -= Jump_canceled;

        _actions.Player.Disable();
    }

}
