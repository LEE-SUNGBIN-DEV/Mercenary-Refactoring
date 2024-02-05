using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public interface ITableData<Key>
{
    public Key GetPrimaryKey();
    public void OnDataLoaded();
}

#region Level Data
[System.Serializable]
public class LevelData : ITableData<int>
{
    public int level;
    public float experience;

    public int GetPrimaryKey()
    {
        return level;
    }

    public void OnDataLoaded()
    {
    }
}
#endregion
#region Text Data
public class TextData : ITableData<string>
{
    public string textID;
    public string textContent;

    public string GetPrimaryKey()
    {
        return textID;
    }

    public void OnDataLoaded()
    {
    }
}
#endregion
#region Skill Data
[System.Serializable]
public class NodeData : ITableData<string>
{
    public string nodeID;
    public NODE_TYPE nodeType;
    public float[] nodeTransform;

    public string GetPrimaryKey()
    {
        return nodeID;
    }

    public void OnDataLoaded()
    {
    }

    public Vector3 GetNodeTransform()
    {
        if(nodeTransform != null && nodeTransform.Length == 3)
        {
            return new Vector3(nodeTransform[0], nodeTransform[1], nodeTransform[2]);
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("[Warning]: Wrong Node Transform");
#endif
            return Vector3.zero;
        }
    }
}

[System.Serializable]
public class SkillData : ITableData<string>
{
    // Skill Informations
    public string skillID;
    public SKILL_TYPE skillType;
    public string skillName;
    public string skillDescription;
    public int currentLevel;
    public int maxLevel;

    // Target Options
    public string fixedOptionID;

    // Preconditions
    public string nextLevelID;
    public string[] preconditionIDs;

    public string nodeID;
    public string spriteID;

    public string GetPrimaryKey()
    {
        return skillID;
    }

    public void OnDataLoaded()
    {
    }

    public SkillData GetNextSkillData()
    {
        return nextLevelID.IsNullOrEmpty() ? null : Managers.DataManager.SkillTable[nextLevelID];
    }
    public Sprite GetSprite()
    {
        return spriteID.IsNullOrEmpty() ? null : Managers.ResourceManager.LoadResourceSync<Sprite>(spriteID);
    }
    public NodeData GetNodeData()
    {
        return nodeID.IsNullOrEmpty() ? null : Managers.DataManager.NodeTable[nodeID];
    }
    public bool IsMaxLevel()
    {
        return (currentLevel == maxLevel);
    }
}
#endregion
#region Quest Data
public class QuestData : ITableData<string>
{
    public string questID;
    public QUEST_CATEGORY questCategory;
    public string questTitle;
    public string questDescription;

    [Header("Conditions")]
    public int preconditionLevel;
    public string[] preconditionQuestIDs;
    public string acceptanceID;
    public string completionID;

    [Header("Tasks")]
    public string[] taskIDs;

    [Header("Rewards")]
    public float rewardExperience;
    public int rewardResponseStone;
    public string[] rewardItemIDs;

    public string GetPrimaryKey()
    {
        return questID;
    }

    public void OnDataLoaded()
    {
    }
}
public class TaskData : ITableData<string>
{
    public string taskID;
    public TASK_CATEGORY taskCategory;
    public string[] taskTooltips;
    public string[] targetIDs;
    public int[] targetAmounts;
    public int[] currentAmounts;

    public string GetPrimaryKey()
    {
        return taskID;
    }

    public void OnDataLoaded()
    {
    }
}
#endregion
#region Buff Data
[System.Serializable]
public class BuffData
{
    public BUFF_TYPE buff;
    public string name;
    public string tooltip;
    public float lifetime;
    public bool isRemovable;
}

[System.Serializable]
public class DebuffData
{
    public DEBUFF_TYPE debuff;
    public string name;
    public string tooltip;
    public float lifetime;
    public bool isRemovable;
}
#endregion
#region Enemy Data
[System.Serializable]
public class EnemyData : ITableData<string>
{
    public string enemyID;
    public string enemyName;

    public ENEMY_TYPE enemyType;
    public float maxHP;
    public float attackPower;
    public float defensePower;
    public float criticalChance;
    public float criticalDamage;
    public float attackSpeed;
    public float moveSpeed;
    public float fixedDamage;
    public float defensePenetration;
    public float damageReduction;
    public float detectionDistance;
    public float chaseDistance;

