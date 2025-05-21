using System.Collections;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(Collider))]
public class MusicZone : MonoBehaviour
{
    public EventReference musicEvent;
    private bool isPlayerInside = false;
    private Coroutine stopCoroutine;
    private Collider zoneCollider;

    void Start()
    {
        zoneCollider = GetComponent<Collider>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && zoneCollider.bounds.Contains(player.transform.position))
        {
            isPlayerInside = true;
            PlayZoneMusic();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPlayerInside)
            {
                isPlayerInside = true;
                PlayZoneMusic();
            }

            CancelStopMusicCoroutine(); // <<--- Este método debe existir
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            stopCoroutine = StartCoroutine(StopMusicAfterDelay(120f));
        }
    }

    IEnumerator StopMusicAfterDelay(float delay)
    {
        float timer = 0f;

        while (timer < delay)
        {
            if (isPlayerInside)
                yield break;

            timer += Time.deltaTime;
            yield return null;
        }

        if (MusicZoneManager.Instance != null)
        {
            MusicZoneManager.Instance.StopMusicIfZoneMatches(this);
        }
    }

    // Este método debe estar aquí para evitar el error
    public void CancelStopMusicCoroutine()
    {
        if (stopCoroutine != null)
        {
            StopCoroutine(stopCoroutine);
            stopCoroutine = null;
        }
    }

    private void PlayZoneMusic()
    {
        if (MusicZoneManager.Instance != null)
        {
            MusicZoneManager.Instance.PlayMusic(musicEvent, this);
        }
    }
}
