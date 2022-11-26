using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class FunctionalNPC : NPC, ITalkable
{
    public static event UnityAction<string, string> OnStartTalk;    // 대화가 시작될 때 -> NPC 이름과, 대화 데이터를 인자로 뿌림 -> 다이얼로그 UI창 열림
    public static event UnityAction<uint> OnEndTalk;                // 대화가 끝났을 때 -> 해당 대화의 DialogueID 종료를 알림 -> 퀘스트 확인

    public static event UnityAction<FunctionalNPC> OnStartDialogue;   // 다이얼로그가 시작 될 때 -> 퀘스트 버튼 리스트 요청
    public static event UnityAction OnEndDialogue;                  // 다이얼로그가 종료 될 때 -> 퀘스트 버튼 리스트 초기화

    [TextArea]
    [SerializeField] private string[] defaultDialogues;
    [SerializeField] private GameObject questMark;
    private int dialogueIndex;
    private uint questID;

    [SerializeField] private List<Quest> questList;         // 퀘스트 목록

    public override void Initialize()
    {
        // 기본 대화 등록
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
