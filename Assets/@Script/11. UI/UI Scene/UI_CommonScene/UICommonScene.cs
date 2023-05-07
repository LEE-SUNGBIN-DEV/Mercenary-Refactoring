using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICommonScene : UIBaseScene
{
    public event UnityAction<string> OnRequestNotice;

    // Panel
    private ConfirmPanel confirmPanel;
    private NoticePanel noticePanel;
    private OptionPanel optionPanel;

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
        optionPanel = GetComponentInChildren<OptionPanel>(true);

        confirmPanel.Initialize();
        noticePanel.Initialize();
        optionPanel.Initialize();

        // Fade
        fadeImage = Functions.FindChild<Image>(gameObject, "Fade_Image");
        fadeDuration = 1f;
    }

    public void RequestNotice(string content)
    {
        OnRequestNotice(content);
    }

    public void NoticeQuestState(Quest quest)
    {
        if (quest.TaskIndex == 1)
        {
            RequestNotice("Äù½ºÆ® ¼ö¶ô");
        }

        if (quest.TaskIndex == quest.QuestTasks.Length)
        {
            RequestNotice("Äù½ºÆ® ¿Ï·á");
        }
    }

    public void RequestConfirm(string content, UnityAction action)
    {
        confirmPanel.SetConfirmPanel(content, action);
        Managers.UIManager.OpenPanel(confirmPanel);
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
    public OptionPanel OptionPanel { get { return optionPanel; } }
    #endregion
}
