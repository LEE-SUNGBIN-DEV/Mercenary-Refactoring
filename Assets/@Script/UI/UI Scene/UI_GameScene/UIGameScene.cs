using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameScene : UIBaseScene
{
    private UIPanel currentUserPanel;

    // Panel
    private UserPanel userPanel;
    private StatusPanel statusPanel;
    private InventoryPanel inventoryPanel;
    private QuestPanel questPopup;
    private DialoguePanel dialoguePanel;
    private EnemyPanel enemyPanel;
    private MapPanel mapPanel;
    private CampaignPopup campaignPopup;
    private CompetePanel competePanel;

    // Popup
    private DiePopup diePopup;
    private HelpPopup helpPopup;
    private StorePopup storePopup;

    public void Initialize(CharacterData characterData)
    {
        if (isInitialized == true)
        {
            Debug.Log($"{this}: Already Initialized.");
            return;
        }
        isInitialized = true;
        currentUserPanel = null;

        // Get Component
        userPanel = GetComponentInChildren<UserPanel>(true);
        statusPanel = GetComponentInChildren<StatusPanel>(true);
        inventoryPanel = GetComponentInChildren<InventoryPanel>(true);
        questPopup = GetComponentInChildren<QuestPanel>(true);
        dialoguePanel = GetComponentInChildren<DialoguePanel>(true);
        enemyPanel = GetComponentInChildren<EnemyPanel>(true);
        mapPanel = GetComponentInChildren<MapPanel>(true);
        competePanel = GetComponentInChildren<CompetePanel>(true);

        diePopup = GetComponentInChildren<DiePopup>(true);
        helpPopup = GetComponentInChildren<HelpPopup>(true);
        storePopup = GetComponentInChildren<StorePopup>(true);
        campaignPopup = GetComponentInChildren<CampaignPopup>(true);

        // Initialize
        userPanel.Initialize(characterData);
        statusPanel.Initialize(characterData);
        inventoryPanel.Initialize(characterData);
        dialoguePanel.Initialize(characterData);
        competePanel.Initialize();

        diePopup.Initialize();
        helpPopup.Initialize();
        //questPopup.Initialize();
        storePopup.Initialize();
        campaignPopup.Initialize(characterData);

        // Add Event
        Managers.CompeteManager.OnStartCompete -= OpenCompetePanel;
        Managers.CompeteManager.OnStartCompete += OpenCompetePanel;

        Managers.CompeteManager.OnEndCompete -= CloseCompetePanel;
        Managers.CompeteManager.OnEndCompete += CloseCompetePanel;

        OpenPanel(userPanel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            SwitchUserPanel(inventoryPanel);

        if (Input.GetKeyDown(KeyCode.T))
            SwitchUserPanel(statusPanel);

        if (Input.GetKeyDown(KeyCode.Q))
            SwitchUserPanel(questPopup);

        if (Input.GetKeyDown(KeyCode.H))
            TogglePopup(helpPopup);
    }

    public void SwitchUserPanel(UIPanel panel)
    {
        if(currentUserPanel == null)
        {
            currentUserPanel = panel;
            OpenPanel(currentUserPanel);
        }
        else if(currentUserPanel == panel)
        {
            ClosePanel(currentUserPanel);
            currentUserPanel = null;
        }
        else
        {
            ClosePanel(currentUserPanel);
            currentUserPanel = panel;
            OpenPanel(currentUserPanel);
        }
    }

    public void OpenCompetePanel() { OpenPanel(competePanel); }
    public void CloseCompetePanel() { ClosePanel(competePanel); }

    #region Property
    public DiePopup DiePopup { get { return diePopup; } }
    public InventoryPanel InventoryPopup { get { return inventoryPanel; } }
    public StatusPanel StatusPopup { get { return statusPanel; } }
    public HelpPopup HelpPopup { get { return helpPopup; } }
    public QuestPanel QuestPopup { get { return questPopup; } }
    public StorePopup StorePopup { get { return storePopup; } }
    public CampaignPopup CampaignPopup { get { return campaignPopup; } }

    public UserPanel UserPanel { get { return userPanel; } }
    public DialoguePanel DialoguePanel { get { return dialoguePanel; } }
    public EnemyPanel EnemyPanel { get { return enemyPanel; } }
    public MapPanel MapPanel { get { return mapPanel; } }
    #endregion
}
