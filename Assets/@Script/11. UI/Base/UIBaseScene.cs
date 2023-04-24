using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBaseScene : UIBase
{
    protected bool isInitialized;
    protected Canvas canvas;
    protected LinkedList<UIPopup> currentPopUpLinkedList = new LinkedList<UIPopup>();

    public virtual void Initialize()
    {
        if (isInitialized == true)
        {
            Debug.Log($"{this}: Already Initialized.");
            return;
        }
        isInitialized = true;
        TryGetComponent<Canvas>(out canvas);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentPopUpLinkedList.Count > 0)
            {
                ClosePopup(currentPopUpLinkedList.First.Value);
            }
            else
            {
                Managers.GameManager.ToggleCursorMode();
            }
        }
    }

    #region Popup Function
    public void FocusPopup(UIPopup popUp)
    {
        currentPopUpLinkedList.Remove(popUp);
        currentPopUpLinkedList.AddFirst(popUp);
        RefreshPopupOrder();
    }

    public void RefreshPopupOrder()
    {
        foreach (UIPopup popUp in currentPopUpLinkedList)
        {
            popUp.transform.SetAsFirstSibling();
        }
    }

    public void OpenPanel(UIPanel panel)
    {
        if(panel.gameObject.activeSelf == false)
        {
            panel.gameObject.SetActive(true);
            panel.FadeIn(Constants.TIME_UI_PANEL_DEFAULT_FADE);
        }
    }

    public void ClosePanel(UIPanel panel)
    {
        if (panel.gameObject.activeSelf == true)
        {
            panel.FadeOut(Constants.TIME_UI_PANEL_DEFAULT_FADE, () => { panel.gameObject.SetActive(false); });
        }
            
    }

    public void TogglePanel(UIPanel panel)
    {
        if (panel.gameObject.activeSelf)
        {
            ClosePanel(panel);
        }
        else
        {
            OpenPanel(panel);
        }
    }

    public void OpenPopup(UIPopup popup)
    {
        if (currentPopUpLinkedList.Contains(popup) == false)
        {
            PlayPopupOpenSFX();
            currentPopUpLinkedList.AddFirst(popup);
            popup.gameObject.SetActive(true);
            popup.FadeIn(0.5f);
        }

        RefreshPopupOrder();
        Managers.GameManager.SetCursorMode(CURSOR_MODE.UNLOCK);
    }
    public void ClosePopup(UIPopup popup)
    {
        if (currentPopUpLinkedList.Contains(popup) == true)
        {
            PlayPopupCloseSFX();
            currentPopUpLinkedList.Remove(popup);
            popup.FadeOut(0.5f, () => popup.gameObject.SetActive(false));
        }

        RefreshPopupOrder();

        if (currentPopUpLinkedList.Count == 0)
        {
            Managers.GameManager.SetCursorMode(CURSOR_MODE.LOCK);
        }
    }

    public void TogglePopup(UIPopup popup)
    {
        if (popup.gameObject.activeSelf)
        {
            ClosePopup(popup);
        }
        else
        {
            OpenPopup(popup);
        }
    }
    #endregion

    #region Popup SFX Function
    public void PlayPopupOpenSFX()
    {
        Managers.AudioManager.PlaySFX("Audio_Popup_Open");
    }
    public void PlayPopupCloseSFX()
    {
        Managers.AudioManager.PlaySFX("Audio_Popup_Close");
    }
    #endregion
}
