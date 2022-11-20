using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialoguePanel : UIPanel
{
    public static event UnityAction onClickFunctionButton;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Button npcFunctionButton;
    [SerializeField] private TextMeshProUGUI npcFunctionButtonText;
    [SerializeField] private QuestListPanel npcQuestListPanel;

    public void Initialize(Character character)
    {
        FunctionalNPC.OnStartTalk -= SetDialogueText;
        FunctionalNPC.OnStartTalk += SetDialogueText;

        FunctionalNPC.OnStartDialogue -= NpcQuestListPanel.ActiveQuestButton;
        FunctionalNPC.OnStartDialogue += NpcQuestListPanel.ActiveQuestButton;

        FunctionalNPC.OnEndDialogue -= NpcQuestListPanel.InavtiveQuestButton;
        FunctionalNPC.OnEndDialogue += NpcQuestListPanel.InavtiveQuestButton;
    }

    public void SetDialogueText(string name, string content)
    {
        nameText.text = name;
        contentText.text = content;
    }
    
    public void ActiveNPCButton(string buttonName)
    {
        npcFunctionButtonText.text = buttonName;
        npcFunctionButton.gameObject.SetActive(true);
    }

    public void OnClickFunctionButton()
    {
        onClickFunctionButton();
    }

    #region Property
    public TextMeshProUGUI NameText { get { return nameText; } }
    public TextMeshProUGUI ContentText { get { return contentText; } }
    public Button NpcFunctionButton { get { return npcFunctionButton; } }
    public TextMeshProUGUI NpcFunctionButtonText { get { return npcFunctionButtonText; } }
    public QuestListPanel NpcQuestListPanel { get { return npcQuestListPanel; } }
    #endregion
}
