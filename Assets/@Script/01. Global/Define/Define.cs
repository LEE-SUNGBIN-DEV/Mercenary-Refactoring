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

[System.Serializable]
public class MaterialContainer
{
    public string key;
    public Material value;
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

public struct AnimationInfo
{
    public string animationName;
    public int animationNameHash;
    public float animationLength;
    public float animationSpeed;
    public int maxFrame;

    public AnimationInfo(string name, float length, int maxFrame, float speed = 1f)
    {
        animationName = name;
        animationNameHash = Animator.StringToHash(name);
        animationLength = length;
        animationSpeed = speed;
        this.maxFrame = maxFrame;
    }
}


public struct BasicStateInformation
{
    public ACTION_STATE idleState;
    public ACTION_STATE walkState;
    public ACTION_STATE runState;
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

