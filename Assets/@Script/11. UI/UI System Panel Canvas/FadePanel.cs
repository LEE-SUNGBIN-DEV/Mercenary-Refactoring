using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadePanel : UIPanel
{
    public enum IMAGE
    {
        Fade_Image
    }

    private Image fadeImage;
    private Coroutine currentFadeCoroutine;
    private float fadeDuration;

    #region Private
    private IEnumerator CoFadeScreen(float startAlpha, float targetAlpha, UnityAction callback = null)
    {
        float elapsedTime = 0f;
        float currentAlpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, currentAlpha);
            yield return null;
        }
        currentFadeCoroutine = null;
        callback?.Invoke();

        if (startAlpha == 1f)
            gameObject.SetActive(false);
    }
    #endregion
    public void Initialize()
    {
        base.Awake();
        BindImage(typeof(IMAGE));
        fadeImage = GetImage((int)IMAGE.Fade_Image);
        fadeDuration = 1f;
    }

    public void FadeIn(float duration = 1f, UnityAction callback = null)
    {
        gameObject.SetActive(true);
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        fadeDuration = duration;
        currentFadeCoroutine = StartCoroutine(CoFadeScreen(1f, 0f, callback));
    }

    public void FadeOut(float duration = 1f, UnityAction callback = null)
    {
        gameObject.SetActive(true);
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        fadeDuration = duration;
        currentFadeCoroutine = StartCoroutine(CoFadeScreen(0f, 1f, callback));
    }
}
