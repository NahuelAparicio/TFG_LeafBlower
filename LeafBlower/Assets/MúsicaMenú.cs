using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MenuMusicController : MonoBehaviour
{
    public FMODUnity.EventReference menuMusicEvent;


    private EventInstance musicInstance;
    private bool isMusicPlaying = false;

    void Start()
    {
        // Al iniciar, reproduce la música del menú
        PlayMenuMusic();
    }

    // Función para iniciar la música del menú
    public void PlayMenuMusic()
    {
        // Si la música ya está reproduciéndose, no hacemos nada
        if (isMusicPlaying) return;

        // Crear la instancia del evento y reproducirlo
        musicInstance = RuntimeManager.CreateInstance(menuMusicEvent);
        musicInstance.start();
        isMusicPlaying = true;
    }

    // Función para detener la música del menú
    public void StopMenuMusic()
    {
        // Detener y liberar la instancia de la música si está reproduciéndose
        if (isMusicPlaying)
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);  // Detiene la música con fundido
            musicInstance.release();
            isMusicPlaying = false;
        }
    }

    // Llamado cuando se destruye el objeto (asegúrate de liberar recursos si es necesario)
    void OnDestroy()
    {
        // Detener y liberar la instancia de música al destruir el objeto
        if (isMusicPlaying)
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);  // Detener inmediatamente
            musicInstance.release();
        }
    }
}
