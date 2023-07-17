using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager
{
    public event UnityAction<bool> OnActiveUserPanel;

    private UICommonScene commonSceneUI;
    private UISelectionScene selectionSceneUI;
    private UIGameScene gameSceneUI;

    private UIPanel activeUserPanel;
    private bool isInteracting;

    public void Initialize(GameObject rootObject)
    {
        // Set Resolution
        Screen.SetResolution(Managers.DataManager.PlayerData.OptionData.ScreenWidth,
            Managers.DataManager.PlayerData.OptionData.ScreenHeight,
            Managers.DataManager.PlayerData.OptionData.IsFullScreen,
            Managers.DataManager.PlayerData.OptionData.ScreenRefreshRate);

        commonSceneUI = Managers.ResourceManager.InstantiatePrefabSync(Constants.Prefab_UI_Scene_Common).GetComponent<UICommonScene>();
        commonSceneUI.transform.SetParent(rootObject.transform);
        commonSceneUI.Initialize();
        if (commonSceneUI.gameObject.activeSelf == false)
        {
            commonSceneUI.gameObject.SetActive(true);
        }
    }

    public void Update()
    {
        Managers.InputManager.UpdateUIInputs();

        // !! Scene
        switch (Managers.SceneManagerCS.CurrentScene.SceneType)
        {
            case SCENE_TYPE.Title:
            case SCENE_TYPE.Selection:
                break;

            case SCENE_TYPE.Game:
                if (isInteracting)
                    return;

                if (Managers.InputManager.InventoryDown)
                    SwitchOrToggleUserPanel(gameSceneUI.InventoryPanel);

                if (Managers.InputManager.SkillDown)
                    SwitchOrToggleUserPanel(gameSceneUI.SkillPanel);

                if (Managers.InputManager.StatusDown)
                    SwitchOrToggleUserPanel(gameSceneUI.StatusPanel);

                if (Managers.InputManager.QuestDown)
                    SwitchOrToggleUserPanel(gameSceneUI.QuestPanel);

                break;

            default:
                break;
        }

        // !! Common
        if (Managers.InputManager.EscDown)
        {
            if (activeUserPanel != null)
                SwitchOrToggleUserPanel(activeUserPanel);

            else if (commonSceneUI != null)
                SwitchOrToggleUserPanel(commonSceneUI.OptionPanel);
        }
    }

    public void ActiveInteraction(bool isActive)
    {
        isInteracting = isActive;
    }

    #region Panel
    public void CloseActiveUserPanel()
    {
        if(activeUserPanel != null)
        {
            ClosePanel(activeUserPanel);
            activeUserPanel = null;
        }
    }

    public void SwitchOrToggleUserPanel(UIPanel panel)
    {
        if (activeUserPanel == null)
        {
            activeUserPanel = panel;
            OpenPanel(activeUserPanel);

            OnActiveUserPanel?.Invoke(true);
        }
        else if (activeUserPanel == panel)
        {
            ClosePanel(activeUserPanel);
            activeUserPanel = null;

            OnActiveUserPanel?.Invoke(false);
        }
        else
        {
            ClosePanel(activeUserPanel);
            activeUserPanel = panel;
            OpenPanel(activeUserPanel);

            OnActiveUserPanel?.Invoke(true);
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
            panel.FadeOut(Constants.TIME_UI_PANEL_DEFAULT_FADE, () =>
            {
                panel.gameObject.SetActive(false);
            });
        }
    }

    public void TogglePanel(UIPanel panel)
    {
        if (panel.gameObject.activeSelf)
            ClosePanel(panel);

        else
            OpenPanel(panel);
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
    public bool IsInteracting { get { return isInteracting; } }
    #endregion
}