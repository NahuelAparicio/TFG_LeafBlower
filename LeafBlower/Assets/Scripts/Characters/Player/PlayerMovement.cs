using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private PlayerController _player;
    private Vector3 _moveDirection;

    [Header("Falling Stats:")]
    [SerializeField] private float _gravity = -20f;

    [SerializeField] private float _velocityToStartFallAnimation = 0.5f;
    private Vector3 _targetPosition;

    [Header("Slope:")]
    [SerializeField] private float _offsetPlayerSlopesThreshold = 0.1f;
    [SerializeField] private float _groundSnapThreshold = 0.5f;
    public RaycastHit slopeHit;

    [Header("Movement:")]
    public float rotationSpeed = 15f;
    public float rotationAirSpeed = 6f;
    public float airAccelerationExtra;

    public bool isJumping = false;
    public int groundDrag = 3;
    public int airDrag = 0;

    public bool isSprinting;

    public float moveSpeed;
    public Enums.CharacterMoveState moveState;

    // -- Momentum Stats -- //
    private float _desiredVelocity;
    private float _lastDesiredVelocity;
    public bool keepMomentum;
    public float speedChangeFactor;
    public Enums.CharacterMoveState lastState;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }          

    public void HandleAllMovement()
    {
        _player.CheckCollisions.UpdateTerrainSlopeAngle();

        HandleFallingAndLanding();

        StateHandler();

        if(!_player.CheckCollisions.IsGrounded)
        {
            HandleAirMovement();
        }

        if(_player.isInteracting || isJumping ||!_player.CheckCollisions.IsGrounded)
        {
            return;
        }
        HandleMovement();
    }

    private void HandleAirMovement()
    {
        SetRigidbodyDrag(airDrag);
        //Checks if there is a wall in players directions if it's ProjectOnPlane movement feel
        _moveDirection = _player.CheckCollisions.IsWall(GetDirectionNormalized());
        _moveDirection.y = 0;

        Vector3 directionMove = _moveDirection * (moveSpeed + airAccelerationExtra);

       // ClampSpeed(moveSpeed);

        _player.Rigidbody.AddForce(directionMove, ForceMode.Acceleration);

        HandleRotation(rotationAirSpeed);
    }
    private void HandleRotation(float speed)
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = GetDirectionNormalized();
        targetDirection.y = 0;
        if(targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            _player.Rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime));
        }
    }

    private void HandleMovement()
    {
        SetRigidbodyDrag(groundDrag);

        ClampSpeed(moveSpeed);

        _player.Rigidbody.AddForce(GetTargetVelocity(), ForceMode.VelocityChange);

        HandleRotation(rotationSpeed);
    }
    private void HandleFallingAndLanding()
    {
        if (_player.CheckCollisions.IsGrounded)
        {
            if(_player.CheckCollisions.OnSlope())
            {
                _player.Rigidbody.useGravity = _player.CheckCollisions.IsOnMaxSlopeAngle();
            }            
            else
            {
                _player.Rigidbody.useGravity = true;
            }
        }
        else
        {
            if (!_player.Rigidbody.useGravity)
            {
                _player.Rigidbody.useGravity = true;
            }
            ApplyAdditiveGravity(_gravity);
            if (!_player.isInteracting && Mathf.Abs(_player.Rigidbody.velocity.y) > _velocityToStartFallAnimation)
            {
                _player.Animations.PlayTargetAnimation(Constants.ANIM_FALLING, true);
            }
        }
        // -- Anchors Player to the Ground if is not falling -- //&& _player.Inputs.IsMovingJoystick()
        if (_player.CheckCollisions.IsGrounded && !isJumping)
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
                else
                {
                    transform.position = _targetPosition;
                }

                // Stop downward momentum after anchoring
                _player.Rigidbody.velocity = new Vector3(_player.Rigidbody.velocity.x, 0, _player.Rigidbody.velocity.z);
            }
        }
    }
    public void HandleJumping()
    {
        if(!_player.CheckCollisions.IsGrounded)
        {
            return;
        }
        _player.Animations.HandleJumpAnimations();
        _player.Rigidbody.velocity = new Vector3(_player.Rigidbody.velocity.x, 0, _player.Rigidbody.velocity.z);
        _player.Rigidbody.AddForce(transform.up * _player.Stats.JumpForce, ForceMode.Impulse);
    }
    public void HandleDash()
    {

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
        _player.ChangeCharacterState(Enums.CharacterState.Idle);
        _player.Movement.isSprinting = false;
        //_moveDirection = Vector3.zero;
        //_player.Rigidbody.velocity = Vector3.zero;
        //_player.Rigidbody.angularVelocity = Vector3.zero;
    }
    private void SetRigidbodyDrag(float _drag)
    {
        if (_player.Rigidbody.drag == _drag) return;

        _player.Rigidbody.drag = _drag;
    }
    private void ApplyAdditiveGravity(float g) => _player.Rigidbody.AddForce(Vector3.up * g, ForceMode.Acceleration);

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
                targetVelocity = Vector3.ProjectOnPlane(slopeDirection, slopeHit.normal) * moveSpeed;

                _player.Movement.isSprinting = false;
                // Apply additional downward gravity to make the player slide
                ApplyAdditiveGravity(_gravity * 50);
            }
            else
            {
                targetVelocity = slopeDirection * moveSpeed;
            }
        }
        else
        {
            _moveDirection = _player.CheckCollisions.IsWall(GetDirectionNormalized());
            targetVelocity = _moveDirection * moveSpeed;
        }

        if (_moveDirection.magnitude == 0 && !isJumping && !_player.Inputs.IsMovingJoystick())
        {
            _player.Rigidbody.velocity = Vector3.zero;
            return Vector3.zero;
        }

        return targetVelocity;
    }
    private Vector3 GetDirectionNormalized() => Utils.GetCameraForwardNormalized(Camera.main) * _player.Inputs.GetMoveDirection().y + Utils.GetCameraRightNormalized(Camera.main) * _player.Inputs.GetMoveDirection().x;
    private Vector3 GetSlopeMoveDirection(Vector3 _direction) => Vector3.ProjectOnPlane(_direction, slopeHit.normal).normalized;
    //private Vector3 GetForwardSlopeDirection() => Vector3.ProjectOnPlane(transform.forward, slopeHit.normal).normalized;

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float t = 0;
        float difference = Mathf.Abs(_desiredVelocity - moveSpeed);
        float startValue;
        if (_player.Rigidbody.velocity.magnitude > _desiredVelocity)
        {
            startValue = moveSpeed;
        }
        else
        {
            startValue = _player.Rigidbody.velocity.magnitude;
        }

        float boostFactor = speedChangeFactor;

        while (t < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, _desiredVelocity, t / difference);
            t += Time.deltaTime * boostFactor;

            yield return null;
        }

        moveSpeed = _desiredVelocity;
        if(moveSpeed == 0)
        {
            _moveDirection = Vector3.zero;
            _player.Rigidbody.velocity = Vector3.zero;
            _player.Rigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void StateHandler()
    {
        if(_player.CheckCollisions.IsGrounded && isSprinting && _player.CurrentCharacterState != Enums.CharacterState.Idle)
        {
            ChangeMoveState(Enums.CharacterMoveState.Running);
            _desiredVelocity = _player.Stats.RunSpeed;
        }
        else if(_player.CheckCollisions.IsGrounded && _player.CurrentCharacterState == Enums.CharacterState.Idle)
        {
            ChangeMoveState(Enums.CharacterMoveState.None);
            _desiredVelocity = 0;
        }
        else if(_player.CheckCollisions.IsGrounded)
        {
            ChangeMoveState(Enums.CharacterMoveState.Walking);
            _desiredVelocity = _player.Stats.WalkSpeed;
        }
        else
        {
            ChangeMoveState(Enums.CharacterMoveState.Air);
            if(lastState == Enums.CharacterMoveState.Walking)
            {
                _desiredVelocity = _player.Stats.WalkSpeed;
            }
            else if(lastState == Enums.CharacterMoveState.Running)
            {
                _desiredVelocity = _player.Stats.RunSpeed;
            }
        }

        bool desiredMoveSpeedHasChanged = _desiredVelocity != _lastDesiredVelocity;

        if (desiredMoveSpeedHasChanged)
        {
            //if (_desiredVelocity > _lastDesiredVelocity)  
            //{
            //    keepMomentum = true;  
            //}
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());

            //if (keepMomentum)
            //{
            //    StopAllCoroutines();
            //    StartCoroutine(SmoothlyLerpMoveSpeed());
            //}
            //else
            //{
            //    StopAllCoroutines();
            //    moveSpeed = _desiredVelocity;
            //}
        }
        _lastDesiredVelocity = _desiredVelocity;
    }
    private void ChangeMoveState(Enums.CharacterMoveState newState)
    {
        if (moveState == newState)
        {
            return;
        }

        lastState = moveState;

        moveState = newState;
    }
}
