using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    public Image imageToFade;
    public float fadeDuration = 1f; // Duración del fade

    private Coroutine fadeCoroutine;

    public void OnFadeIn()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeImageAlpha(0f, 1f));
    }

    public void OnFadeOut()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeImageAlpha(1f, 0f));
    }

    private IEnumerator FadeImageAlpha(float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;
        Color color = imageToFade.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            color.a = newAlpha;
            imageToFade.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        imageToFade.color = color;
    }
}
