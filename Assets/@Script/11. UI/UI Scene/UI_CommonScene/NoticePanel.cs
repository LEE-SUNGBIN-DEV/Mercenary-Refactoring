using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoticePanel : UIPanel
{
    public enum TEXT
    {
        NoticeText
    }

    private bool isNotice;
    private float noticeTime;
    private Queue<string> systemNoticeQueue = new Queue<string>();

    public void Initialize()
    {
        BindText(typeof(TEXT));

        Managers.UIManager.CommonSceneUI.OnRequestNotice -= AcceptRequest;
        Managers.UIManager.CommonSceneUI.OnRequestNotice += AcceptRequest;

        isNotice = false;
        noticeTime = 0f;
    }

    private void Update()
    {
        if (isNotice == false && systemNoticeQueue.Count != 0)
        {
            isNotice = true;
            noticeTime += Time.deltaTime;

            GetText((int)TEXT.NoticeText).text = systemNoticeQueue.Dequeue();
            GetText((int)TEXT.NoticeText).gameObject.SetActive(true);

            if (noticeTime >= Constants.TIME_CLIENT_NOTICE)
            {
                isNotice = false;
                noticeTime = 0f;
                GetText((int)TEXT.NoticeText).gameObject.SetActive(false);
            }
        }
    }

    public void AcceptRequest(string content)
    {
        systemNoticeQueue.Enqueue(content);
    }
}
