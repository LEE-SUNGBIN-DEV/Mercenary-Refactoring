using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager
{
    public event UnityAction<Quest> OnCompleteQuest;
    public event UnityAction<FunctionalNPC> OnRequestNPCQuestList;

    [SerializeField] private Quest[] mainQuestDatabase;
    [SerializeField] private Quest[] subQuestDatabase;

    private Dictionary<uint, Quest> questDictionary = new Dictionary<uint, Quest>();
    private Dictionary<uint, string[]> dialogueDictionary = new Dictionary<uint, string[]>();

    [SerializeField] private List<Quest> inactiveQuestList = new List<Quest>();
    [SerializeField] private List<Quest> activeQuestList = new List<Quest>();
    [SerializeField] private List<Quest> acceptQuestList = new List<Quest>();
    [SerializeField] private List<Quest> completeQuestList = new List<Quest>();
    
    public void Initialize()
    {
        Managers.DataManager.SelectCharacterData.QuestData.OnChangeQuestData -= LoadQuest;
        Managers.DataManager.SelectCharacterData.QuestData.OnChangeQuestData += LoadQuest;
        Managers.DataManager.SelectCharacterData.QuestData.OnChangeQuestData -= SaveQuest;
        Managers.DataManager.SelectCharacterData.QuestData.OnChangeQuestData += SaveQuest;

        Managers.SceneManagerCS.OnSceneEnter -= RequestNPCQuestList;
        Managers.SceneManagerCS.OnSceneEnter += RequestNPCQuestList;

        QuestPopup.OnClickAcceptButton -= RequestAcceptList;
        QuestPopup.OnClickAcceptButton += RequestAcceptList;

        QuestPopup.OnClickCompleteButton -= RequestCompleteList;
        QuestPopup.OnClickCompleteButton += RequestCompleteList;

        Managers.DataManager.SelectCharacterData.QuestData.OnChangeQuestData -= RefreshInactiveQuest;
        Managers.DataManager.SelectCharacterData.QuestData.OnChangeQuestData += RefreshInactiveQuest;

        AddQuestToDictionary();
        ClassifyByQuestState();
    }

    private void AddQuestToDictionary()
    {
        for (uint i = 0; i < mainQuestDatabase.Length; ++i)
        {
            mainQuestDatabase[i].OnActiveQuest -= ActiveQuest;
            mainQuestDatabase[i].OnActiveQuest += ActiveQuest;

            mainQuestDatabase[i].OnAcceptQuest -= AcceptQuest;
            mainQuestDatabase[i].OnAcceptQuest -= AcceptQuest;

            mainQuestDatabase[i].OnCompleteQuest -= CompleteQuest;
            mainQuestDatabase[i].OnCompleteQuest -= CompleteQuest;

            QuestDictionary.Add(mainQuestDatabase[i].QuestID, mainQuestDatabase[i]);
        }

        for (uint i = 0; i < subQuestDatabase.Length; ++i)
        {
            subQuestDatabase[i].OnActiveQuest -= ActiveQuest;
            subQuestDatabase[i].OnActiveQuest += ActiveQuest;

            subQuestDatabase[i].OnAcceptQuest -= AcceptQuest;
            subQuestDatabase[i].OnAcceptQuest -= AcceptQuest;

            subQuestDatabase[i].OnCompleteQuest -= CompleteQuest;
            subQuestDatabase[i].OnCompleteQuest -= CompleteQuest;

            QuestDictionary.Add(subQuestDatabase[i].QuestID, subQuestDatabase[i]);
        }
    }

    public void ClassifyByQuestState()
    {
        foreach (var quest in QuestDictionary)
        {
            switch (quest.Value.questState)
            {
                case QUEST_STATE.INACTIVE:
                    {
                        InactiveQuestList.Add(quest.Value);
                        break;
                    }
                case QUEST_STATE.ACTIVE:
                    {
                        ActiveQuestList.Add(quest.Value);
                        break;
                    }
                case QUEST_STATE.ACCEPT:
                    {
                        AcceptQuestList.Add(quest.Value);
                        break;
                    }
                case QUEST_STATE.COMPLETE:
                    {
                        CompleteQuestList.Add(quest.Value);
                        break;
                    }
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

    #region Requst NPC Quest List
    // If there is any quest in NPC, Add quest to NPC quest List.
    public void RequestNPCQuestList()
    {
        foreach (var npc in Managers.NPCManager.NpcDictionary)
        {
            FunctionalNPC functionNPC = npc.Value as FunctionalNPC;
            if (functionNPC != null)
            {
                functionNPC.QuestList.Clear();

                for (int i = 0; i < AcceptQuestList.Count; ++i)
                {
                    DialogueTask dialogueTask = AcceptQuestList[i].QuestTasks[AcceptQuestList[i].TaskIndex] as DialogueTask;

                    if (dialogueTask != null && dialogueTask.NpcID == functionNPC.NpcID)
                    {
                        functionNPC.QuestList.Add(AcceptQuestList[i]);
                    }
                }

                for (int i = 0; i < ActiveQuestList.Count; ++i)
                {
                    DialogueTask dialogueTask = ActiveQuestList[i].QuestTasks[ActiveQuestList[i].TaskIndex] as DialogueTask;

                    if (dialogueTask != null && dialogueTask.NpcID == functionNPC.NpcID)
                    {
                        functionNPC.QuestList.Add(ActiveQuestList[i]);
                    }
                }

                OnRequestNPCQuestList(functionNPC);
            }
        }
    }
    public void RequestNPCQuestList(QuestTask questTask)
    {
        RequestNPCQuestList();
    }
    #endregion

    // 비활성화 목록을 검사하여 활성화 가능한 퀘스트가 있으면 활성화 리스트로 이동
    public void RefreshInactiveQuest(QuestData questData)
    {
        for (int i = 0; i < InactiveQuestList.Count; ++i)
        {
            if (Managers.DataManager.SelectCharacterData.StatData.Level >= InactiveQuestList[i].LevelCondition
                && questData.MainQuestPrograss >= InactiveQuestList[i].QuestCondition)
            {
                InactiveQuestList[i].ActiveQuest();
                --i;
            }
        }
        RequestNPCQuestList();
    }

    public void RequestAcceptList(QuestPopup questPopUp)
    {
        for (int i = 0; i < AcceptQuestList.Count; ++i)
        {
            if (questPopUp.QuestPopUpButtonList.Count < AcceptQuestList.Count)
            {
                questPopUp.CreateQuestButton();
            }

            if (questPopUp.QuestPopUpButtonList[i].isActive == false)
            {
                questPopUp.QuestPopUpButtonList[i].isActive = true;
                questPopUp.QuestPopUpButtonList[i].quest = AcceptQuestList[i];
                questPopUp.QuestPopUpButtonList[i].buttonText.text = questPopUp.QuestPopUpButtonList[i].quest.QuestTitle;
                questPopUp.QuestPopUpButtonList[i].button.onClick.AddListener(questPopUp.QuestPopUpButtonList[i].OnClickButton);
                questPopUp.QuestPopUpButtonList[i].button.gameObject.SetActive(true);
            }
        }
    }

    public void RequestCompleteList(QuestPopup questPopUp)
    {
        for (int i = 0; i < CompleteQuestList.Count; ++i)
        {
            if (questPopUp.QuestPopUpButtonList.Count < CompleteQuestList.Count)
            {
                questPopUp.CreateQuestButton();
            }

            questPopUp.QuestPopUpButtonList[i].isActive = true;
            questPopUp.QuestPopUpButtonList[i].quest = CompleteQuestList[i];
            questPopUp.QuestPopUpButtonList[i].buttonText.text = questPopUp.QuestPopUpButtonList[i].quest.QuestTitle;
            questPopUp.QuestPopUpButtonList[i].button.onClick.AddListener(questPopUp.QuestPopUpButtonList[i].OnClickButton);
            questPopUp.QuestPopUpButtonList[i].button.gameObject.SetActive(true);
        }
    }

    #region Save & Load
    public void SaveQuest(QuestData qeustData)
    {
        for (int i = 0; i < MainQuestDatabase.Length; ++i)
        {
            qeustData.QuestSaveList.Add(MainQuestDatabase[i].SaveQuest());
        }

        for (int i = 0; i < SubQuestDatabase.Length; ++i)
        {
            qeustData.QuestSaveList.Add(SubQuestDatabase[i].SaveQuest());
        }
    }

    public void LoadQuest(QuestData qeustData)
    {
        for (int i = 0; i < qeustData.QuestSaveList.Count; ++i)
        {
            QuestDictionary[qeustData.QuestSaveList[i].questID].LoadQuest(qeustData.QuestSaveList[i]);
        }
    }
    #endregion

    public void AddDialogue(QuestTask questTask)
    {
        if (questTask is DialogueTask)
        {
            DialogueTask dialogueTask = questTask as DialogueTask;
            uint dialogueID = dialogueTask.NpcID + dialogueTask.OwnerQuest.QuestID;
            if (!dialogueDictionary.ContainsKey(dialogueID))
            {
                dialogueDictionary.Add(dialogueID, dialogueTask.Dialogues);
            }
        }
    }
    public void RemoveDialogue(QuestTask questTask)
    {
        if (questTask is DialogueTask)
        {
            DialogueTask dialogueTask = questTask as DialogueTask;
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
    public Dictionary<uint, Quest> QuestDictionary { get { return questDictionary; } }
    public Dictionary<uint, string[]> DialogueDictionary { get { return dialogueDictionary; } }
    public Quest[] MainQuestDatabase { get { return mainQuestDatabase; } }
    public Quest[] SubQuestDatabase { get { return subQuestDatabase; } }
    public List<Quest> InactiveQuestList { get { return inactiveQuestList; } }
    public List<Quest> ActiveQuestList { get { return activeQuestList; } }
    public List<Quest> AcceptQuestList { get { return acceptQuestList; } }
    public List<Quest> CompleteQuestList { get { return completeQuestList; } }
    #endregion
}
