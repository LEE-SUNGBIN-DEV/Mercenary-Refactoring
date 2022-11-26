using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class FunctionalNPC : NPC, ITalkable
{
    public static event UnityAction<string, string> OnStartTalk;    // ��ȭ�� ���۵� �� -> NPC �̸���, ��ȭ �����͸� ���ڷ� �Ѹ� -> ���̾�α� UIâ ����
    public static event UnityAction<uint> OnEndTalk;                // ��ȭ�� ������ �� -> �ش� ��ȭ�� DialogueID ���Ḧ �˸� -> ����Ʈ Ȯ��

    public static event UnityAction<FunctionalNPC> OnStartDialogue;   // ���̾�αװ� ���� �� �� -> ����Ʈ ��ư ����Ʈ ��û
    public static event UnityAction OnEndDialogue;                  // ���̾�αװ� ���� �� �� -> ����Ʈ ��ư ����Ʈ �ʱ�ȭ

    [TextArea]
    [SerializeField] private string[] defaultDialogues;
    [SerializeField] private GameObject questMark;
    private int dialogueIndex;
    private uint questID;

    [SerializeField] private List<Quest> questList;         // ����Ʈ ���

    public override void Initialize()
    {
        // �⺻ ��ȭ ���
        if (!Managers.QuestManager.DialogueDictionary.ContainsKey(NpcID))
        {
            Managers.QuestManager.DialogueDictionary.Add(NpcID, defaultDialogues);
        }

        Managers.QuestManager.OnRequestNPCQuestList -= SetQuestMark;
        Managers.QuestManager.OnRequestNPCQuestList += SetQuestMark;
        Managers.QuestManager.RefreshNPCQuestList(this);

        CanTalk = false;
        IsTalk = false;
        dialogueIndex = 0;
        questID = 0;
    }

    public void OnDisable()
    {
        QuestList.Clear();
        DialoguePanel.onClickFunctionButton -= OpenNPCUI;
    }

    private void Update()
    {
        if (CanTalk && Input.GetKeyDown(KeyCode.G))
        {
            OnDialogue();
        }

        else if(IsTalk && Input.GetKeyDown(KeyCode.G))
        {
            OnTalk();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>() != null)
        {
            CanTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Character>() != null)
        {
            OffTalk();
        }
    }

    public void SetQuestMark(FunctionalNPC functionNPC)
    {
        if (functionNPC == this)
        {
            if (functionNPC.questList.Count > 0)
            {
                QuestMark.SetActive(true);
            }
            else
            {
                QuestMark.SetActive(false);
            }
        }
    }

    public void OnDialogue()
    {
        DialoguePanel.onClickFunctionButton -= OpenNPCUI;
        DialoguePanel.onClickFunctionButton += OpenNPCUI;

        CanTalk = false;
        OnStartDialogue(this);
        ActiveNPCFunctionButton();

        OnTalk();
    }

    public void OnTalk()
    {
        // Set Talk Infomation
        uint dialogueID = questID + NpcID;
        string dialogueData = Managers.QuestManager.GetDialogue(dialogueID, DialogueIndex);

        // Talk Progress
        if (dialogueData != null)
        {
            IsTalk = true;
            ++dialogueIndex;

            OnStartTalk(NpcName, dialogueData);
        }

        // Talk End
        else
        {
            OnEndTalk(dialogueID);
            OffTalk();
            CanTalk = true;
        }
    }

    public void OffTalk()
    {
        CanTalk = false;
        IsTalk = false;
        dialogueIndex = 0;
        questID = 0;

        OnEndDialogue();
        CloseNPCUI();

        DialoguePanel.onClickFunctionButton -= OpenNPCUI;
    }

    public abstract void OpenNPCUI();
    public abstract void CloseNPCUI();
    public abstract void ActiveNPCFunctionButton();

    #region Property
    public bool CanTalk { get; set; }
    public bool IsTalk { get; set; }
    public string[] DefaultTalk { get { return defaultDialogues; } }
    public int DialogueIndex { get { return dialogueIndex; } set { dialogueIndex = value; } }
    public uint QuestID
    {
        get { return questID; }
        set { questID = value; }
    }
    public GameObject QuestMark
    {
        get { return questMark; }
    }
    public List<Quest> QuestList
    {
        get { return questList; }
    }
    #endregion
}
