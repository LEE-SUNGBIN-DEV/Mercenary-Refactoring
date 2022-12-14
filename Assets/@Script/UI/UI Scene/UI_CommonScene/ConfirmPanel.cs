using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class ConfirmPanel : UIPanel
{
    public enum TEXT
    {
        ConfirmText
    }

    public enum BUTTON
    {
        ConfirmButton,
        CancelButton
    }

    public event UnityAction OnConfirm;

    public void Initialize()
    {
        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        BindEvent(GetButton((int)BUTTON.ConfirmButton).gameObject, OnClickConfirmButton, UI_EVENT.Click);
        BindEvent(GetButton((int)BUTTON.CancelButton).gameObject, OnClickCancelButton, UI_EVENT.Click);
    }

    public void OnClickConfirmButton()
    {
        Managers.AudioManager.PlaySFX("Audio_Button_Click");
        OnConfirm();
        OnConfirm = null;
        gameObject.SetActive(false);
    }

    public void OnClickCancelButton()
    {
        Managers.AudioManager.PlaySFX("Audio_Button_Click");
        OnConfirm = null;
        gameObject.SetActive(false);
    }

    public void SetConfirmPanel(string content, UnityAction action)
    {
        GetText((int)TEXT.ConfirmText).text = content;
        OnConfirm -= action;
        OnConfirm += action;
    }
}
