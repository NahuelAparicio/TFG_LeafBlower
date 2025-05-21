using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicZoneManager : MonoBehaviour
{
    public static MusicZoneManager Instance;

    private EventInstance currentMusicInstance;
    private EventReference currentMusicEvent;
    private MusicZone currentActiveZone;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Evita duplicados
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persiste entre escenas
    }

    /// <summary>
    /// Reproducir música desde una zona (zona activa controlada)
    /// </summary>
    public void PlayMusic(EventReference newMusicEvent, MusicZone requestingZone)
    {
        if (currentActiveZone == requestingZone)
            return; // Ya está sonando desde esta zona

        if (currentActiveZone != null)
        {
            currentActiveZone.CancelStopMusicCoroutine();
        }

        // Detener música anterior si había
        if (currentMusicInstance.isValid())
        {
            currentMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusicInstance.release();
        }

        currentMusicEvent = newMusicEvent;
        currentMusicInstance = RuntimeManager.CreateInstance(newMusicEvent);
        currentMusicInstance.start();

        currentActiveZone = requestingZone;
    }

    /// <summary>
    /// Reproducir música desde el menú u otros sistemas sin zona activa
    /// </summary>
    public void PlayMusic(EventReference newMusicEvent)
    {
        // Detener música actual si hay
        if (currentMusicInstance.isValid())
        {
            currentMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusicInstance.release();
        }

        currentMusicEvent = newMusicEvent;
        currentMusicInstance = RuntimeManager.CreateInstance(newMusicEvent);
        currentMusicInstance.start();

        currentActiveZone = null; // No zona activa
    }

    /// <summary>
    /// Detener música solo si proviene de la zona indicada
    /// </summary>
    public void StopMusicIfZoneMatches(MusicZone zone)
    {
        if (zone != null && currentActiveZone == zone)
        {
            if (currentMusicInstance.isValid())
            {
                currentMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                currentMusicInstance.release();
                currentMusicInstance = default;
                currentActiveZone = null;
            }
        }
    }

    private void OnDestroy()
    {
        if (currentMusicInstance.isValid())
        {
            currentMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusicInstance.release();
        }
    }
}
