using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemMessagePanel : UIPanel
{
    public enum TEXT
    {
        System_Message_Text
    }

    private TextMeshProUGUI systemMessageText;
    private Queue<string> systemMessageQueue = new Queue<string>();
    private bool isNotice;
    private float noticeTime;

    #region Private
    private void Update()
    {
        if (isNotice == false && systemMessageQueue.Count != 0)
        {
            isNotice = true;
            noticeTime += Time.deltaTime;

            systemMessageText.text = systemMessageQueue.Dequeue();
            gameObject.SetActive(true);

            if (noticeTime >= Constants.TIME_CLIENT_NOTICE)
            {
                isNotice = false;
                noticeTime = 0f;
                gameObject.SetActive(false);
            }
        }
    }
    #endregion
    public void Initialize()
    {
        BindText(typeof(TEXT));
        systemMessageText = GetText((int)TEXT.System_Message_Text);

        isNotice = false;
        noticeTime = 0f;
    }
    public void OpenPanel(string content)
    {
        systemMessageQueue.Enqueue(content);
    }
}
