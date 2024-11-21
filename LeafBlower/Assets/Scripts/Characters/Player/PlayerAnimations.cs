using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private PlayerController _player;
    private Animator _animator;
    public Animator Animator => _animator;

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
            _animator.SetFloat(Constants.ANIM_VAR_JOYDIR, Mathf.Clamp01(_player.Rigidbody.velocity.magnitude / _player.Stats.WalkSpeed));
        }
    }

    internal void HandleIdleAnimations()
    {
        //If there is more than one idle animation, Second Blend Tree
        if(!_player.Inputs.IsMovingJoystick())
        {
            _animator.SetFloat(Constants.ANIM_VAR_JOYDIR, 0f);
        }
    }

    internal void HandleJumpAnimations()
    {
        PlayTargetAnimation(Constants.ANIM_JUMP, true);
    }
}
