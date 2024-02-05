using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    private UIScenePanelCanvas uiScenePanelCanvas;
    private UIFixedPanelCanvas uiFixedPanelCanvas;
    private UIInteractionPanelCanvas uiInteractionPanelCanvas;
    private UISystemPanelCanvas uiSystemPanelCanvas;

    [Header("Cursor")]
    private CURSOR_MODE cursorMode;

    public void Initialize()
    {
        // Set Cursor
        Managers.ResourceManager.LoadResourceAsync<Texture2D>(Constants.SPRITE_CURSOR_DEFAULT, SetCursorTexture);
        SetCursorMode(CURSOR_MODE.VISIBLE);

        // Set Resolution
        Screen.SetResolution(
            Managers.DataManager.PlayerData.OptionData.ScreenWidth,
            Managers.DataManager.PlayerData.OptionData.ScreenHeight,
            Managers.DataManager.PlayerData.OptionData.IsFullScreen,
            Managers.DataManager.PlayerData.OptionData.ScreenRefreshRate);

        if (Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_UI_SYSTEM_PANEL_CANVAS).TryGetComponent(out uiSystemPanelCanvas))
        {
            uiSystemPanelCanvas.transform.SetParent(transform);
            if (uiSystemPanelCanvas.gameObject.activeSelf == false)
            {
                uiSystemPanelCanvas.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (Managers.InputManager.UIToggleCursorButton.WasPressedThisFrame())
            ToggleCursorMode();

        if (Managers.InputManager.UIEscButton.WasPressedThisFrame())
            UIInteractionPanelCanvas.CloseCurrentFocusPanel();

        if (Managers.InputManager.UIOptionPanelButton.WasPressedThisFrame())
            UIInteractionPanelCanvas.OptionPanel.TogglePanel();

        if (Managers.InputManager.UIInventoryPanelButton.WasPressedThisFrame())
            UIInteractionPanelCanvas.InventoryPanel.TogglePanel();

        if (Managers.InputManager.UIStatusPanelButton.WasPressedThisFrame())
            UIInteractionPanelCanvas.StatusPanel.TogglePanel();

        if (Managers.InputManager.UISkillPanelButton.WasPressedThisFrame())
            UIInteractionPanelCanvas.SkillNodePanel.TogglePanel();
    }

    #region Cursor Function
    public void SetCursorTexture(Texture2D texture)
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }

    public void SetCursorMode(CURSOR_MODE cursorMode)
    {
        switch (cursorMode)
        {
            case CURSOR_MODE.INVISIBLE:
                {
                    this.cursorMode = CURSOR_MODE.INVISIBLE;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                }
            case CURSOR_MODE.VISIBLE:
                {
                    this.cursorMode = CURSOR_MODE.VISIBLE;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                }
        }
    }

    public void ToggleCursorMode()
    {
        if (cursorMode == CURSOR_MODE.INVISIBLE)
        {
            SetCursorMode(CURSOR_MODE.VISIBLE);
        }

        else
        {
            SetCursorMode(CURSOR_MODE.INVISIBLE);
        }
    }
    #endregion

    #region Property
    public UIScenePanelCanvas UIScenePanelCanvas
    {
        get
        {
            if(uiScenePanelCanvas == null)
            {
                if (Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_UI_SCENE_PANEL_CANVAS).TryGetComponent(out uiScenePanelCanvas))
                {
                    if (uiScenePanelCanvas.gameObject.activeSelf == false)
                    {
                        uiScenePanelCanvas.gameObject.SetActive(true);
                    }
                }
            }
            return uiScenePanelCanvas;
        }
    }
    public UIFixedPanelCanvas UIFixedPanelCanvas
    {
        get
        {
            if (uiFixedPanelCanvas == null)
            {
                if (Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_UI_FIXED_PANEL_CANVAS).TryGetComponent(out uiFixedPanelCanvas))
                {
                    if (uiFixedPanelCanvas.gameObject.activeSelf == false)
                    {
                        uiFixedPanelCanvas.gameObject.SetActive(true);
                    }
                }
            }
            return uiFixedPanelCanvas;
        }
    }
    public UIInteractionPanelCanvas UIInteractionPanelCanvas
    {
        get
        {
            if (uiInteractionPanelCanvas == null)
            {
                if (Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_UI_INTERACTION_PANEL_CANVAS).TryGetComponent(out uiInteractionPanelCanvas))
                {
                    if (uiInteractionPanelCanvas.gameObject.activeSelf == false)
                    {
                        uiInteractionPanelCanvas.gameObject.SetActive(true);
                    }
                }
            }
            return uiInteractionPanelCanvas;
        }
    }
    public UISystemPanelCanvas UISystemPanelCanvas { get { return uiSystemPanelCanvas; } }
    #endregion
}