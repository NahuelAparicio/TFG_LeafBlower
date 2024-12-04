using System.Collections;
using UnityEngine;

// -- Handles Movement States and Lerps Between Speeds (to smooth transitions) on State Changes
public class MovementStateHandler : MonoBehaviour
{

    public float speedChangeFactor;
    public float toZeroChangeFactor;

    private PlayerController _player;
    private Enums.CharacterMoveState _moveState;
    private Enums.CharacterMoveState _lastState;

    // -- Momentum Stats -- //
    private float _desiredVelocity;
    private float _lastDesiredVelocity;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    // -- Handles Movement State depending on character movement (grounded, air, etc)
    public void HandleState()
    {
        if (_player.CheckCollisions.IsGrounded && _player.Movement.isSprinting && _player.CurrentCharacterState != Enums.CharacterState.Idle)
        {
            ChangeMoveState(Enums.CharacterMoveState.Running);
        }
        else if (_player.CheckCollisions.IsGrounded && _player.CurrentCharacterState == Enums.CharacterState.Idle)
        {
            ChangeMoveState(Enums.CharacterMoveState.None);
        }
        else if (_player.CheckCollisions.IsGrounded)
        {
            ChangeMoveState(Enums.CharacterMoveState.Walking);
        }
        else
        {
            ChangeMoveState(Enums.CharacterMoveState.Air);
        }

        bool desiredMoveSpeedHasChanged = _desiredVelocity != _lastDesiredVelocity;

        if (desiredMoveSpeedHasChanged)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        _lastDesiredVelocity = _desiredVelocity;
    }

    // -- Sets the desired Speed depending on the move state changes (run, walk, no move, etc.)
    public void ChangeMoveState(Enums.CharacterMoveState newState)
    {
        if (_moveState == newState)
        {
            return;
        }

        switch (newState)
        {
            case Enums.CharacterMoveState.None:
                _desiredVelocity = 0;
                break;
            case Enums.CharacterMoveState.Walking:
                _desiredVelocity = _player.Stats.WalkSpeed;
                break;
            case Enums.CharacterMoveState.Running:
                _desiredVelocity = _player.Stats.RunSpeed;
                break;
            case Enums.CharacterMoveState.Air:
                if (_lastState == Enums.CharacterMoveState.Walking)
                {
                    _desiredVelocity = _player.Stats.WalkSpeed;
                }
                else if (_lastState == Enums.CharacterMoveState.Running)
                {
                    _desiredVelocity = _player.Stats.RunSpeed;
                }
                break;
            default:
                break;
        }

        _lastState = _moveState;

        _moveState = newState;
    }

    // -- Lerps Between currentSpeed and desiredSpeed to smooth Transition
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
        if (_desiredVelocity == 0) boostFactor = toZeroChangeFactor;

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
