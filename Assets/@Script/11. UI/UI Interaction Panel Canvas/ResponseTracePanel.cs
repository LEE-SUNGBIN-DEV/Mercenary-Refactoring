using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ResponseTracePanel : UIPanel, IInteractionPanel
{
    public enum TEXT
    {
        Title_Text,
        Content_Text
    }

    public event UnityAction<IInteractionPanel> OnOpenPanel;
    public event UnityAction<IInteractionPanel> OnClosePanel;

    private TextMeshProUGUI titleText;
    private TextMeshProUGUI contentText;

    public void Initialize()
    {
        BindText(typeof(TEXT));

        titleText = GetText((int)TEXT.Title_Text);
        contentText = GetText((int)TEXT.Content_Text);
    }
    public void OpenPanel(string title, string content)
    {
        OnOpenPanel?.Invoke(this);
        titleText.text = title;
        contentText.text = content;
        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        OnClosePanel?.Invoke(this);
        gameObject.SetActive(false);
        titleText.text = null;
        contentText.text = null;
    }
}
