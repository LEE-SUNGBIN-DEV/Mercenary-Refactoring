using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ResponseTracePanel : UIPanel, IFocusPanel
{
    public enum TEXT
    {
        Title_Text,
        Content_Text
    }
    public event UnityAction<IFocusPanel> OnOpenFocusPanel;
    public event UnityAction<IFocusPanel> OnCloseFocusPanel;

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
        OnOpenFocusPanel?.Invoke(this);
        Managers.InputManager.PushInputMode(CHARACTER_INPUT_MODE.INTERACTION);
        titleText.text = title;
        contentText.text = content;
        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        OnCloseFocusPanel?.Invoke(this);
        Managers.InputManager.PopInputMode();
        gameObject.SetActive(false);
        titleText.text = null;
        contentText.text = null;
    }
}
