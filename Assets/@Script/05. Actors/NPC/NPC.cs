using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum NPC_TYPE
{
    NOMRAL,
    BLACK_SMITH
}

public class NPC : BaseActor, IInteractableObject
{
    public event UnityAction<NPC> OnFinishTalk;

    [Header("NPC")]
    [SerializeField] protected NPCData npcData;
    [SerializeField] protected List<Quest> questList;
    [SerializeField] protected bool isTalking;
    protected NPCMoveController moveController;

    [Header("IInteractable")]
    protected PlayerCharacter targetCharacter;
    protected float distance;

    [Header("Mark Objects")]
    [SerializeField] protected GameObject enabledQuestMark;
    [SerializeField] protected GameObject completedQuestMark;

    public virtual void Initialize(NPCData npcData)
    {
        this.npcData = npcData;
        isTalking = false;
    }

    #region Unity Functions
    public virtual void OnEnable()
    {
        UpdateQuestList();
    }
    public virtual void OnDisable()
    {
        ClearQuestList();
    }
    private void Update()
    {
        if (targetCharacter != null && npcData.isTalkable)
        {
            distance = Vector3.SqrMagnitude(targetCharacter.transform.position - transform.position);

            targetCharacter.InteractionController.EnterDetection(this, targetCharacter);
            targetCharacter.InteractionController.UpdateDetection(this, targetCharacter);
            targetCharacter.InteractionController.UpdateInteraction(this, targetCharacter);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (targetCharacter == null && other.TryGetComponent(out PlayerCharacter character) && npcData.isTalkable)
        {
            targetCharacter = character;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacter character) && character == targetCharacter)
        {
            targetCharacter.InteractionController.ExitInteraction(this, targetCharacter);
            targetCharacter = null;
        }
    }
    #endregion

    #region IInteractableObject Functions
    // Detection Functions
    public void EnterDetection(PlayerCharacter character)
    {
        Managers.UIManager.UIFixedPanelCanvas.InteractionPanel.OpenPanel(Managers.DataManager.TextTable["TEXT_CONVERSATION"].textContent);
    }
    public void UpdateDetection(PlayerCharacter character)
    {
        if (Managers.InputManager.InteractionEnterButton.WasPressedThisFrame())
        {
            character.InteractionController.EnterInteraction(this, character);
        }
    }
    public void ExitDetection(PlayerCharacter character)
    {
        Managers.UIManager.UIFixedPanelCanvas.InteractionPanel.ClosePanel();
    }

    // Interaction Functions
    public void EnterInteraction(PlayerCharacter character)
    {
        if (GetDefaultDialogueData() != null)
        {
            Managers.UIManager.UIInteractionPanelCanvas.NPCPanel.OpenPanel(this);
            isTalking = true;
        }
        else
        {
            character.InteractionController.ExitInteraction(this, character);
        }
    }
    public void UpdateInteraction(PlayerCharacter character)
    {
        if (Managers.InputManager.InteractionUpdateButton.WasPressedThisFrame() && !Managers.UIManager.UIInteractionPanelCanvas.DialogueSelectionPanel.IsSelecting())
        {
            Managers.UIManager.UIInteractionPanelCanvas.NPCPanel.UpdateDialogue(character);
        }

        if (Managers.InputManager.InteractionExitButton.WasPressedThisFrame())
        {
            character.InteractionController.ExitInteraction(this, character);
        }
    }
    public void ExitInteraction(PlayerCharacter character)
    {
        Managers.UIManager.UIInteractionPanelCanvas.DialogueSelectionPanel.ClosePanel();
        Managers.UIManager.UIInteractionPanelCanvas.NPCPanel.ClosePanel();
        isTalking = false;
    }
    #endregion

    public DialogueData GetDefaultDialogueData()
    {
        if (Managers.DataManager.DialogueTable.TryGetValue($"{npcData.npcID}_DEFAULT_0", out DialogueData defaultDialogueData))
        {
            return defaultDialogueData;
        }
        return null;
    }
    public void UpdateQuestList()
    {
        questList.Clear();
    }
    public void ClearQuestList()
    {
        questList.Clear();
    }

    public void SetQuestMark()
    {
        if (questList.Count > 0)
            enabledQuestMark.SetActive(true);
        else
            enabledQuestMark.SetActive(false);
    }

    #region Property
    public NPCData NPCData { get { return npcData; } }
    public List<Quest> QuestList { get {  return questList; } }
    public bool IsTalking { get { return isTalking; } }

    public PlayerCharacter TargetCharacter { get { return targetCharacter; } }
    public float Distance { get { return distance; } }
    #endregion
}
