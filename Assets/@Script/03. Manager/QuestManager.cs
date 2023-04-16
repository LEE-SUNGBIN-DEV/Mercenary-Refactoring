using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager
{
    public event UnityAction<Quest> OnCompleteQuest;
    public event UnityAction<FunctionalNPC> OnRequestNPCQuestList;

    private Dictionary<uint, string[]> dialogueDictionary = new Dictionary<uint, string[]>();

    [SerializeField] private List<Quest> inactiveQuestList = new List<Quest>();
    [SerializeField] private List<Quest> activeQuestList = new List<Quest>();
    [SerializeField] private List<Quest> acceptQuestList = new List<Quest>();
    [SerializeField] private List<Quest> completeQuestList = new List<Quest>();
    
    public void Initialize()
    {
        Managers.DataManager.CurrentCharacterData.QuestData.OnChangeQuestData -= LoadPlayerQuestData;
        Managers.DataManager.CurrentCharacterData.QuestData.OnChangeQuestData += LoadPlayerQuestData;

        Managers.DataManager.CurrentCharacterData.QuestData.OnChangeQuestData -= RefreshInactiveQuest;
        Managers.DataManager.CurrentCharacterData.QuestData.OnChangeQuestData += RefreshInactiveQuest;

        AddQuestEvent();
    }

    public void LoadPlayerQuestData(PlayerQuestData savedQuest)
    {
        for (int i = 0; i < savedQuest.QuestSaveList.Count; ++i)
        {
            QuestDatabase[savedQuest.QuestSaveList[i].questID].LoadQuest(savedQuest.QuestSaveList[i]);
        }
    }

    private void AddQuestEvent()
    {
        foreach(var quest in QuestDatabase.Values)
        {
            quest.OnActiveQuest -= ActiveQuest;
            quest.OnActiveQuest += ActiveQuest;

            quest.OnAcceptQuest -= AcceptQuest;
            quest.OnAcceptQuest -= AcceptQuest;

            quest.OnCompleteQuest -= CompleteQuest;
            quest.OnCompleteQuest -= CompleteQuest;

            ClassifyByQuestState(quest);
        }
    }

    public void ClassifyByQuestState(Quest quest)
    {
        switch (quest.QuestState)
        {
            case QUEST_STATE.INACTIVE:
                {
                    InactiveQuestList.Add(quest);
                    break;
                }
            case QUEST_STATE.ACTIVE:
                {
                    ActiveQuestList.Add(quest);
                    break;
                }
            case QUEST_STATE.ACCEPT:
                {
                    AcceptQuestList.Add(quest);
                    break;
                }
            case QUEST_STATE.COMPLETE:
                {
                    CompleteQuestList.Add(quest);
                    break;
                }
        }
    }

    public void ActiveQuest(Quest quest)
    {
        if(InactiveQuestList.Contains(quest))
        {
            ActiveQuestList.Add(quest);
            InactiveQuestList.Remove(quest);
        }
    }
    public void AcceptQuest(Quest quest)
    {
        if (ActiveQuestList.Contains(quest))
        {
            AcceptQuestList.Add(quest);
            ActiveQuestList.Remove(quest);
        }
    }
    public void CompleteQuest(Quest quest)
    {
        if (AcceptQuestList.Contains(quest))
        {
            CompleteQuestList.Add(quest);
            AcceptQuestList.Remove(quest);
        }
        OnCompleteQuest(quest);
    }

    #region Refresh NPC Quest List
    public void RefreshNPCQuestList(NPC requestNPC)
    {
        if (requestNPC is FunctionalNPC functionalNPC)
        {
            functionalNPC.QuestList.Clear();
            for (int i = 0; i < AcceptQuestList.Count; ++i)
            {
                if(AcceptQuestList[i].QuestTasks[AcceptQuestList[i].TaskIndex] is DialogueTask dialogueTask)
                {
                    if (dialogueTask.NpcID == functionalNPC.NpcID)
                    {
                        functionalNPC.QuestList.Add(AcceptQuestList[i]);
                    }
                }
            }

            for (int i = 0; i < ActiveQuestList.Count; ++i)
            {
                if(ActiveQuestList[i].QuestTasks[ActiveQuestList[i].TaskIndex] is DialogueTask dialogueTask)
                {
                    if (dialogueTask.NpcID == functionalNPC.NpcID)
                    {
                        functionalNPC.QuestList.Add(ActiveQuestList[i]);
                    }
                }
            }
            OnRequestNPCQuestList?.Invoke(functionalNPC);
        }
    }
    public void RefreshAllNPCQuestList()
    {
        foreach (var npc in Managers.NPCManager.NPCDictionary.Values)
        {
            RefreshNPCQuestList(npc);
        }
    }
    public void RefreshAllNPCQuestList(QuestTask questTask)
    {
        RefreshAllNPCQuestList();
    }
    #endregion

    // 비활성화 목록을 검사하여 활성화 가능한 퀘스트가 있으면 활성화 리스트로 이동
    public void RefreshInactiveQuest(PlayerQuestData questData)
    {
        for (int i = 0; i < InactiveQuestList.Count; ++i)
        {
            if (InactiveQuestList[i].CanActive(Managers.DataManager.CurrentCharacterData))
            {
                InactiveQuestList[i].ActiveQuest();
                --i;
            }
        }
        RefreshAllNPCQuestList();
    }

    public void RequestAcceptList(QuestPanel questPopUp)
    {
        for (int i = 0; i < AcceptQuestList.Count; ++i)
        {
            if (questPopUp.QuestPopUpButtonList.Count < AcceptQuestList.Count)
                questPopUp.CreateQuestButton();

            questPopUp.QuestPopUpButtonList[i].SetQuestButton(AcceptQuestList[i]);
        }
    }

    public void RequestCompleteList(QuestPanel questPopUp)
    {
        for (int i = 0; i < CompleteQuestList.Count; ++i)
        {
            if (questPopUp.QuestPopUpButtonList.Count < CompleteQuestList.Count)
                questPopUp.CreateQuestButton();

            questPopUp.QuestPopUpButtonList[i].SetQuestButton(CompleteQuestList[i]);
        }
    }

    public void AddDialogue(QuestTask questTask)
    {
        if (questTask is DialogueTask dialogueTask)
        {
            uint dialogueID = dialogueTask.NpcID + dialogueTask.OwnerQuest.QuestID;
            if (!dialogueDictionary.ContainsKey(dialogueID))
            {
                dialogueDictionary.Add(dialogueID, dialogueTask.Dialogues);
            }
        }
    }
    public void RemoveDialogue(QuestTask questTask)
    {
        if (questTask is DialogueTask dialogueTask)
        {
            uint dialogueID = dialogueTask.NpcID + dialogueTask.OwnerQuest.QuestID;
            if (dialogueDictionary.ContainsKey(dialogueID))
            {
                dialogueDictionary.Remove(dialogueID);
            }
        }
    }
    public string GetDialogue(uint dialogueID, int dialogueIndex)
    {
        if (dialogueIndex == dialogueDictionary[dialogueID].Length)
        {
            return null;
        }
        else
        {
            return dialogueDictionary[dialogueID][dialogueIndex];
        }
    }

    #region Property
    public Dictionary<uint, Quest> QuestDatabase { get { return Managers.DataManager?.QuestTable; } }
    public Dictionary<uint, string[]> DialogueDictionary { get { return dialogueDictionary; } }
    public List<Quest> InactiveQuestList { get { return inactiveQuestList; } }
    public List<Quest> ActiveQuestList { get { return activeQuestList; } }
    public List<Quest> AcceptQuestList { get { return acceptQuestList; } }
    public List<Quest> CompleteQuestList { get { return completeQuestList; } }
    #endregion
}
