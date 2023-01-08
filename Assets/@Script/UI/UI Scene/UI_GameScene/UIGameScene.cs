using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameScene : UIBaseScene
{
    // Panel
    private UserPanel userPanel;
    private DialoguePanel dialoguePanel;
    private MonsterPanel monsterPanel;
    private MapPanel mapPanel;
    private CompetePanel competePanel;

    // Popup
    private DiePopup diePopup;
    private InventoryPopup inventoryPopup;
    private StatusPopup statusPopup;
    private HelpPopup helpPopup;
    private QuestPopup questPopup;
    private StorePopup storePopup;
    private CampaignPopup campaignPopup;

    public void Initialize(CharacterData characterData)
    {
        if (isInitialized == true)
        {
            Debug.Log($"{this}: Already Initialized.");
            return;
        }
        isInitialized = true;

        // Get Component
        userPanel = GetComponentInChildren<UserPanel>(true);
        dialoguePanel = GetComponentInChildren<DialoguePanel>(true);
        monsterPanel = GetComponentInChildren<MonsterPanel>(true);
        mapPanel = GetComponentInChildren<MapPanel>(true);
        competePanel = GetComponentInChildren<CompetePanel>(true);

        diePopup = GetComponentInChildren<DiePopup>(true);
        inventoryPopup = GetComponentInChildren<InventoryPopup>(true);
        statusPopup = GetComponentInChildren<StatusPopup>(true);
        helpPopup = GetComponentInChildren<HelpPopup>(true);
        questPopup = GetComponentInChildren<QuestPopup>(true);
        storePopup = GetComponentInChildren<StorePopup>(true);
        campaignPopup = GetComponentInChildren<CampaignPopup>(true);

        // Initialize
        userPanel.Initialize(characterData);
        dialoguePanel.Initialize(characterData);
        competePanel.Initialize();

        diePopup.Initialize();
        inventoryPopup.Initialize(characterData.InventoryData);
        statusPopup.Initialize(characterData);
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
            TogglePopup(inventoryPopup);

        if (Input.GetKeyDown(KeyCode.T))
            TogglePopup(statusPopup);

        if (Input.GetKeyDown(KeyCode.Q))
            TogglePopup(questPopup);

        if (Input.GetKeyDown(KeyCode.H))
            TogglePopup(helpPopup);
    }

    public void OpenCompetePanel() { OpenPanel(competePanel); }
    public void CloseCompetePanel() { ClosePanel(competePanel); }

    #region Property
    public DiePopup DiePopup { get { return diePopup; } }
    public InventoryPopup InventoryPopup { get { return inventoryPopup; } }
    public StatusPopup StatusPopup { get { return statusPopup; } }
    public HelpPopup HelpPopup { get { return helpPopup; } }
    public QuestPopup QuestPopup { get { return questPopup; } }
    public StorePopup StorePopup { get { return storePopup; } }
    public CampaignPopup CampaignPopup { get { return campaignPopup; } }

    public UserPanel UserPanel { get { return userPanel; } }
    public DialoguePanel DialoguePanel { get { return dialoguePanel; } }
    public MonsterPanel MonsterPanel { get { return monsterPanel; } }
    public MapPanel MapPanel { get { return mapPanel; } }
    #endregion
}
