using FMOD.Studio;
using FMODUnity;
using UnityEngine;
public class PlayerSounds : MonoBehaviour
{
    private PlayerController _player;
    private EventInstance engineSound;
    PLAYBACK_STATE engineSTATE;
    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        engineSound = RuntimeManager.CreateInstance("event:/Vehicles/Engine");
        Set3DAttributes();
        engineSound.setParameterByName("RPM", 2000);
    }

    private void Update()
    {
        engineSound.getPlaybackState(out engineSTATE);
        if (engineSTATE == PLAYBACK_STATE.PLAYING)
        {
            Set3DAttributes();
        }
    }

    private void Set3DAttributes()
    {
        FMOD.ATTRIBUTES_3D attributes = RuntimeUtils.To3DAttributes(transform.position);
        engineSound.set3DAttributes(attributes);
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

    public void PlayEngineSound() => engineSound.start();
    public void StopEngineSound() => engineSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

    private void OnDestroy()
    {
        engineSound.release();
    }
}
