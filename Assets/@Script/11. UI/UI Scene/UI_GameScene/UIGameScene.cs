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
    private ResonancePointPanel resonancePointPanel;
    private QuestPanel questPanel;
    private InteractionPanel interactionPanel;
    private DialoguePanel dialoguePanel;
    private EnemyPanel enemyPanel;
    private MapPanel mapPanel;
    private CampaignPopup campaignPanel;
    private CompetePanel competePanel;

    // Popup
    private DiePopup diePopup;
    private StorePopup storePopup;

    public void Initialize(CharacterData characterData)
    {
        base.Initialize();
        currentUserPanel = null;

        // Get Component
        userPanel = GetComponentInChildren<UserPanel>(true);
        statusPanel = GetComponentInChildren<StatusPanel>(true);
        inventoryPanel = GetComponentInChildren<InventoryPanel>(true);
        resonancePointPanel = GetComponentInChildren<ResonancePointPanel>(true);
        questPanel = GetComponentInChildren<QuestPanel>(true);
        interactionPanel = GetComponentInChildren<InteractionPanel>(true);
        dialoguePanel = GetComponentInChildren<DialoguePanel>(true);
        enemyPanel = GetComponentInChildren<EnemyPanel>(true);
        mapPanel = GetComponentInChildren<MapPanel>(true);
        competePanel = GetComponentInChildren<CompetePanel>(true);

        diePopup = GetComponentInChildren<DiePopup>(true);
        storePopup = GetComponentInChildren<StorePopup>(true);
        campaignPanel = GetComponentInChildren<CampaignPopup>(true);

        // Initialize
        userPanel.Initialize(characterData);
        statusPanel.Initialize(characterData);
        inventoryPanel.Initialize(characterData);
        resonancePointPanel.Initialize(characterData);
        interactionPanel.Initialize();
        dialoguePanel.Initialize(characterData);
        competePanel.Initialize();

        diePopup.Initialize();
        //questPanel.Initialize();
        storePopup.Initialize();
        campaignPanel.Initialize(characterData);

        Managers.CompeteManager.OnStartCompete += OpenCompetePanel;
        Managers.CompeteManager.OnEndCompete += CloseCompetePanel;

        OpenPanel(userPanel);
    }

    private void OnDestroy()
    {
        Managers.CompeteManager.OnStartCompete -= OpenCompetePanel;
        Managers.CompeteManager.OnEndCompete -= CloseCompetePanel;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            SwitchUserPanel(inventoryPanel);

        if (Input.GetKeyDown(KeyCode.T))
            SwitchUserPanel(statusPanel);

        if (Input.GetKeyDown(KeyCode.Q))
            SwitchUserPanel(questPanel);

        if (Input.GetKeyDown(KeyCode.Escape) && currentUserPanel != null)
        {
            ClosePanel(currentUserPanel);
        }
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
    public UserPanel UserPanel { get { return userPanel; } }
    public StatusPanel StatusPanel { get { return statusPanel; } }
    public InventoryPanel InventoryPanel { get { return inventoryPanel; } }
    public ResonancePointPanel ResonancePointPanel { get { return resonancePointPanel; } }
    public QuestPanel QuestPanel { get { return questPanel; } }
    public InteractionPanel InteractionPanel { get { return interactionPanel; } }
    public DialoguePanel DialoguePanel { get { return dialoguePanel; } }
    public EnemyPanel EnemyPanel { get { return enemyPanel; } }
    public MapPanel MapPanel { get { return mapPanel; } }

    public CampaignPopup CampaignPopup { get { return campaignPanel; } }
    public StorePopup StorePopup { get { return storePopup; } }
    public DiePopup DiePopup { get { return diePopup; } }
    #endregion
}
