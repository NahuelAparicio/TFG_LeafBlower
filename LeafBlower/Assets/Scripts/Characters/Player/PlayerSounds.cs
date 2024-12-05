using FMODUnity;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private PlayerController _player;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }
    public void PlayFootSteps()
    {
        if (_player.CheckCollisions.IsGrounded && _player.CurrentCharacterState != Enums.CharacterState.Idle)
            RuntimeManager.PlayOneShot("event:/Character/FootSteps/FootSteps_Concrete", transform.position);

    }
}
