using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterQuestData : ICharacterData
{
    public event UnityAction<CharacterQuestData> OnChangeQuestData;

    [Header("Save Datas")]
    [JsonProperty][SerializeField] private Dictionary<string, QuestSaveData> questSaveDict;

    [Header("Runtime Datas")]
    [JsonIgnore] private Dictionary<string, Quest> questDict;

    public void CreateData()
    {
        questSaveDict = new Dictionary<string, QuestSaveData>();
    }
    public void LoadData()
    {
        // Update Quest Data From Table
        foreach (QuestData questData in Managers.DataManager.QuestTable.Values)
        {
            if (!questSaveDict.ContainsKey(questData.questID))
            {
                QuestSaveData saveData = new QuestSaveData();
                saveData.InitializeByQuestTable(questData);
                questSaveDict.Add(saveData.questID, saveData);
            }
        }

        // Load From Save Data
        questDict = new Dictionary<string, Quest>();
        foreach (QuestSaveData saveData in questSaveDict.Values)
        {
            Quest newQuest = new Quest();
            newQuest.LoadFromSaveData(saveData);
            questDict.Add(saveData.questID, newQuest);
        }
    }
    public void UpdateData(CharacterData characterData)
    {

    }
    public void SaveData()
    {
        foreach (Quest quest in questDict.Values)
        {
            if (questSaveDict.TryGetValue(quest.QuestData.questID, out QuestSaveData questSaveData))
            {
                questSaveData.SaveQuestData(quest);
            }
            else
            {
                QuestSaveData newQuestSaveData = new QuestSaveData().SaveQuestData(quest);
                questSaveDict.Add(newQuestSaveData.questID, newQuestSaveData);
            }
        }
    }

    public void DisableQuest(string questID)
    {
        questDict[questID].DisableQuest();
    }
    public void EnableQuest(string questID)
    {
        questDict[questID].EnableQuest();
    }
    public void AcceptQuest(string questID)
    {
        Managers.AudioManager.PlaySFX("AUDIO_QUEST_ACCEPT");
        questDict[questID].AcceptQuest();
    }
    public void ProgressQuest(string questID)
    {
        questDict[questID].ProgressQuest();
    }
    public void CompleteQuest(string questID)
    {
        Managers.AudioManager.PlaySFX("AUDIO_QUEST_COMPLETE");
        questDict[questID].CompleteQuest();
        //inventoryData.RewardResponseStone(quest.QuestData.rewardResponseStone);
        //statusData.RewardExperience(quest.QuestData.rewardExperience);
    }

    #region Property
    [JsonIgnore] public Dictionary<string, Quest> QuestDict { get { return questDict; } }
    #endregion
}
