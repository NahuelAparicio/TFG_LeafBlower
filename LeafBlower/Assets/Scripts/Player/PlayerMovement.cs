using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _player;
    private Vector3 _moveDirection;

    public float rotationSpeed = 15f;

    // -- Momentum Stats -- //
    private float _desiredVelocity;
    private float _lastDesiredVelocity;
    public bool keepMomentum;

    [Header("Falling Stats:")]
    public float inAirTime;
    public float maxDistance;
    public LayerMask groundLayer;
    private Vector3 _targetPosition;

    [Header("Slope Stats:")]
    public float maxSlopeAngle = 60f;
    private RaycastHit _slopeHit;

    [Header("Movement flags")]
    private bool _isJumping = false;
    private bool _isSprinting = false; // ?????

    public float gravity = -9.8f;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }          

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        //if !isgrounded
        if(!_player.CheckGround.IsGrounded)
        {
            HandleAirMovement();
        }

        if(_player.isInteracting || _isJumping ||!_player.CheckGround.IsGrounded)
        {
            return;
        }
        HandleMovement();
    }

    private void HandleAirMovement()
    {
        _moveDirection = GetDirectionNormalized();
        Vector3 directionMove = _moveDirection * _player.Stats.Speed;



        //Vector3 velocityHorizontal = new Vector3(_player.Rigidbody.velocity.x, 0f, _player.Rigidbody.velocity.z);

        //if (velocityHorizontal.magnitude > _player.Stats.MaxAirSpeed)
        //{
        //    velocityHorizontal = velocityHorizontal.normalized * _player.Stats.MaxAirSpeed;
        //    newVelocity = new Vector3(limitedVelocity.x, _player.Rigidbody.velocity.y, limitedVelocity.z);
        //}
        _player.Rigidbody.velocity = new Vector3(directionMove.x, _player.Rigidbody.velocity.y, directionMove.z);
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
        _moveDirection = GetDirectionNormalized();
        _moveDirection.y = 0;

        Vector3 targetVelocity = _moveDirection * _player.Stats.Speed;

        if(OnSlope())
        {
            targetVelocity = GetSlopeMoveDirection(_moveDirection) * _player.Stats.Speed;
        }

        _player.Rigidbody.MovePosition(_player.Rigidbody.position + targetVelocity * Time.deltaTime); // Interpolates Between current Velocity and target Velocity

        HandleRotation(rotationSpeed);
    }

    private void HandleFallingAndLanding()
    {
        if(_player.CheckGround.IsGrounded)
        {
            _player.Rigidbody.useGravity = !OnSlope();
        }
        else
        {
            inAirTime += Time.deltaTime;
            ApplyGravity();
            //If it falls not from jump set fall animation
            if(!_player.isInteracting)
            {
                //_player.anim.PlayTargetAnimation(ANIMATION_FALL; true)
            }
        }
        //Anchors player to the ground
        if(_player.CheckGround.IsGrounded && !_isJumping)
        {
            _targetPosition = transform.position;
            _targetPosition.y = _player.CheckGround.GetGroundHitPoint().y; // + playerheight
            if(_player.isInteracting || _player.Inputs.GetMoveDirection().magnitude > 0)
            {
                _player.Rigidbody.MovePosition(Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime / 0.1f));
            }
            else
            {
                _player.Rigidbody.MovePosition(_targetPosition);
            }
        }
    }

    private void ApplyGravity()
    {
        Vector3 gravityVector = Vector3.up * gravity * Time.deltaTime;
        _player.Rigidbody.velocity += Vector3.up * gravity * Time.deltaTime;
        //_player.Rigidbody.AddForce(gravityVector, ForceMode.Acceleration);
    }

    public void HandleJumping()
    {
        if(!_player.CheckGround.IsGrounded)
        {
            return;
        }
        //player anim setboll true
        //Playtarget anim
        _player.Rigidbody.velocity = new Vector3(_player.Rigidbody.velocity.x, 0, _player.Rigidbody.velocity.z);
        _player.Rigidbody.AddForce(transform.up * _player.Stats.JumpForce * 10, ForceMode.Impulse);
    }

    public void HandleDash()
    {

    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, -Vector3.up, out _slopeHit, maxDistance, groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }


    private Vector3 GetDirectionNormalized() => Utils.GetCameraForwardNormalized(Camera.main) * _player.Inputs.GetMoveDirection().y + Utils.GetCameraRightNormalized(Camera.main) * _player.Inputs.GetMoveDirection().x;
    private Vector3 GetSlopeMoveDirection(Vector3 _direction) => Vector3.ProjectOnPlane(_direction, _slopeHit.normal).normalized;
    private Vector3 GetForwardSlopeDirection() => Vector3.ProjectOnPlane(transform.forward, _slopeHit.normal).normalized;
}
