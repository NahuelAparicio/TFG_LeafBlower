using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioFadeIn : MonoBehaviour
{
    private Bus masterBus;
    private float delayBeforeFade = 1f; // Espera 1 segundo antes de empezar
    private float fadeDuration = 3f; // Duración del fade-in en segundos

    void Awake()
    {
        //masterBus = RuntimeManager.GetBus("bus:/");

        //masterBus.setVolume(0f);
        //MusicManager.Instance.isMakingFadeIn = true;
        //StartCoroutine(FadeInAudio());
    }

    //IEnumerator FadeInAudio()
    //{
    //    yield return new WaitForSeconds(delayBeforeFade);

    //    float elapsedTime = 0f;

    //    while (elapsedTime < fadeDuration)
    //    {
    //        float volume = Mathf.Lerp(0f, MusicManager.Instance.masterVolume, elapsedTime / fadeDuration);
    //        masterBus.setVolume(volume);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    MusicManager.Instance.isMakingFadeIn = false;
    //    masterBus.setVolume(MusicManager.Instance.masterVolume); // Asegurar que llegue a 100%
    //}
}
