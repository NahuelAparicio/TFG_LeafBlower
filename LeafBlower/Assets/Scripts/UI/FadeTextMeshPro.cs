using UnityEngine;
using TMPro;
using System.Collections;

public class FadeTextMeshPro : MonoBehaviour
{
    public TextMeshProUGUI textToFade;
    public float fadeDuration = 1f; // Duración del fade

    private Coroutine fadeCoroutine;

    public void OnFadeIn()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeTextAlpha(0f, 1f));
    }

    public void OnFadeOut()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeTextAlpha(1f, 0f));
    }

    private IEnumerator FadeTextAlpha(float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;
        Color color = textToFade.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            color.a = newAlpha;
            textToFade.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        textToFade.color = color;
    }
}
