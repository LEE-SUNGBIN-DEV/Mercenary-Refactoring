using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPanel : UIBase
{
    [SerializeField] private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;

    protected virtual void Awake()
    {
        canvasGroup = Functions.GetOrAddComponent<CanvasGroup>(gameObject);
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
        fadeCoroutine = null;

        callback?.Invoke();
    }

    protected void FadeInPanel(float duration = Constants.TIME_UI_PANEL_DEFAULT_FADE, UnityAction callback = null)
    {
        gameObject.SetActive(true);
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(CoFade(0f, 1f, duration, callback));
    }
    protected void FadeOutPanel(float duration = Constants.TIME_UI_PANEL_DEFAULT_FADE, UnityAction callback = null)
    {
        gameObject.SetActive(true);
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(CoFade(1f, 0f, duration, callback));
    }
}
