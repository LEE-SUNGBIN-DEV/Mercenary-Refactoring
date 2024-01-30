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
    [SerializeField] protected Quest selectQuest;

    [Header("Current States")]
    [SerializeField] protected string dialogueID;
    [SerializeField] protected bool isTalking;
    [SerializeField] protected int currentDialogueIndex;
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
        dialogueID = null;
        isTalking = false;
        currentDialogueIndex = 0;
        selectQuest = null;
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
        EnterTalk();
    }
    public void UpdateInteraction(PlayerCharacter character)
    {
        if (Managers.InputManager.InteractionUpdateButton.WasPressedThisFrame())
        {
            UpdateTalk(character);
        }

        if (Managers.InputManager.InteractionExitButton.WasPressedThisFrame())
        {
            character.InteractionController.ExitInteraction(this, character);
        }
    }
    public void ExitInteraction(PlayerCharacter character)
    {
        ExitTalk();
    }
    #endregion

    public string GetDialogueID()
    {
        if (dialogueID == null)
        {
            if (selectQuest == null)
                dialogueID = $"{npcData.npcID}_DEFAULT";
            else
                dialogueID = $"{selectQuest.QuestData.questID}";
        }

        dialogueID += $"_{currentDialogueIndex}";

        return dialogueID;
    }
    public void EnterTalk()
    {
        currentDialogueIndex = 0;
        if (Managers.DataManager.DialogueTable.TryGetValue(GetDialogueID(), out DialogueData dialogueData))
        {
            Managers.InputManager.SwitchInputMode(CHARACTER_INPUT_MODE.INTERACTION);
            Managers.UIManager.UIInteractionPanelCanvas.NPCPanel.OpenPanel(dialogueData);
            isTalking = true;
        }
    }
    public void UpdateTalk(PlayerCharacter character)
    {
        // Progress Talk
        if (Managers.DataManager.DialogueTable.TryGetValue(GetDialogueID(), out DialogueData dialogueData))
        {
            // Selection Check


            // Dialogue Check
            ++currentDialogueIndex;
            Managers.UIManager.UIInteractionPanelCanvas.NPCPanel.OpenPanel(dialogueData);
        }

        // Finish or Exit Talk
        else
        {
            // Quest
            if(selectQuest != null)
            {
            }

            OnFinishTalk?.Invoke(this);
            character.InteractionController.ExitInteraction(character);
        }
    }
    public void ExitTalk()
    {
        Managers.InputManager.SwitchInputMode(CHARACTER_INPUT_MODE.ALL);
        isTalking = false;
        currentDialogueIndex = 0;
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
    public Quest SelectQuest { get { return selectQuest; } }

    public PlayerCharacter TargetCharacter { get { return targetCharacter; } }
    public float Distance { get { return distance; } }

    public bool IsTalking { get { return isTalking; } }
    public int CurrentDialogueIndex { get { return currentDialogueIndex; } }
    #endregion
}
