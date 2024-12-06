using FMODUnity;
using UnityEngine;
using System.Collections; // Necesario para IEnumerator
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
    public void PlayJump()
    {
            RuntimeManager.PlayOneShot("event:/Character/Jump/Jump_Concrete", transform.position);
    }
    public void PlayLand()
    {
        RuntimeManager.PlayOneShot("event:/Character/Land/Land_Concrete", transform.position);
    }


}
