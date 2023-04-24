using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICommonScene : UIBaseScene
{
    // Panel
    private ConfirmPanel confirmPanel;
    private NoticePanel noticePanel;

    // Popup
    private OptionPopup optionPopup;

    // Fade
    private Image fadeImage;
    private Coroutine currentFadeCoroutine;
    private float fadeDuration;

    public override void Initialize()
    {
        base.Initialize();

        canvas.sortingOrder = 1;

        // Panel
        confirmPanel = GetComponentInChildren<ConfirmPanel>(true);
        noticePanel = GetComponentInChildren<NoticePanel>(true);

        confirmPanel.Initialize();
        noticePanel.Initialize();

        // Popup
        optionPopup = gameObject.GetComponentInChildren<OptionPopup>(true);
        optionPopup.Initialize();

        // Fade
        fadeImage = Functions.FindChild<Image>(gameObject, "Fade_Image");
        fadeDuration = 1f;
    }

    public void RequestNotice()
    {

    }

    public void RequestConfirm(string content, UnityAction action)
    {
        confirmPanel.SetConfirmPanel(content, action);
        OpenPanel(confirmPanel);
    }

    public void FadeIn(float duration = 1f, UnityAction callback = null)
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        fadeDuration = duration;
        currentFadeCoroutine = StartCoroutine(CoFade(1f, 0f, callback));
    }

    public void FadeOut(float duration = 1f, UnityAction callback = null)
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        fadeDuration = duration;
        currentFadeCoroutine = StartCoroutine(CoFade(0f, 1f, callback));
    }

    private IEnumerator CoFade(float startAlpha, float targetAlpha, UnityAction callback = null)
    {
        float elapsedTime = 0f;
        float currentAlpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            fadeImage.color = Functions.SetColor(fadeImage.color, currentAlpha);
            yield return null;
        }

        currentFadeCoroutine = null;

        callback?.Invoke();
    }

    #region Property
    public ConfirmPanel ConfirmPanel { get { return confirmPanel; } }
    public NoticePanel NoticePanel { get { return noticePanel; } }
    public OptionPopup OptionPopup { get { return optionPopup; } }
    #endregion
}
