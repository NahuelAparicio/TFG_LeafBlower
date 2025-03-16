using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicTrigger : MonoBehaviour
{
    public string eventPath = "event:/MUSIC/MAIN THEME (SHORT)";
    private EventInstance musicInstance;
    private bool playerInside = false;

    private void Start()
    {
        musicInstance = RuntimeManager.CreateInstance(eventPath);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            CheckAndPlayMusic();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            StopMusic();
        }
    }

    private void CheckAndPlayMusic()
    {
        if (!playerInside) return;

        // Verificar si la música ya está sonando
        PLAYBACK_STATE playbackState;
        musicInstance.getPlaybackState(out playbackState);

        if (playbackState != PLAYBACK_STATE.PLAYING)
        {
            musicInstance.start();
        }
    }

    private void StopMusic()
    {
        // Detiene la música con fadeout
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void Update()
    {
        // Si el jugador sigue dentro y la música se ha detenido por el menú, la reactivamos
        if (playerInside)
        {
            PLAYBACK_STATE playbackState;
            musicInstance.getPlaybackState(out playbackState);

            if (playbackState == PLAYBACK_STATE.STOPPED)
            {
                musicInstance.start();
            }
        }
    }

    private void OnDestroy()
    {
        musicInstance.release();
    }
}
