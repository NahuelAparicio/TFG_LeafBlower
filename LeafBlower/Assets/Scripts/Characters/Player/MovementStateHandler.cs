using System.Collections;
using UnityEngine;

public class MovementStateHandler : MonoBehaviour
{
    private PlayerController _player;

    public Enums.CharacterMoveState moveState;

    // -- Momentum Stats -- //
    private float _desiredVelocity;
    private float _lastDesiredVelocity;
    public float speedChangeFactor;
    public Enums.CharacterMoveState lastState;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }
    public void HandleState()
    {
        if (_player.CheckCollisions.IsGrounded && _player.Movement.isSprinting && _player.CurrentCharacterState != Enums.CharacterState.Idle)
        {
            ChangeMoveState(Enums.CharacterMoveState.Running);
            _desiredVelocity = _player.Stats.RunSpeed;
        }
        else if (_player.CheckCollisions.IsGrounded && _player.CurrentCharacterState == Enums.CharacterState.Idle)
        {
            ChangeMoveState(Enums.CharacterMoveState.None);
            _desiredVelocity = 0;
        }
        else if (_player.CheckCollisions.IsGrounded)
        {
            ChangeMoveState(Enums.CharacterMoveState.Walking);
            _desiredVelocity = _player.Stats.WalkSpeed;
        }
        else
        {
            ChangeMoveState(Enums.CharacterMoveState.Air);
            if (lastState == Enums.CharacterMoveState.Walking)
            {
                _desiredVelocity = _player.Stats.WalkSpeed;
            }
            else if (lastState == Enums.CharacterMoveState.Running)
            {
                _desiredVelocity = _player.Stats.RunSpeed;
            }
        }

        bool desiredMoveSpeedHasChanged = _desiredVelocity != _lastDesiredVelocity;

        if (desiredMoveSpeedHasChanged)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        _lastDesiredVelocity = _desiredVelocity;
    }

    public void ChangeMoveState(Enums.CharacterMoveState newState)
    {
        if (moveState == newState)
        {
            return;
        }

        lastState = moveState;

        moveState = newState;
    }


    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float t = 0;
        float difference = Mathf.Abs(_desiredVelocity - _player.Movement.MoveSpeed);
        float startValue;
        if (_player.Rigidbody.velocity.magnitude > _desiredVelocity)
        {
            startValue = _player.Movement.MoveSpeed;
        }
        else
        {
            startValue = _player.Rigidbody.velocity.magnitude;
        }

        float boostFactor = speedChangeFactor;

        while (t < difference)
        {
            _player.Movement.MoveSpeed = Mathf.Lerp(startValue, _desiredVelocity, t / difference);
            t += Time.deltaTime * boostFactor;

            yield return null;
        }

        _player.Movement.MoveSpeed = _desiredVelocity;

        if (_player.Movement.MoveSpeed == 0)
        {
            _player.Movement.MoveDirection = Vector3.zero;
            _player.Rigidbody.velocity = Vector3.zero;
            _player.Rigidbody.angularVelocity = Vector3.zero;
        }
    }

}
