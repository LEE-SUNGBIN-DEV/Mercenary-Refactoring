using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CenterNoticePanel : UIPanel
{
    public enum TEXT
    {
        Center_Notice_Text
    }

    private TextMeshProUGUI centerNoticeText;

    private Queue<string> noticeQueue = new Queue<string>();
    private bool isNotice;
    private float duration;
    private Coroutine noticeCoroutine;

    #region Private
    private void Update()
    {
        if (noticeQueue.Count > 0 && !isNotice)
        {
            if (noticeCoroutine != null)
                StopCoroutine(noticeCoroutine);

            isNotice = true;
            noticeCoroutine = StartCoroutine(CoNotice(duration));
        }
    }

    private IEnumerator CoNotice(float duration)
    {
        centerNoticeText.text = noticeQueue.Dequeue();
        FadeInPanel();
        yield return new WaitForSeconds(duration);
        isNotice = false;
        FadeOutPanel(0.4f, () => IsEmpty());
    }
    private void IsEmpty()
    {
        if(noticeQueue.Count > 0)
            return;
        else
            ClosePanel();
    }
    #endregion
    public void Initialize()
    {
        base.Awake();
        BindText(typeof(TEXT));
        centerNoticeText = GetText((int)TEXT.Center_Notice_Text);

        isNotice = false;
        duration = 3f;
    }
    public void OpenPanel(string content)
    {
        noticeQueue.Enqueue(content);
        if(gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
