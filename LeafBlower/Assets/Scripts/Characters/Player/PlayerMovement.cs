using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public List<MovementEntry> movements;

    private PlayerController _player;

    [Header("Falling Stats:")]
    [SerializeField] private float _velocityToStartFallAnimation = 0.5f;
    private Vector3 _targetPosition;

    [Header("Slope:")]
    [SerializeField] private float _offsetPlayerSlopesThreshold = 0.1f;
    [SerializeField] private float _groundSnapThreshold = 0.5f;
    public RaycastHit slopeHit;

    [Header("Movement:")]
    public float rotationSpeed = 15f;
    public float rotationAirSpeed = 6f;

    public bool isJumping = false;
    public bool isHovering = false;

    public bool isSprinting;

    private MovementStateHandler _stateHandler;
    private CustomGravityHandler _gravityHandler;
    private Vector3 _moveDirection;
    public Vector3 MoveDirection { get => _moveDirection; set { _moveDirection = value; } }

    private float _moveSpeed;
    public float MoveSpeed { get => _moveSpeed; set { _moveSpeed = value; } }

    [SerializeField] private Transform _lookAt;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _stateHandler = GetComponent<MovementStateHandler>();
        _gravityHandler = GetComponent<CustomGravityHandler>();
    }

    public void HandleAllMovement()
    {
        _player.CheckCollisions.UpdateTerrainSlopeAngle();

        _stateHandler.HandleState();

        if (_player.CheckCollisions.IsGrounded)
        {
            if (_player.CurrentCharacterState == Enums.CharacterState.Idle) return;
            HandleGroundBehavior();
        }
        else
        {
            HandleAirBehavior();
        }
    }
    private void HandleAirBehavior()
    {
        MakeMovement(Enums.Movements.AirMovement, GetAirDirectionToMove());
        HandleRotation(rotationSpeed);

        if (isHovering)
        {
            Hover();
        }
        else
        {
            _gravityHandler.ApplyAdditiveGravity(_player.Rigidbody);
        }
        if (!_player.isInteracting && Mathf.Abs(_player.Rigidbody.velocity.y) > _velocityToStartFallAnimation)
        {
            _player.Animations.PlayTargetAnimation(Constants.ANIM_FALLING, true);
        }
    }

    private void HandleGroundBehavior()
    {
        ClampSpeed(_moveSpeed);
        MakeMovement(Enums.Movements.GroundMovement, GetTargetVelocity());
        HandleRotation(rotationSpeed);

        if (!isJumping)
        {
            // -- Anchors Player to the Ground if is not falling -- //&& _player.Inputs.IsMovingJoystick()
            AnchorToGround();

        }
        if (isHovering)
        {
            isHovering = false;
            _player.BlowerController.isHovering = isHovering;
        }
    }

    private void HandleRotation(float speed)
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = GetDirectionNormalized();
        //targetDirection = Camera.main.transform.forward;
        targetDirection.y = 0;
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            _player.Rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime));
        }
    }

    private void AnchorToGround()
    {
        float distanceToGround = transform.position.y - slopeHit.point.y;

        if (distanceToGround <= _groundSnapThreshold)
        {
            _targetPosition = transform.position;
            _targetPosition.y = slopeHit.point.y + _offsetPlayerSlopesThreshold;

            if (_player.isInteracting || _player.Inputs.GetMoveDirection().magnitude > 0)
            {
                transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime / 0.1f);
            }
            //else
            //{
            //    transform.position = _targetPosition;
            //}

            // Stop downward momentum after anchoring
            _player.Rigidbody.velocity = new Vector3(_player.Rigidbody.velocity.x, 0, _player.Rigidbody.velocity.z);
        }
    }
    public void Jump()
    {
        if (isJumping || !_player.CheckCollisions.IsGrounded) return;

        _player.Animations.HandleJumpAnimations();

        MakeMovement(Enums.Movements.Jump, _player.Stats.JumpForce);
    }

    public void Dash() 
    {
        if (_player.CheckCollisions.IsGrounded || _player.BlowerController.Aspirer.IsObjectAttached) return;
        MakeMovement(Enums.Movements.Dash, _player.Stats.DashForce);
    }

    public void ToggleHover()
    {
        if (_player.CheckCollisions.IsGrounded || !_player.BlowerController.CanUseLeafBlower()) return;
        isHovering = !isHovering;
        _player.BlowerController.isHovering = isHovering;
        if(isHovering)
        {
            _player.Rigidbody.velocity = new Vector3(_player.Rigidbody.velocity.x, 0, _player.Rigidbody.velocity.z);
            _player.BlowerController.Handler.ConsumeStaminaOverTime();
        }
        else
        {
            _player.BlowerController.Handler.ReEnableRecoverStamina();
        }
    }
    public void Hover()
    {
        if(!_player.BlowerController.Handler.HasStamina())
        {
            isHovering = false;
            return;
        }
        if (isHovering)
        {
            MakeMovement(Enums.Movements.Hover, _player.Stats.HoverForce); 
        }
    }
    private void ClampSpeed(float speedToClamp)
    {
        Vector3 horizontalVelocity = new Vector3(_player.Rigidbody.velocity.x, 0, _player.Rigidbody.velocity.z);

        if (horizontalVelocity.magnitude > speedToClamp)
        {
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, speedToClamp);
            _player.Rigidbody.velocity = new Vector3(horizontalVelocity.x, _player.Rigidbody.velocity.y, horizontalVelocity.z);
        }
    }
    public void DisableMovement()
    {
        if (_player.CurrentCharacterState == Enums.CharacterState.Idle) return;

        _player.ChangeCharacterState(Enums.CharacterState.Idle);
        _player.Movement.isSprinting = false;
    }
    private Vector3 GetTargetVelocity()
    {
        _moveDirection = GetDirectionNormalized();
        _moveDirection.y = 0;

        Vector3 targetVelocity = Vector3.zero;

        if (_player.CheckCollisions.OnSlope())
        {
            Vector3 slopeDirection = GetSlopeMoveDirection(_moveDirection);
           // targetVelocity = slopeDirection * _player.Stats.Acceleration;
            if (_player.CheckCollisions.IsOnMaxSlopeAngle())
            {
                // Project the movement direction onto the slope to avoid moving upwards
                targetVelocity = Vector3.ProjectOnPlane(slopeDirection, slopeHit.normal) * _moveSpeed;

                _player.Movement.isSprinting = false;
                // Apply additional downward gravity to make the player slide
               // ApplyAdditiveGravity(_gravity * 50);
            }
            else
            {
                targetVelocity = slopeDirection * _moveSpeed;
            }
        }
        else
        {
            _moveDirection = _player.CheckCollisions.IsWall(GetDirectionNormalized());
            targetVelocity = _moveDirection * _moveSpeed;
        }

        if (_moveDirection.magnitude == 0 && !isJumping && !_player.Inputs.IsMovingJoystick())
        {
            _player.Rigidbody.velocity = Vector3.zero;
            return Vector3.zero;
        }

        return targetVelocity;
    }
    private Vector3 GetAirDirectionToMove()
    {
        _moveDirection = _player.CheckCollisions.IsWall(GetDirectionNormalized());
        _moveDirection.y = 0;
        return _moveDirection * _moveSpeed;
    }
    private Vector3 GetDirectionNormalized() => Utils.GetCameraForwardNormalized(Camera.main) * _player.Inputs.GetMoveDirection().y + Utils.GetCameraRightNormalized(Camera.main) * _player.Inputs.GetMoveDirection().x;
    private Vector3 GetSlopeMoveDirection(Vector3 _direction) => Vector3.ProjectOnPlane(_direction, slopeHit.normal).normalized;
    //private Vector3 GetForwardSlopeDirection() => Vector3.ProjectOnPlane(transform.forward, slopeHit.normal).normalized;

       
    public void MakeMovement(Enums.Movements move, float force)
    {
        foreach (var movement in movements)
        {
            if(movement.type == move)
            {
                movement.movement.ExecuteMovement(_player.Rigidbody, force);
            }
        }
    }

    public void MakeMovement(Enums.Movements move, Vector3 forceDir)
    {
        foreach (var movement in movements)
        {
            if (movement.type == move)
            {
                movement.movement.ExecuteMovement(_player.Rigidbody, forceDir);
            }
        }
    }
}
