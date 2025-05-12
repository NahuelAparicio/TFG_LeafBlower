using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicZone : MonoBehaviour
{
    public EventReference musicEventPath;

    private EventInstance musicInstance;
    private bool isPlayerInside = false;
    private Coroutine musicStopCoroutine;

    private Collider zoneCollider;

    void Start()
    {
        zoneCollider = GetComponent<Collider>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && zoneCollider.bounds.Contains(player.transform.position))
        {
            isPlayerInside = true;
            musicInstance = RuntimeManager.CreateInstance(musicEventPath);
            musicInstance.start();
        }
    }

    void Update()
    {
        // Verifica si estamos dentro y la música no está sonando después de reanudar
        if (isPlayerInside && musicInstance.isValid())
        {
            float pauseValue;
            RuntimeManager.StudioSystem.getParameterByName("Pause", out pauseValue);

            if (pauseValue == 0f) // Solo si ya no está en pausa
            {
                PLAYBACK_STATE state;
                musicInstance.getPlaybackState(out state);
                if (state != PLAYBACK_STATE.PLAYING && state != PLAYBACK_STATE.STARTING)
                {
                    musicInstance.start();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            if (musicInstance.isValid())
            {
                PLAYBACK_STATE state;
                musicInstance.getPlaybackState(out state);
                if (state != PLAYBACK_STATE.PLAYING)
                {
                    musicInstance.start();
                }
            }
            else
            {
                musicInstance = RuntimeManager.CreateInstance(musicEventPath);
                musicInstance.start();
            }

            if (musicStopCoroutine != null)
            {
                StopCoroutine(musicStopCoroutine);
                musicStopCoroutine = null;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;

            musicStopCoroutine = StartCoroutine(StopMusicAfterDelay(7f));
        }
    }

    IEnumerator StopMusicAfterDelay(float delay)
    {
        float timer = 0f;

        while (timer < delay)
        {
            if (isPlayerInside)
                yield break;

            float pauseValue;
            RuntimeManager.StudioSystem.getParameterByName("Pause", out pauseValue);

            if (pauseValue == 1f)
            {
                yield return null;
                continue;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
        }
    }

    void OnDestroy()
    {
        float pauseValue = 0f;
        RuntimeManager.StudioSystem.getParameterByName("Pause", out pauseValue);

        // No destruir la música si estamos en pausa
        if (musicInstance.isValid() && pauseValue != 1f)
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }
    }
}
