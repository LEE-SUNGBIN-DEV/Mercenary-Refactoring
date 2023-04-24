using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Newtonsoft.Json;

public struct DamageInformation
{
    public float damage;
    public bool isCritical;

    public DamageInformation(float damage, bool isCritical)
    {
        this.damage = damage;
        this.isCritical = isCritical;
    }
}

public struct Location
{
    public Vector3 position;
    public Vector3 rotation;

    public Location(Vector3 position, Vector3 rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    public Quaternion GetQuaternion()
    {
        return Quaternion.Euler(rotation);
    }
}

public struct AnimationClipInformation
{
    public string name;
    public int nameHash;
    public float length;
    public float frameRate;
    public int maxFrame;

    public AnimationClipInformation(string name, float length, float frameRate)
    {
        this.name = name;
        nameHash = Animator.StringToHash(name);
        this.length = length;
        this.frameRate = frameRate;
        this.maxFrame = Mathf.RoundToInt(frameRate * length);
    }
}

public struct CombatControllerInfomation
{
    public HIT_TYPE hitType;
    public GUARD_TYPE guardType;
    public float damageRatio;
    public float crowdControlDuration;

    public CombatControllerInfomation(HIT_TYPE hitType = HIT_TYPE.NONE, GUARD_TYPE guardType = GUARD_TYPE.NONE, float damageRatio = 1f, float crowdControlDuration = 0f)
    {
        this.hitType = hitType;
        this.guardType = guardType;
        this.damageRatio = damageRatio;
        this.crowdControlDuration = crowdControlDuration;
    }
}

#region Data Structure
[System.Serializable]
public struct LevelTable
{
    public int[] levels;
    public float[] maxExperiences;
}

[System.Serializable]
public struct QuestSaveData
{
    public QUEST_STATE questState;
    public uint questID;

    public int taskIndex;
    public int taskSuccessAmount;

    public QuestSaveData(Quest quest)
    {
        questState = quest.QuestState;
        questID = quest.QuestID;
        taskIndex = quest.TaskIndex;
        taskSuccessAmount = quest.QuestTasks[taskIndex].SuccessAmount;
    }
}

[System.Serializable]
public struct BuffData
{
    public BUFF_TYPE buff;
    public string name;
    public string tooltip;
    public float lifetime;
    public bool isRemovable;
}

[System.Serializable]
public struct DebuffData
{
    public DEBUFF_TYPE debuff;
    public string name;
    public string tooltip;
    public float lifetime;
    public bool isRemovable;
}

[System.Serializable]
public struct EnemySpawnData
{
    public SCENE_LIST scene;
    public ENEMY_TYPE enemyType;
    public string enemyName;
    public float xCoordinate;
    public float yCoordinate;
    public float zCoordinate;

    public Vector3 GetPosition()
    {
        return new Vector3(xCoordinate, yCoordinate, zCoordinate);
    }
}

[System.Serializable]
public struct ResonanceObjectData
{
    public SCENE_LIST scene;
    public string regionName;
    public string objectName;
    public float xCoordinate;
    public float yCoordinate;
    public float zCoordinate;

    [JsonIgnore] public int index;

    public Vector3 GetPosition()
    {
        return new Vector3(xCoordinate, yCoordinate, zCoordinate);
    }
}

[System.Serializable]
public struct GameSceneData
{
    public SCENE_LIST scene;
    public SCENE_TYPE sceneType;
    public string sceneName;
    public WEATHER_TYPE weatherType;

    [JsonIgnore] public List<EnemySpawnData> normalSpawnDataList;
    [JsonIgnore] public List<EnemySpawnData> eliteSpawnDataList;
    [JsonIgnore] public List<EnemySpawnData> bossSpawnDataList;
    [JsonIgnore] public List<ResonanceObjectData> resonanceObjectDataList;

    public void Initialize()
    {
        resonanceObjectDataList = new List<ResonanceObjectData>();
        normalSpawnDataList = new List<EnemySpawnData>();
        eliteSpawnDataList = new List<EnemySpawnData>();
        bossSpawnDataList = new List<EnemySpawnData>();
    }
}
#endregion