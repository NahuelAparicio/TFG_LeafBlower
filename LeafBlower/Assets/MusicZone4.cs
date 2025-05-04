using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicZone4 : MonoBehaviour
{
    public EventReference musicEventPath;

    private EventInstance musicInstance;
    private bool isPlayerInside = false;
    private Coroutine musicStopCoroutine;

    private Collider zoneCollider;

    void Start()
    {
        zoneCollider = GetComponent<Collider>();

        // Verificamos si el Player ya está dentro al iniciar
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && zoneCollider.bounds.Contains(player.transform.position))
        {
            isPlayerInside = true;
            musicInstance = RuntimeManager.CreateInstance(musicEventPath);
            musicInstance.start();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Si el Player entra, marcamos como dentro y verificamos si ya hay una instancia en ejecución
            isPlayerInside = true;

            // Si ya hay una instancia de música reproduciéndose, no creamos una nueva
            if (musicInstance.isValid())
            {
                PLAYBACK_STATE state;
                musicInstance.getPlaybackState(out state);
                if (state != PLAYBACK_STATE.PLAYING)
                {
                    musicInstance.start(); // Iniciamos la música si no está sonando
                }
            }
            else
            {
                // Si no existe una instancia válida, creamos una nueva
                musicInstance = RuntimeManager.CreateInstance(musicEventPath);
                musicInstance.start();
            }

            // Cancelamos cualquier espera para detener la música si el Player vuelve
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
            // Si el Player sale de la zona, marcamos como fuera y comenzamos la espera
            isPlayerInside = false;

            musicStopCoroutine = StartCoroutine(StopMusicAfterDelay(7f));
        }
    }

    IEnumerator StopMusicAfterDelay(float delay)
    {
        float timer = 0f;

        // Esperamos durante 'delay' segundos
        while (timer < delay)
        {
            // Si el Player regresa antes de que pase el tiempo, cancelamos el proceso
            if (isPlayerInside)
            {
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Si pasaron 15 segundos y el Player no ha vuelto, paramos la música
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }

    void OnDestroy()
    {
        // Aseguramos de liberar la instancia de la música al destruir el objeto
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }
    }
}
