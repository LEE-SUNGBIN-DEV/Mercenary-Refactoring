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
public struct LevelData
{
    public int level;
    public float experience;
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

public struct EnemyData
{
    public int id;
    public string enemyName;
    public string enemyPrefabName;

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

    public int dropDataID;
}
[System.Serializable]
public struct EnemySpawnerData
{
    public int id;
    public int enemyID;

    public SCENE_LIST GetSpawnerScene()
    {
        return (SCENE_LIST)(id / 100);
    }

    public int GetSpawnerIndex()
    {
        return id % 100;
    }
}

[System.Serializable]
public struct ResonanceGateData
{
    public int id;
    public SCENE_LIST destination;

    public SCENE_LIST GetLocatedScene()
    {
        return (SCENE_LIST)(id / 100);
    }

    public SCENE_LIST GetDestination()
    {
        return destination;
    }

    public int GetResonanceGateIndex()
    {
        return id % 100;
    }
}

[System.Serializable]
public struct ResonanceCrystalData
{
    public int id;
    public string regionName;

    public SCENE_LIST GetLocatedScene()
    {
        return (SCENE_LIST)(id / 100);
    }

    public int GetResonancePointIndex()
    {
        return id % 100;
    }
}

[System.Serializable]
public struct DropData
{
    public int id;
    public float experience;
    public float resonanceStone;
    public string[] itemNames;
    public int[] itemWeights;
}

[System.Serializable]
public struct TreasureBoxData
{
    public int id;

    public SCENE_LIST GetLocatedScene()
    {
        return (SCENE_LIST)(id / 100);
    }

    public int GetTreasureBoxIndex()
    {
        return id % 100;
    }
}

[System.Serializable]
public struct GameSceneData
{
    public SCENE_LIST scene;
    public SCENE_TYPE sceneType;
    public string sceneName;
    public WEATHER_TYPE weatherType;

    [JsonIgnore] public List<ResonanceCrystalData> resonanceCrystalDataList;
    [JsonIgnore] public List<ResonanceGateData> resonanceGateDataList;
    [JsonIgnore] public List<TreasureBoxData> treasureBoxDataList;

    public void Initialize()
    {
        resonanceCrystalDataList = new List<ResonanceCrystalData>();
        resonanceGateDataList = new List<ResonanceGateData>();
        treasureBoxDataList = new List<TreasureBoxData>();
    }
}

public struct HalberdData
{
    public int id;
    public string name;

    public float attackPower;
    public float attackSpeed;
    public float fixedDamage;
    public float defensePenetration;
}
public struct SwordShieldData
{
    public int id;
    public string name;

    public float attackPower;
    public float attackSpeed;
    public float fixedDamage;
    public float defensePower;
}
public struct ArmorData
{
    public int id;
    public string name;

    public float hp;
    public float sp;
    public float defensePower;
    public float damageReduction;
    public float spRecovery;
    public float spCostReduction;
}
#endregion