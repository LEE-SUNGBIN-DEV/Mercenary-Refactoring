using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class NPCPanel : UIPanel, IFocusPanel, IDialogueablePanel
{
    public enum TEXT
    {
        NPC_Name_Text,
        NPC_Contents_Text
    }

    public enum BUTTON
    {
        NPC_Exit_Button
    }

    public event UnityAction<IFocusPanel> OnOpenFocusPanel;
    public event UnityAction<IFocusPanel> OnCloseFocusPanel;

    private LayoutGroup[] layoutGroups;

    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI npcContentText;
    [SerializeField] private Button exitButton;

    [SerializeField] private List<QuestData> questDatas;
    [SerializeField] private List<NPCQuestButton> npcQuestButtonList;
    [SerializeField] private List<Button> npcButtonList;
    [SerializeField] protected Quest selectQuest;

    [SerializeField] private NPC targetNPC;
    [SerializeField] private string dialogueID;
    [SerializeField] private int currentDialogueIndex;

    public void Initialize()
    {
        layoutGroups = GetComponentsInChildren<LayoutGroup>(true);

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        npcNameText = GetText((int)TEXT.NPC_Name_Text);
        npcContentText = GetText((int)TEXT.NPC_Contents_Text);
        exitButton = GetButton((int)BUTTON.NPC_Exit_Button);

        exitButton.onClick.AddListener(OnClickExitButton);

        targetNPC = null;
        dialogueID = null;
        currentDialogueIndex = 0;
    }

    private void OnEnable()
    {
        UpdateNPCQuestList();
        UpdateFunctionButtonList();
        Functions.RebuildLayout(layoutGroups);
    }

    private void OnDisable()
    {
        npcButtonList.Clear();
        npcQuestButtonList.Clear();
    }

    public string GetQuestDialogueID()
    {
        dialogueID = $"{selectQuest.QuestData.questID}_{selectQuest.CurrentTaskIndex}_{currentDialogueIndex}";
        return dialogueID;
    }
    public bool TryGetNextDialogue(out DialogueData nextDialogueData)
    {
        ++currentDialogueIndex;
        int dialogueIndex = dialogueID.LastIndexOf('_');
        dialogueID = dialogueID.Substring(0, dialogueIndex + 1) + currentDialogueIndex;
#if UNITY_EDITOR
        Debug.Log($"Next Dialogue ID: {dialogueID}");
#endif

        if (Managers.DataManager.DialogueTable.TryGetValue(dialogueID, out nextDialogueData))
            return true;
        else
            return false;
    }
    public void SetDialogue(DialogueData dialogueData)
    {
        dialogueID = dialogueData.dialogueID;
        npcNameText.text = dialogueData.speaker;
        npcContentText.text = dialogueData.content;
    }
    public void OpenPanel(NPC npc)
    {
        if (gameObject.activeSelf == false)
        {
            OnOpenFocusPanel?.Invoke(this);
            Managers.InputManager.PushInputMode(CHARACTER_INPUT_MODE.INTERACTION);
            Managers.UIManager.SetCursorMode(CURSOR_MODE.VISIBLE);
            targetNPC = npc;
            currentDialogueIndex = 0;
            SetDialogue(targetNPC.GetDefaultDialogueData());
            gameObject.SetActive(true);
        }
    }
    public void ClosePanel()
    {
        if (gameObject.activeSelf == true)
        {
            OnCloseFocusPanel?.Invoke(this);
            Managers.InputManager.PopInputMode();
            Managers.UIManager.SetCursorMode(CURSOR_MODE.INVISIBLE);
            gameObject.SetActive(false);
            currentDialogueIndex = 0;
            selectQuest = null;
            targetNPC = null;
        }
    }
    public void UpdateDialogue(PlayerCharacter character)
    {
        // 현재 다이얼로그 데이터 얻어오기
        if (Managers.DataManager.DialogueTable.TryGetValue(dialogueID, out DialogueData dialogueData))
        {
            // 선택지 존재여부 확인
            if (!dialogueData.selections.IsNullOrEmpty())
            {
                Managers.UIManager.UIInteractionPanelCanvas.DialogueSelectionPanel.OpenPanel(this, dialogueData);
            }

            else
            {
                // 다음 다이얼로그 존재여부 확인
                if (TryGetNextDialogue(out DialogueData nextDialogueData))
                    SetDialogue(nextDialogueData);

                // 대화 완료
                else
                {
                    // Quest
                    if (selectQuest != null)
                    {
                    }
                    character.InteractionController.ExitInteraction(character);
                }
            }
        }
    }
    public void UpdateNPCQuestList()
    {
        for (int i = 0; i < npcQuestButtonList.Count; ++i)
        {
            npcQuestButtonList[i].HideQuestButton();
            npcQuestButtonList[i].ShowQuestButton(questDatas[i]);
        }
    }

    public void UpdateFunctionButtonList()
    {

    }

    public void OnClickQuestButton()
    {

    }

    public void OnClickExitButton()
    {
        targetNPC.TargetCharacter.InteractionController.ExitInteraction(targetNPC.TargetCharacter);
    }

    #region Property
    public TextMeshProUGUI NPCNameText { get { return npcNameText; } }
    public TextMeshProUGUI NPCContentText { get { return npcContentText; } }
    public List<Button> NPCButtonList { get { return npcButtonList; } }
    #endregion
}
