using UnityEngine;

public class UIGameScene : UIBaseScene
{
    // Panel
    private UserPanel userPanel;
    private StatusPanel statusPanel;
    private SkillPanel skillPanel;
    private InventoryPanel inventoryPanel;
    private ResonancePointPanel resonancePointPanel;
    private QuestPanel questPanel;
    private InteractionPanel interactionPanel;
    private DialoguePanel dialoguePanel;
    private EnemyPanel enemyPanel;
    private SceneNamePanel mapPanel;
    private CompetePanel competePanel;
    private CenterNoticePanel centerNoticePanel;
    private DiePanel diePanel;

    // Popup
    private StorePopup storePopup;

    public void Initialize(CharacterData characterData)
    {
        base.Initialize();
        Managers.UIManager.GameSceneUI = this;

        // Get Panels
        userPanel = GetComponentInChildren<UserPanel>(true);
        statusPanel = GetComponentInChildren<StatusPanel>(true);
        skillPanel = GetComponentInChildren<SkillPanel>(true);
        inventoryPanel = GetComponentInChildren<InventoryPanel>(true);
        resonancePointPanel = GetComponentInChildren<ResonancePointPanel>(true);
        questPanel = GetComponentInChildren<QuestPanel>(true);
        interactionPanel = GetComponentInChildren<InteractionPanel>(true);
        dialoguePanel = GetComponentInChildren<DialoguePanel>(true);
        enemyPanel = GetComponentInChildren<EnemyPanel>(true);
        mapPanel = GetComponentInChildren<SceneNamePanel>(true);
        competePanel = GetComponentInChildren<CompetePanel>(true);
        centerNoticePanel = GetComponentInChildren<CenterNoticePanel>(true);
        diePanel = GetComponentInChildren<DiePanel>(true);

        // Initialize
        userPanel.Initialize(characterData);
        statusPanel.Initialize(characterData);
        skillPanel.Initialize(characterData);
        inventoryPanel.Initialize(characterData);
        resonancePointPanel.Initialize(characterData);
        interactionPanel.Initialize();
        dialoguePanel.Initialize(characterData);
        enemyPanel.Initialize();
        competePanel.Initialize();
        centerNoticePanel.Initialize();
        diePanel.Initialize();
        //questPanel.Initialize();

        storePopup = GetComponentInChildren<StorePopup>(true);
        storePopup.Initialize();

        Managers.SpecialCombatManager.OnStartCompete += OpenCompetePanel;
        Managers.SpecialCombatManager.OnEndCompete += CloseCompetePanel;

        Managers.UIManager.OpenPanel(userPanel);
    }

    public void OpenCompetePanel() { Managers.UIManager.OpenPanel(competePanel); }
    public void CloseCompetePanel() { Managers.UIManager.ClosePanel(competePanel); }

    #region Property
    public UserPanel UserPanel { get { return userPanel; } }
    public StatusPanel StatusPanel { get { return statusPanel; } }
    public SkillPanel SkillPanel { get { return skillPanel; } }
    public InventoryPanel InventoryPanel { get { return inventoryPanel; } }
    public ResonancePointPanel ResonancePointPanel { get { return resonancePointPanel; } }
    public QuestPanel QuestPanel { get { return questPanel; } }
    public InteractionPanel InteractionPanel { get { return interactionPanel; } }
    public DialoguePanel DialoguePanel { get { return dialoguePanel; } }
    public EnemyPanel EnemyPanel { get { return enemyPanel; } }
    public SceneNamePanel MapPanel { get { return mapPanel; } }
    public CenterNoticePanel CenterNoticePanel { get { return centerNoticePanel; } }
    public DiePanel DiePanel { get { return diePanel; } }

    public StorePopup StorePopup { get { return storePopup; } }
    #endregion
}
