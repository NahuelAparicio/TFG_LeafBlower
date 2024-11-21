using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private PlayerController _player;
    private Animator _animator;
    public Animator Animator => _animator;

    public float decreaseLerpDuration, sprintLerpDuration;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        _animator.SetBool("isInteracting", isInteracting);
        _animator.CrossFade(targetAnimation, 0.2f);
    }

    internal void HandleMovingAnimations()
    {
        if(_player.Inputs.IsMovingJoystick())
        {
            if(_player.Movement.isSprinting)
            {
                if (_animator.GetFloat(Constants.ANIM_VAR_JOYDIR) == 2) return;

                float currentValue = _animator.GetFloat(Constants.ANIM_VAR_JOYDIR);
                float targetValue = 2f;
                float newValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime / sprintLerpDuration);
                _animator.SetFloat(Constants.ANIM_VAR_JOYDIR, newValue);
            }
            else
            {
                float currentValue = _animator.GetFloat(Constants.ANIM_VAR_JOYDIR);
                float targetValue = Mathf.Clamp01(_player.Rigidbody.velocity.magnitude / _player.Movement.moveSpeed);
                float newValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime / decreaseLerpDuration);
                _animator.SetFloat(Constants.ANIM_VAR_JOYDIR, newValue);
            }
        }
    }

    internal void HandleIdleAnimations()
    {
        //If there is more than one idle animation, Second Blend Tree
        if(!_player.Inputs.IsMovingJoystick())
        {
            float currentValue = _animator.GetFloat(Constants.ANIM_VAR_JOYDIR);
            float newValue = Mathf.Lerp(currentValue, 0, Time.deltaTime / decreaseLerpDuration);
            _animator.SetFloat(Constants.ANIM_VAR_JOYDIR, newValue);
        }
    }

    internal void HandleJumpAnimations()
    {
        PlayTargetAnimation(Constants.ANIM_JUMP, true);
    }
}
