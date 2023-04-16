using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICommonScene : UIBaseScene
{
    public enum IMAGE
    {
        FadeImage
    }

    private ConfirmPanel confirmPanel;
    private NoticePanel noticePanel;
    private Image fadeImage;

    private OptionPopup optionPopup;

    public void Initialize()
    {
        if (isInitialized == true)
        {
            Debug.Log($"{this}: Already Initialized.");
            return;
        }
        isInitialized = true;

        BindImage(typeof(IMAGE));
        fadeImage = GetImage((int)IMAGE.FadeImage);

        // Panel
        confirmPanel = gameObject.GetComponentInChildren<ConfirmPanel>(true);
        noticePanel = gameObject.GetComponentInChildren<NoticePanel>(true);
        confirmPanel.Initialize();
        noticePanel.Initialize();
        // Popup
        optionPopup = gameObject.GetComponentInChildren<OptionPopup>(true);
        optionPopup.Initialize();
    }

    public void RequestNotice()
    {

    }
    public void RequestConfirm(string content, UnityAction action)
    {
        confirmPanel.SetConfirmPanel(content, action);
        OpenPanel(confirmPanel);
    }

    public void SetAlpha(float alpha)
    {
        fadeImage.color = Functions.SetColor(fadeImage.color, alpha);
    }

    public void FadeIn(float fadeTime = 1f, UnityAction callback = null)
    {
        StartCoroutine(CoFadeIn(fadeTime, callback));
    }

    public void FadeOut(float fadeTime = 1f, UnityAction callback = null)
    {
        StartCoroutine(CoFadeOut(fadeTime, callback));
    }

    IEnumerator CoFadeIn(float fadeTime, UnityAction callback = null)
    {
        Color color = fadeImage.color;
        float currentTime = 0f;

        while (currentTime <= fadeTime)
        {
            currentTime += Time.deltaTime;
            color.a -= (currentTime / fadeTime);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;

        callback?.Invoke();
    }

    IEnumerator CoFadeOut(float fadeTime, UnityAction callback = null)
    {
        Color color = fadeImage.color;
        float currentTime = 0f;

        while (currentTime <= fadeTime)
        {
            currentTime += Time.deltaTime;
            color.a += (currentTime / fadeTime);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        callback?.Invoke();
    }

    #region Property
    public ConfirmPanel ConfirmPanel { get { return confirmPanel; } }
    public NoticePanel NoticePanel { get { return noticePanel; } }
    public OptionPopup OptionPopup { get { return optionPopup; } }
    #endregion
}
