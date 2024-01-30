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

    #region Private
    private void OnDisable()
    {
        OnConfirm = null;
        OnCancel = null;
    }
    private void OnClickConfirmButton()
    {
        Managers.AudioManager.PlaySFX("Audio_Button_Click");
        OnConfirm?.Invoke();
        OnConfirm = null;
        gameObject.SetActive(false);
    }

    private void OnClickCancelButton()
    {
        Managers.AudioManager.PlaySFX("Audio_Button_Click");
        OnConfirm = null;
        OnCancel?.Invoke();
        gameObject.SetActive(false);
    }
    #endregion
    public void Initialize()
    {
        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        confirmContentText = GetText((int)TEXT.Confirm_Content_Text);
        confirmButton = GetButton((int)BUTTON.Confirm_Button);
        cancelButton = GetButton((int)BUTTON.Cancel_Button);

        confirmButton.onClick.AddListener(OnClickConfirmButton);
        cancelButton.onClick.AddListener(OnClickCancelButton);
    }
    public void OpenPanel(string content, UnityAction action = null)
    {
        confirmContentText.text = content;
        OnConfirm -= action;
        OnConfirm += action;

        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