    public string dropID;

    public string GetPrimaryKey()
    {
        return enemyID;
    }

    public void OnDataLoaded()
    {
    }
}
#endregion
#region Scene Data
[System.Serializable]
public class EnemySpawnerData : ITableData<string>
{
    public string spawnerID;
    public string enemyID;
    public SCENE_ID locatedScene;

    public string GetPrimaryKey()
    {
        return spawnerID;
    }

    public void OnDataLoaded()
    {
    }
}

[System.Serializable]
public class ResponseGateData : ITableData<string>
{
    public string responseGateID;
    public SCENE_ID locatedScene;
    public string destinationGateID;
    public string[] conditionBossIDs;

    public string GetPrimaryKey()
    {
        return responseGateID;
    }

    public void OnDataLoaded()
    {
    }

    public SCENE_ID GetDestinationScene()
    {
        return Managers.DataManager.ResponseGateTable[destinationGateID].locatedScene;
    }
}

[System.Serializable]
public class ResponseCrystalData : ITableData<string>
{
    public string responseCrystalID;
    public string regionName;
    public SCENE_ID locatedScene;

    public string GetPrimaryKey()
    {
        return responseCrystalID;
    }

    public void OnDataLoaded()
    {
    }
}
[System.Serializable]
public class ResponseTraceData : ITableData<string>
{
    public string responseTraceID;
    public SCENE_ID locatedScene;
    public string title;
    public string content;

    public string GetPrimaryKey()
    {
        return responseTraceID;
    }
    public void OnDataLoaded()
    {
    }
}
[System.Serializable]
public class TreasureBoxData : ITableData<string>
{
    public string treasureBoxID;
    public SCENE_ID locatedScene;
    public int dropCount;
    public string dropTableID;

    public string GetPrimaryKey()
    {
        return treasureBoxID;
    }

    public void OnDataLoaded()
    {
    }

    public DropTableData GetDropData()
    {
        Managers.DataManager.DropTable.TryGetValue(dropTableID, out DropTableData dropData);
        return dropData;
    }
}

public class NPCData : ITableData<string>
{
    public string npcID;
    public string npcName;
    public NPC_TYPE npcType;
    public bool isTalkable;

    public string GetPrimaryKey()
    {
        return npcID;
    }

    public void OnDataLoaded()
    {
    }
}
[System.Serializable]
public class GameSceneData : ITableData<SCENE_ID>
{
    public SCENE_ID sceneID;
    public SCENE_TYPE sceneType;
    public string sceneName;
    public WEATHER_TYPE weatherType;

    [JsonIgnore] public List<ResponseCrystalData> responseCrystalDataList;
    [JsonIgnore] public List<ResponseGateData> responseGateDataList;
    [JsonIgnore] public List<ResponseTraceData> responseTraceDataList;
    [JsonIgnore] public List<TreasureBoxData> treasureBoxDataList;

    public SCENE_ID GetPrimaryKey()
    {
        return sceneID;
    }

    public void OnDataLoaded()
    {
        responseCrystalDataList = new List<ResponseCrystalData>();
        responseGateDataList = new List<ResponseGateData>();
        responseTraceDataList = new List<ResponseTraceData>();
        treasureBoxDataList = new List<TreasureBoxData>();
    }
}
#endregion
#region Item Data
[System.Serializable]
public class ItemData : ITableData<string>
{
    public string itemID;
    public string itemName;
    public string itemDescription;
    public ITEM_TYPE itemType;
    public ITEM_RANK itemRank;
    public ITEM_CATEGORY itemCategory;
    public int itemGrade;
    public string nextGradeID;
    public string fixedOptionID;
    public string randomOptionID;
    public string spriteID;

    public string GetPrimaryKey()
    {
        return itemID;
    }

    public void OnDataLoaded()
    {
    }

    public Sprite GetItemSprite()
    {
        return spriteID.IsNullOrEmpty() ? null : Managers.ResourceManager.LoadResourceSync<Sprite>(spriteID);
    }
}
[System.Serializable]
public class ResponseWaterData : ITableData<string>
{
    public string responseWaterID;

    public int maxCount;
    public float hpRecoveryPercentage;
    public float spRecoveryPercentage;
    public int responseCount;

