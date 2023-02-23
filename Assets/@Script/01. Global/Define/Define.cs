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

public struct CombatInformation
{
    public COMBAT_TYPE hitType;
    public float damageRatio;
    public BUFF debuffType;
    public float debuffDuration;
    public Location effectLocation;

    public CombatInformation(COMBAT_TYPE hitType, float damageRatio, BUFF abnormalType, float abnormalDuration)
    {
        this.hitType = hitType;
        this.damageRatio = damageRatio;
        this.debuffType = abnormalType;
        this.debuffDuration = abnormalDuration;
        this.effectLocation = new Location(Vector3.zero, Vector3.zero);
    }

    public CombatInformation(COMBAT_TYPE hitType, float damageRatio, BUFF abnormalType, float abnormalDuration, Vector3 effectPosition, Vector3 effectRotation)
    {
        this.hitType = hitType;
        this.damageRatio = damageRatio;
        this.debuffType = abnormalType;
        this.debuffDuration = abnormalDuration;
        this.effectLocation = new Location(effectPosition, effectRotation);
    }

    public CombatInformation(COMBAT_TYPE hitType, float damageRatio, BUFF abnormalType, float abnormalDuration, Location effectLocation)
    {
        this.hitType = hitType;
        this.damageRatio = damageRatio;
        this.debuffType = abnormalType;
        this.debuffDuration = abnormalDuration;
        this.effectLocation = effectLocation;
    }

    public Quaternion GetQuaternion()
    {
        return effectLocation.GetQuaternion();
    }
}

