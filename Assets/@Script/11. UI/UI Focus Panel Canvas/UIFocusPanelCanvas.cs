using UnityEngine;
using UnityEngine.Events;

public class UIFocusPanelCanvas : UIBaseCanvas
{
    private IFocusPanel activedFocusPanel;

    // Panel
    private InventoryPanel inventoryPanel;
    private StatusPanel statusPanel;
    private SkillNodePanel skillNodePanel;
    private QuestPanel questPanel;
    private OptionPanel optionPanel;

    #region Private
    protected override void Awake()
    {
        base.Awake();
        canvas.sortingOrder = 2;

        inventoryPanel = GetComponentInChildren<InventoryPanel>(true);
        inventoryPanel.Initialize();
        statusPanel = GetComponentInChildren<StatusPanel>(true);
        statusPanel.Initialize();
        skillNodePanel = GetComponentInChildren<SkillNodePanel>(true);
        skillNodePanel.Initialize();
        questPanel = GetComponentInChildren<QuestPanel>(true);
        questPanel.Initialize();
        optionPanel = GetComponentInChildren<OptionPanel>(true);
        optionPanel.Initialize();
    }
    #endregion

    public void TogglePanel(IFocusPanel focusPanel)
    {

    }

    public void SwitchFocusPanel(IFocusPanel focusPanel)
    {
        if (activedFocusPanel == focusPanel)
        {
            activedFocusPanel.CloseFocusPanel();
            activedFocusPanel = null;
            Managers.InputManager.SwitchInputMode(CHARACTER_INPUT_MODE.ALL);
        }
        else
        {
            activedFocusPanel?.CloseFocusPanel();
            activedFocusPanel = focusPanel;
            activedFocusPanel.OpenFocusPanel();
            Managers.InputManager.SwitchInputMode(CHARACTER_INPUT_MODE.UI);
        }
    }

    public void CloseActivedFocusPanel()
    {
        if(activedFocusPanel != null)
        {
            activedFocusPanel?.CloseFocusPanel();
            activedFocusPanel = null;
            Managers.InputManager.SwitchInputMode(CHARACTER_INPUT_MODE.ALL);
        }
    }

    #region Property
    public StatusPanel StatusPanel { get { return statusPanel; } }
    public SkillNodePanel SkillNodePanel { get { return skillNodePanel; } }
    public InventoryPanel InventoryPanel { get { return inventoryPanel; } }
    public QuestPanel QuestPanel { get { return questPanel; } }
    public OptionPanel OptionPanel { get { return optionPanel; } }
    #endregion
}
