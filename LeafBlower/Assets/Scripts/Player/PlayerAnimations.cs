using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private PlayerController _player;
    private Animator _animator;
    public Animator Animator => _animator;

    private void Awake()
    {
        _player = transform.parent.GetComponent<PlayerController>();
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
            _animator.SetFloat(Constants.ANIM_VAR_JOYDIR, _player.Inputs.GetMoveDirection().magnitude);
        }
    }

    internal void HandleIdleAnimations()
    {
        //If there is more than one idle animation, Second Blend Tree
        if(!_player.Inputs.IsMovingJoystick())
        {
            //Lerp from value to 0
            _animator.SetFloat(Constants.ANIM_VAR_JOYDIR, 0f);
        }
    }
}
