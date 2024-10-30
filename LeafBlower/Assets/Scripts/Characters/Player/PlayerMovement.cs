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
    public bool isJumping = false;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }          

    public void HandleAllMovement()
    {
        _player.CheckCollisions.UpdateTerrainSlopeAngle();

        HandleFallingAndLanding();

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
        SetRigidbodyDrag(0);
        //Checks if there is a wall in players directions if it's ProjectOnPlane movement feel
        _moveDirection = _player.CheckCollisions.IsWall(GetDirectionNormalized());
        _moveDirection.y = 0;
        Vector3 directionMove = _moveDirection * _player.Stats.AirAcceleration;
        ClampSpeed(_player.Stats.MaxSpeed);
        _player.Rigidbody.AddForce(directionMove, ForceMode.Acceleration);

        HandleRotation(rotationSpeed);
    }
    private void HandleRotation(float speed)
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = GetDirectionNormalized();
        targetDirection.y = 0;
        //if (targetDirection != Vector3.zero)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //    Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        //    transform.rotation = playerRotation;
        //}
        if(targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            _player.Rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime));
        }
    }
    private void HandleMovement()
    {
        SetRigidbodyDrag(3);

        ClampSpeed(_player.Stats.MaxSpeed);
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
        if (_player.CheckCollisions.IsGrounded && !isJumping )
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
        _moveDirection = Vector3.zero;
        _player.Rigidbody.velocity = Vector3.zero;
        _player.Rigidbody.angularVelocity = Vector3.zero;
    }
    private void SetRigidbodyDrag(float _drag)
    {
        if(_player.Rigidbody.drag != _drag)
        {
            _player.Rigidbody.drag = _drag;
        }
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
                targetVelocity = Vector3.ProjectOnPlane(slopeDirection, slopeHit.normal) * _player.Stats.Acceleration;

                // Apply additional downward gravity to make the player slide
                ApplyAdditiveGravity(_gravity * 50);
            }
            else
            {
                targetVelocity = slopeDirection * _player.Stats.Acceleration;
            }
        }
        else
        {
            _moveDirection = _player.CheckCollisions.IsWall(GetDirectionNormalized());
            targetVelocity = _moveDirection * _player.Stats.Acceleration;
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
    private Vector3 GetForwardSlopeDirection() => Vector3.ProjectOnPlane(transform.forward, slopeHit.normal).normalized;

}
