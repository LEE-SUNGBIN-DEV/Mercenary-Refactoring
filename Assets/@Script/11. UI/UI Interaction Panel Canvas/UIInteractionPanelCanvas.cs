using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteractionPanelCanvas : UIBaseCanvas
{
    private IFocusPanel[] focusPanels;
    [SerializeField] private IFocusPanel currentFocusPanel;

    private InventoryPanel inventoryPanel;
    private StatusPanel statusPanel;
    private SkillNodePanel skillNodePanel;
    private QuestPanel questPanel;
    private OptionPanel optionPanel;

    private ResponseTracePanel responseTracePanel;
    private ResponsePointPanel responsePointPanel;
    private NPCPanel npcPanel;
    private DialogueSelectionPanel dialogueSelectionPanel;
    private StorePanel storePanel;

    protected override void Awake()
    {
        base.Awake();
        canvas.sortingOrder = 2;

        focusPanels = GetComponentsInChildren<IFocusPanel>(true);
        for (int i = 0; i < focusPanels.Length; i++)
        {
            focusPanels[i].OnOpenFocusPanel -= OnOpenFocusPanel;
            focusPanels[i].OnOpenFocusPanel += OnOpenFocusPanel;
            focusPanels[i].OnCloseFocusPanel -= OnCloseFocusPanel;
            focusPanels[i].OnCloseFocusPanel += OnCloseFocusPanel;
        }

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

        responseTracePanel = GetComponentInChildren<ResponseTracePanel>(true);
        responseTracePanel.Initialize();
        responsePointPanel = GetComponentInChildren<ResponsePointPanel>(true);
        responsePointPanel.Initialize();
        npcPanel = GetComponentInChildren<NPCPanel>(true);
        npcPanel.Initialize();
        dialogueSelectionPanel = GetComponentInChildren<DialogueSelectionPanel>(true);
        dialogueSelectionPanel.Initialize();

        storePanel = GetComponentInChildren<StorePanel>(true);
    }

    private void OnOpenFocusPanel(IFocusPanel focusPanel)
    {
        if (currentFocusPanel == null)
        {
            currentFocusPanel = focusPanel;
            return;
        }
        if (currentFocusPanel != null && currentFocusPanel != focusPanel)
        {
            currentFocusPanel?.ClosePanel();
            currentFocusPanel = null;
            currentFocusPanel = focusPanel;
        }
    }
    private void OnCloseFocusPanel(IFocusPanel focusPanel)
    {
        if (currentFocusPanel == focusPanel)
        {
            currentFocusPanel = null;
        }
    }
    public void CloseCurrentFocusPanel()
    {
        if (currentFocusPanel != null)
        {
            currentFocusPanel?.ClosePanel();
            currentFocusPanel = null;
        }
    }

    #region Property
    public StatusPanel StatusPanel { get { return statusPanel; } }
    public SkillNodePanel SkillNodePanel { get { return skillNodePanel; } }
    public InventoryPanel InventoryPanel { get { return inventoryPanel; } }
    public QuestPanel QuestPanel { get { return questPanel; } }
    public OptionPanel OptionPanel { get { return optionPanel; } }

    public ResponseTracePanel ResponseTracePanel { get { return responseTracePanel; } }
    public ResponsePointPanel ResponsePointPanel { get { return responsePointPanel; } }
    public NPCPanel NPCPanel { get { return npcPanel; } }
    public DialogueSelectionPanel DialogueSelectionPanel { get { return dialogueSelectionPanel; } }
    public StorePanel StorePanel { get { return storePanel; } }
    #endregion
}
