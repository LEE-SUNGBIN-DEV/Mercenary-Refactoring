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

    public void Initialize()
    {
        BindText(typeof(TEXT));

        centerNoticeText = GetText((int)TEXT.Center_Notice_Text);

        isNotice = false;
        duration = 3f;
    }

    private void Update()
    {
        if(noticeQueue.Count > 0)
        {
            TryNotice();
        }
    }

    public void TryNotice()
    {
        if (isNotice)
            return;

        if (noticeCoroutine != null)
            StopCoroutine(noticeCoroutine);

        isNotice = true;
        noticeCoroutine = StartCoroutine(CoNotice(duration));
    }

    public void RequestNotice(string content)
    {
        noticeQueue.Enqueue(content);
    }

    public IEnumerator CoNotice(float duration)
    {
        centerNoticeText.text = noticeQueue.Dequeue();
        FadeIn();
        yield return new WaitForSeconds(duration);
        isNotice = false;
        FadeOut();
    }
}
