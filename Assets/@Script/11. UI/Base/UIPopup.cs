using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIPopup : UIBase, IPointerDownHandler
{
    public event UnityAction<UIPopup> OnFocus;

    private CanvasGroup canvasGroup;
    private Coroutine currentFadeCoroutine;
    private float fadeDuration;

    private void Awake()
    {
        canvasGroup = Functions.GetOrAddComponent<CanvasGroup>(gameObject);
        fadeDuration = 1f;
    }

    public void FadeIn(float duration = 1f, UnityAction callback = null)
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        fadeDuration = duration;
        currentFadeCoroutine = StartCoroutine(CoFade(0f, 1f));
    }

    public void FadeOut(float duration = 1f, UnityAction callback = null)
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        fadeDuration = duration;
        currentFadeCoroutine = StartCoroutine(CoFade(1f, 0f));
    }

    private IEnumerator CoFade(float startAlpha, float targetAlpha, UnityAction callback = null)
    {
        float elapsedTime = 0f;
        float currentAlpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            canvasGroup.alpha = currentAlpha;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        currentFadeCoroutine = null;

        callback?.Invoke();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnFocus?.Invoke(this);
    }
}