    public string GetPrimaryKey()
    {
        return responseWaterID;
    }

    public void OnDataLoaded()
    {
    }
}
#endregion

#region Stat Data

[System.Serializable]
public class StatData : ITableData<string>
{
    public string statID;

    public string statName;
    public STAT_TYPE statType;
    public float defaultValue;
    public float minValue;
    public float maxValue;
    public STAT_UNIT_TYPE statUnitType;

    public string GetPrimaryKey()
    {
        return statID;
    }

    public void OnDataLoaded()
    {
    }
}
[System.Serializable]
public class FixedOptionData : ITableData<string>
{
    public string fixedOptionID;

    public string[] statIDs;
    public VALUE_TYPE[] valueTypes;
    public float[] values;

    public string GetPrimaryKey()
    {
        return fixedOptionID;
    }

    public void OnDataLoaded()
    {

    }

    public StatOption[] CreateFixedOptions()
    {
        if (statIDs.IsNullOrEmpty())
            return null;

        StatOption[] newStatOptions = new StatOption[statIDs.Length];
        for (int i = 0; i < statIDs.Length; i++)
        {
            newStatOptions[i] = new StatOption(statIDs[i], valueTypes[i], values[i]);
        }
        return newStatOptions;
    }
}
[System.Serializable]
public class RandomOptionData : ITableData<string>
{
    public string randomOptionID;

    public int optionCount;
    public string[] statIDs;
    public VALUE_TYPE[] valueTypes;
    public float[] minValueRanges;
    public float[] maxValueRanges;

    public string GetPrimaryKey()
    {
        return randomOptionID;
    }

    public void OnDataLoaded()
    {
    }

    public StatOption[] CreateRandomOptions()
    {
        if (statIDs.IsNullOrEmpty())
            return null;

        HashSet<int> optionIndexHashSet = new HashSet<int>();
        while (optionIndexHashSet.Count < optionCount)
        {
            optionIndexHashSet.Add(Random.Range(0, statIDs.Length));
        }

        StatOption[] newStats = new StatOption[optionCount];
        int index = 0;
        foreach (int optionIndex in optionIndexHashSet)
        {
            float statusOptionValue = System.MathF.Round(Random.Range(minValueRanges[optionIndex], maxValueRanges[optionIndex]), 1);
            newStats[index] = new StatOption(statIDs[optionIndex], valueTypes[optionIndex], statusOptionValue);
            index++;
        }

        return newStats;
    }
}
#endregion
[System.Serializable]
public class DropTableData : ITableData<string>
{
    public string dropID;
    public float dropExperience;
    public int dropResonanceStone;
    public float dropItemProbability;
    public string[] dropItemIDs;
    public int[] dropItemWeights;

    public string GetPrimaryKey()
    {
        return dropID;
    }

    public void OnDataLoaded()
    {
    }

    public BaseItem DropItem()
    {
        // »πµÊ ∞·¡§
        float randomDrop = Random.Range(0, 1);

        // »πµÊ
        if (randomDrop <= dropItemProbability)
        {
            int totalWeight = 0;
            for (int i = 0; i < dropItemIDs.Length; ++i)
            {
                totalWeight += dropItemWeights[i];
            }

            int randomWeight = Random.Range(0, totalWeight);
            for (int i = 0; i < dropItemIDs.Length; ++i)
            {
                randomWeight -= dropItemWeights[i];
                if (randomWeight <= 0)
                {
                    ItemData itemData = Managers.DataManager.ItemTable[dropItemIDs[i]];
                    switch (itemData.itemType)
                    {
                        case ITEM_TYPE.NORMAL:
                            BaseItem newItem = new BaseItem(dropItemIDs[i]);
                            return newItem;

                        case ITEM_TYPE.RUNE:
                            RuneItem newRuneItem = new RuneItem(dropItemIDs[i]);
                            newRuneItem.CreateRandomOptions();
                            return newRuneItem as RuneItem;
                    }

                }
            }
        }
        // πÃ»πµÊ
        return null;
    }
}

[System.Serializable]
public class DialogueData : ITableData<string>
{
    public string dialogueID;
    public string speaker;
    public string content;
    public string[] selections;
    public string[] selectionTargetIDs;

    public string GetPrimaryKey()
    {
        return dialogueID;
    }

    public void OnDataLoaded()
    {
    }
}


