using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    [Header("Quest Infomations")]
    public QUEST_CATEGORY questCategory;
    public QUEST_STATE questState;
    public uint questID;
    public string questTitle;

    [Header("Conditions")]
    public int levelCondition;
    public uint mainQuestCondition;

    [Header("Tasks")]
    public string[] taskTooltip;
    public int[] requireAmount;
    public int[] successAmount;

    [Header("Rewards")]
    public float rewardExperience;
    public int rewardMoney;
    public int[] rewardItemIDs;
}

[System.Serializable]
public class QuestTable
{
    public QuestData[] questTable;
}
