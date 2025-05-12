using UnityEngine;
using FMODUnity;

public class MainMenuMusicPlayer : MonoBehaviour
{
    [SerializeField] private EventReference mainMenuMusic;

    private void Start()
    {
        if (MusicZoneManager.Instance != null)
        {
            MusicZoneManager.Instance.PlayMusic(mainMenuMusic); // Sin zona activa
        }
        else
        {
            Debug.LogWarning("❌ MusicZoneManager.Instance no encontrado en escena.");
        }
    }
}
