using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _player;
    private Vector3 _moveDirection;
    public Vector3 MoveDirection => _moveDirection;
    public float rotationSpeed = 15f;

    [Header("Falling Stats:")]
    public float gravity = -9.8f;
    public float maxDistanceSlopeRay;
    public float velocityToStartFallAnimation;
    public LayerMask groundLayer;
    private Vector3 _targetPosition;

    [Header("Slope Stats:")]
    public float maxSlopeAngle = 60f;
    private RaycastHit _slopeHit;

    [Header("Movement flags")]
    public bool isJumping = false;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }          

    public void HandleAllMovement()
    {
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
        //Checks if there is a wall in players directions if it's ProjectOnPlane movement feel
        _moveDirection = _player.CheckCollisions.IsWall(GetDirectionNormalized());
        _moveDirection.y = 0;
        Vector3 directionMove = _moveDirection * _player.Stats.AirAcceleration;
        ClampSpeed(_player.Stats.MaxAirSpeed);
        _player.Rigidbody.AddForce(directionMove, ForceMode.VelocityChange);

        HandleRotation(_player.Stats.MaxAirSpeed);
    }

    private void HandleRotation(float speed)
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = GetDirectionNormalized();
        targetDirection.y = 0;
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = playerRotation;
        }
    }

    private void HandleMovement()
    {
        _moveDirection = _player.CheckCollisions.IsWall(GetDirectionNormalized());
       // _moveDirection = GetDirectionNormalized();
        _moveDirection.y = 0;

        Vector3 targetVelocity = _moveDirection * _player.Stats.Acceleration;

        if(OnSlope())
        {
            targetVelocity = GetSlopeMoveDirection(_moveDirection) * _player.Stats.Acceleration;

            if (_moveDirection.magnitude == 0 && !isJumping && !_player.Inputs.IsMovingJoystick())
            {
                _player.Rigidbody.velocity = Vector3.zero;
                return;
            }
        }

        ClampSpeed(_player.Stats.MaxSpeed);

        _player.Rigidbody.AddForce(targetVelocity, ForceMode.VelocityChange);

        HandleRotation(rotationSpeed);

    }

    private void HandleFallingAndLanding()
    {

        if (_player.CheckCollisions.IsGrounded)
        {
            _player.Rigidbody.useGravity = !OnSlope();
        }
        else
        {
            if (!_player.Rigidbody.useGravity)
            {
                _player.Rigidbody.useGravity = true;
            }
            ApplyGravity();
            if (!_player.isInteracting && Mathf.Abs(_player.Rigidbody.velocity.y) > velocityToStartFallAnimation)
            {
                _player.Animations.PlayTargetAnimation(Constants.ANIM_FALLING, true);
            }
        }

        // -- Anchors Player to the Ground if is not falling -- //
        if (_player.CheckCollisions.IsGrounded && !isJumping)
        {
            RaycastHit groundHit = _player.CheckCollisions.GetGroundHit(transform.forward * 0.15f);
            float distanceToGround = transform.position.y - groundHit.point.y;
            float groundSnapThreshold = 0.5f; // Set a small threshold to prevent snapping too far

            if (distanceToGround <= groundSnapThreshold)
            {
                _targetPosition = transform.position;
                _targetPosition.y = groundHit.point.y; // Snap to the detected ground height

                if (_player.isInteracting || _player.Inputs.GetMoveDirection().magnitude > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime / 0.01f);
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

    private void ApplyGravity()
    {
        _player.Rigidbody.velocity += Vector3.up * gravity * Time.deltaTime;
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

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, -Vector3.up, out _slopeHit, maxDistanceSlopeRay, groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    // Clamps the speed if you going to fast
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
    private Vector3 GetDirectionNormalized() => Utils.GetCameraForwardNormalized(Camera.main) * _player.Inputs.GetMoveDirection().y + Utils.GetCameraRightNormalized(Camera.main) * _player.Inputs.GetMoveDirection().x;
    private Vector3 GetSlopeMoveDirection(Vector3 _direction) => Vector3.ProjectOnPlane(_direction, _slopeHit.normal).normalized;
    private Vector3 GetForwardSlopeDirection() => Vector3.ProjectOnPlane(transform.forward, _slopeHit.normal).normalized;
}
