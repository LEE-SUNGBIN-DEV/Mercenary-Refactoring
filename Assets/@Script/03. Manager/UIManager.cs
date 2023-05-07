using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager
{
    private UICommonScene commonSceneUI;
    private UISelectionScene selectionSceneUI;
    private UIGameScene gameSceneUI;
    private UIPanel activeUserPanel;

    private bool canInput;

    public void Initialize(Transform rootTransform)
    {
        commonSceneUI = Managers.ResourceManager.InstantiatePrefabSync(Constants.Prefab_UI_Scene_Common).GetComponent<UICommonScene>();
        commonSceneUI.transform.SetParent(rootTransform);
        commonSceneUI.Initialize();
        if (commonSceneUI.gameObject.activeSelf == false)
        {
            commonSceneUI.gameObject.SetActive(true);
        }

        canInput = true;
    }

    public void Update()
    {
        if(canInput)
        {
            // !! Game Scene UI - Switchable Panel
            if (gameSceneUI != null)
            {
                if (Input.GetKeyDown(KeyCode.I))
                    SwitchUserPanel(gameSceneUI.InventoryPanel);

                if (Input.GetKeyDown(KeyCode.T))
                    SwitchUserPanel(gameSceneUI.StatusPanel);

                if (Input.GetKeyDown(KeyCode.Q))
                    SwitchUserPanel(gameSceneUI.QuestPanel);
            }

            // !! ESC
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (activeUserPanel != null)
                {
                    SwitchUserPanel(activeUserPanel);
                }

                else if (commonSceneUI != null)
                {
                    TogglePanel(commonSceneUI.OptionPanel);
                }
            }
        }
    }

    #region Panel
    public void SwitchUserPanel(UIPanel panel)
    {
        if (activeUserPanel == null)
        {
            activeUserPanel = panel;
            OpenPanel(activeUserPanel);
        }
        else if (activeUserPanel == panel)
        {
            ClosePanel(activeUserPanel);
            activeUserPanel = null;
        }
        else
        {
            ClosePanel(activeUserPanel);
            activeUserPanel = panel;
            OpenPanel(activeUserPanel);
        }
    }

    public void OpenPanel(UIPanel panel)
    {
        if (panel.gameObject.activeSelf == false)
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
    #endregion

    #region Audio
    public void PlayUserPanelOpenSFX()
    {
        Managers.AudioManager.PlaySFX("Audio_Popup_Open");
    }
    public void PlayUserPanelCloseSFX()
    {
        Managers.AudioManager.PlaySFX("Audio_Popup_Close");
    }
    #endregion

    #region Property
    public GameObject RootObject
    {
        get
        {
            GameObject rootObject = GameObject.Find("@UI Root");
            if (rootObject == null)
            {
                rootObject = new GameObject { name = "@UI Root" };
            }
            return rootObject;
        }
    }
    public UICommonScene CommonSceneUI { get { return commonSceneUI; } }
    public UISelectionScene SelectionSceneUI { get { return selectionSceneUI; } set { selectionSceneUI = value; } }
    public UIGameScene GameSceneUI { get { return gameSceneUI; } set { gameSceneUI = value; } }
    public UIPanel ActiveUserPanel { get { return activeUserPanel; } }
    #endregion
}