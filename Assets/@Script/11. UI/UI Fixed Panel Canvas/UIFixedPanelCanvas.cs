using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIFixedPanelCanvas : UIBaseCanvas
{
    private CharacterPanel characterPanel;
    private QuickSlotPanel quickSlotPanel;
    private SceneNamePanel sceneNamePanel;
    private EnemyPanel enemyPanel;
    private DropPanel dropPanel;
    private InteractionPanel interactionPanel;
    private ConfirmPanel confirmPanel;
    private CompetePanel competePanel;
    private CenterNoticePanel centerNoticePanel;
    private DiePanel diePanel;

    protected override void Awake()
    {
        base.Awake();
        canvas.sortingOrder = 1;

        characterPanel = GetComponentInChildren<CharacterPanel>(true);
        characterPanel.Initialize();
        quickSlotPanel = GetComponentInChildren<QuickSlotPanel>(true);
        sceneNamePanel = GetComponentInChildren<SceneNamePanel>(true);
        sceneNamePanel.Initialize();
        enemyPanel = GetComponentInChildren<EnemyPanel>(true);
        enemyPanel.Initialize();
        dropPanel = GetComponentInChildren<DropPanel>(true);
        dropPanel.Initialize();
        interactionPanel = GetComponentInChildren<InteractionPanel>(true);
        interactionPanel.Initialize();
        competePanel = GetComponentInChildren<CompetePanel>(true);
        competePanel.Initialize();
        confirmPanel = GetComponentInChildren<ConfirmPanel>(true);
        confirmPanel.Initialize();
        centerNoticePanel = GetComponentInChildren<CenterNoticePanel>(true);
        centerNoticePanel.Initialize();
        diePanel = GetComponentInChildren<DiePanel>(true);
        diePanel.Initialize();
    }

    #region Property
    public CharacterPanel CharacterPanel { get { return characterPanel; } }
    public EnemyPanel EnemyPanel { get { return enemyPanel; } }

    public SceneNamePanel SceneNamePanel { get { return sceneNamePanel; } }
    public CompetePanel CompetePanel { get { return competePanel; } }
    public ConfirmPanel ConfirmPanel { get { return confirmPanel; } }
    public CenterNoticePanel CenterNoticePanel { get { return centerNoticePanel; } }
    public DiePanel DiePanel { get { return diePanel; } }
    public InteractionPanel InteractionPanel { get { return interactionPanel; } }
    #endregion
}
