using UnityEngine;

public class UIGameScene : UIBaseScene
{
    // Panel
    private UserPanel userPanel;
    private StatusPanel statusPanel;
    private InventoryPanel inventoryPanel;
    private ResonancePointPanel resonancePointPanel;
    private QuestPanel questPanel;
    private InteractionPanel interactionPanel;
    private DialoguePanel dialoguePanel;
    private EnemyPanel enemyPanel;
    private SceneNamePanel mapPanel;
    private CampaignPopup campaignPanel;
    private CompetePanel competePanel;

    // Popup
    private DiePopup diePopup;
    private StorePopup storePopup;

    public void Initialize(CharacterData characterData)
    {
        base.Initialize();
        Managers.UIManager.GameSceneUI = this;

        // Get Panels
        userPanel = GetComponentInChildren<UserPanel>(true);
        statusPanel = GetComponentInChildren<StatusPanel>(true);
        inventoryPanel = GetComponentInChildren<InventoryPanel>(true);
        resonancePointPanel = GetComponentInChildren<ResonancePointPanel>(true);
        questPanel = GetComponentInChildren<QuestPanel>(true);
        interactionPanel = GetComponentInChildren<InteractionPanel>(true);
        dialoguePanel = GetComponentInChildren<DialoguePanel>(true);
        enemyPanel = GetComponentInChildren<EnemyPanel>(true);
        mapPanel = GetComponentInChildren<SceneNamePanel>(true);
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

        Managers.UIManager.OpenPanel(userPanel);
    }

    public void OpenCompetePanel() { Managers.UIManager.OpenPanel(competePanel); }
    public void CloseCompetePanel() { Managers.UIManager.ClosePanel(competePanel); }

    #region Property
    public UserPanel UserPanel { get { return userPanel; } }
    public StatusPanel StatusPanel { get { return statusPanel; } }
    public InventoryPanel InventoryPanel { get { return inventoryPanel; } }
    public ResonancePointPanel ResonancePointPanel { get { return resonancePointPanel; } }
    public QuestPanel QuestPanel { get { return questPanel; } }
    public InteractionPanel InteractionPanel { get { return interactionPanel; } }
    public DialoguePanel DialoguePanel { get { return dialoguePanel; } }
    public EnemyPanel EnemyPanel { get { return enemyPanel; } }
    public SceneNamePanel MapPanel { get { return mapPanel; } }

    public CampaignPopup CampaignPopup { get { return campaignPanel; } }
    public StorePopup StorePopup { get { return storePopup; } }
    public DiePopup DiePopup { get { return diePopup; } }
    #endregion
}
