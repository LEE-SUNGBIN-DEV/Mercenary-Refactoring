using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot
{
    public int slotIndex;
    public SelectionCharacter selectionCharacter;
    public Vector3 characterPoint;
    public TextMeshProUGUI slotText;
    public Button slotButton;

    public CharacterSlot()
    {
        slotText = null;
        slotButton = null;
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

public struct CombatActionInfomation
{
    public COMBAT_TYPE combatType;
    public float damageRatio;
    public float crowdControlDuration;
    public Location effectLocation;

    public CombatActionInfomation(COMBAT_TYPE hitType, float damageRatio, float crowdControlDuration = 0f)
    {
        this.combatType = hitType;
        this.damageRatio = damageRatio;
        this.crowdControlDuration = crowdControlDuration;
        this.effectLocation = new Location(Vector3.zero, Vector3.zero);
    }

    public CombatActionInfomation(COMBAT_TYPE hitType, float damageRatio, Vector3 effectPosition, Vector3 effectRotation, float crowdControlDuration = 0f)
    {
        this.combatType = hitType;
        this.damageRatio = damageRatio;
        this.crowdControlDuration = crowdControlDuration;
        this.effectLocation = new Location(effectPosition, effectRotation);
    }

    public CombatActionInfomation(COMBAT_TYPE hitType, float damageRatio, Location effectLocation, float crowdControlDuration = 0f)
    {
        this.combatType = hitType;
        this.damageRatio = damageRatio;
        this.crowdControlDuration = crowdControlDuration;
        this.effectLocation = effectLocation;
    }

    public Quaternion GetQuaternion()
    {
        return effectLocation.GetQuaternion();
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
public struct WayPointObjectData
{
    public CHAPTER_LIST chapter;
    public WAY_POINT_OBJECT_TYPE type;
    public string name;
    public Vector3 point;
}

[System.Serializable]
public struct WayPointData
{
    public CHAPTER_LIST chapter;
    public string name;
    public WayPointObjectData[] wayPointObjects;
}
#endregion