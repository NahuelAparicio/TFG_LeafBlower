using UnityEngine;
using FMODUnity;

public class MenuMusicController : MonoBehaviour
{
    [Header("Música del Menú Principal")]
    public EventReference menuMusicEvent;

    void Start()
    {
        // Reproducir la música del menú a través del MusicZoneManager
        if (MusicZoneManager.Instance != null)
        {
            MusicZoneManager.Instance.PlayMusic(menuMusicEvent);
        }
    }
}
