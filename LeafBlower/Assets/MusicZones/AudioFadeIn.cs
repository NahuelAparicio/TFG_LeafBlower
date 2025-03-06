using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioFadeIn : MonoBehaviour
{
    private Bus masterBus;
    private float delayBeforeFade = 1f; // Espera 1 segundo antes de empezar
    private float fadeDuration = 3f; // Duración del fade-in en segundos

    void Start()
    {
        masterBus = RuntimeManager.GetBus("bus:/");

        // Asegurar que el volumen empiece en 0
        masterBus.setVolume(0f);

        // Iniciar la corutina del fade-in
        StartCoroutine(FadeInAudio());
    }

    IEnumerator FadeInAudio()
    {
        // Espera antes de comenzar el fade-in
        yield return new WaitForSeconds(delayBeforeFade);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float volume = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            masterBus.setVolume(volume);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        masterBus.setVolume(1f); // Asegurar que llegue a 100%
    }
}
