using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicZone5 : MonoBehaviour
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
            MusicZoneManager.PlayNewMusic(musicInstance);
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
                    MusicZoneManager.PlayNewMusic(musicInstance);
                }
            }
            else
            {
                musicInstance = RuntimeManager.CreateInstance(musicEventPath);
                MusicZoneManager.PlayNewMusic(musicInstance);
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
            {
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
            musicInstance.clearHandle();
        }
    }

    void OnDestroy()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicInstance.release();
            musicInstance.clearHandle();
        }
    }
}
