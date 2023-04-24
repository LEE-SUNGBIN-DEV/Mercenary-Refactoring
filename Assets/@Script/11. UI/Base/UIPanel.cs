using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPanel : UIBase
{
    private CanvasGroup canvasGroup;
    private Coroutine currentFadeCoroutine;

    private void Awake()
    {
        canvasGroup = Functions.GetOrAddComponent<CanvasGroup>(gameObject);
    }

    public void FadeIn(float duration = Constants.TIME_UI_PANEL_DEFAULT_FADE, UnityAction callback = null)
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        currentFadeCoroutine = StartCoroutine(CoFade(0f, 1f, duration, callback));
    }

    public void FadeOut(float duration = Constants.TIME_UI_PANEL_DEFAULT_FADE, UnityAction callback = null)
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        currentFadeCoroutine = StartCoroutine(CoFade(1f, 0f, duration, callback));
    }

    private IEnumerator CoFade(float startAlpha, float targetAlpha, float duration = Constants.TIME_UI_PANEL_DEFAULT_FADE, UnityAction callback = null)
    {
        float elapsedTime = 0f;
        float currentAlpha;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            canvasGroup.alpha = currentAlpha;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        currentFadeCoroutine = null;

        callback?.Invoke();
    }
}
