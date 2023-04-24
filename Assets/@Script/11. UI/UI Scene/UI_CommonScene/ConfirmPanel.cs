using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class ConfirmPanel : UIPanel
{
    public enum TEXT
    {
        Confirm_Content_Text
    }

    public enum BUTTON
    {
        Confirm_Button,
        Cancel_Button
    }

    public event UnityAction OnConfirm;
    public event UnityAction OnCancel;

    private TextMeshProUGUI confirmContentText;
    private Button confirmButton;
    private Button cancelButton;

    private void OnDisable()
    {
        OnConfirm = null;
    }

    public void Initialize()
    {
        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        confirmContentText = GetText((int)TEXT.Confirm_Content_Text);
        confirmButton = GetButton((int)BUTTON.Confirm_Button);
        cancelButton = GetButton((int)BUTTON.Cancel_Button);

        confirmButton.onClick.AddListener(ClickConfirmButton);
        cancelButton.onClick.AddListener(ClickCancelButton);
    }

    public void ClickConfirmButton()
    {
        Managers.AudioManager.PlaySFX("Audio_Button_Click");
        OnConfirm?.Invoke();
        OnConfirm = null;
    }

    public void ClickCancelButton()
    {
        Managers.AudioManager.PlaySFX("Audio_Button_Click");
        OnConfirm = null;
        OnCancel?.Invoke();
    }

    public void SetConfirmPanel(string content, UnityAction action)
    {
        confirmContentText.text = content;
        OnConfirm -= action;
        OnConfirm += action;
    }
}
