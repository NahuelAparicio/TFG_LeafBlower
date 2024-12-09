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
            float currentValue = _animator.GetFloat(Constants.ANIM_VAR_JOYDIR);
            if(_player.Movement.isSprinting)
            {
                if (_animator.GetFloat(Constants.ANIM_VAR_JOYDIR) == 2) return;

                if(currentValue < 1.95f)
                {
                    LerpJoyDir(currentValue, 2f, sprintLerpDuration);
                }
                else
                {
                    _animator.SetFloat(Constants.ANIM_VAR_JOYDIR, 2f);
                }
            }
            else
            {
                if( currentValue > 0.95f && currentValue < 1.05f)
                {
                    _animator.SetFloat(Constants.ANIM_VAR_JOYDIR, 1f);
                    return;
                }
                float targetValue = Mathf.Clamp01(_player.Rigidbody.velocity.magnitude / _player.Movement.MoveSpeed);
                if(!float.IsNaN(targetValue))
                {
                    LerpJoyDir(currentValue, targetValue, decreaseLerpDuration);                    
                }
            }
        }
    }

    internal void HandleIdleAnimations()
    {
        //If there is more than one idle animation, Second Blend Tree
        if(!_player.Inputs.IsMovingJoystick())
        {
            float currentValue = _animator.GetFloat(Constants.ANIM_VAR_JOYDIR);

            if (currentValue > 0.05f)
            {
                LerpJoyDir(currentValue, 0, decreaseLerpDuration);
            }
            else
            {
                _animator.SetFloat(Constants.ANIM_VAR_JOYDIR, 0);
            }
        }
    }

    internal void HandleTalkingAnimation()
    {
        _animator.SetFloat(Constants.ANIM_VAR_JOYDIR, 0);
        _player.Rigidbody.velocity = Vector3.zero;
    }

    internal void HandleJumpAnimations()
    {
        PlayTargetAnimation(Constants.ANIM_JUMP, true);
    }

    private void LerpJoyDir(float currentValue, float targetValue, float duration)
    {
        if(Mathf.Approximately(duration, targetValue))
        {
            _animator.SetFloat(Constants.ANIM_VAR_JOYDIR, targetValue);
            return;
        }
        float newValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime / duration);
        _animator.SetFloat(Constants.ANIM_VAR_JOYDIR, newValue);
    }
}
