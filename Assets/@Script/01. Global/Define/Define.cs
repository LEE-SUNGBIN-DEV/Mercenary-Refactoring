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
    public HIT_TYPE hitType;
    public float damageRatio;
    public ABNORMAL_TYPE abnormalType;
    public float abnormalDuration;
    public Location effectLocation;

    public CombatInformation(HIT_TYPE hitType, float damageRatio, ABNORMAL_TYPE abnormalType, float abnormalDuration, Vector3 effectPosition, Vector3 effectRotation)
    {
        this.hitType = hitType;
        this.damageRatio = damageRatio;
        this.abnormalType = abnormalType;
        this.abnormalDuration = abnormalDuration;
        this.effectLocation = new Location(effectPosition, effectRotation);
    }
    public CombatInformation(HIT_TYPE hitType, float damageRatio, ABNORMAL_TYPE abnormalType, float abnormalDuration, Location effectLocation)
    {
        this.hitType = hitType;
        this.damageRatio = damageRatio;
        this.abnormalType = abnormalType;
        this.abnormalDuration = abnormalDuration;
        this.effectLocation = effectLocation;
    }

    public Quaternion GetQuaternion()
    {
        return effectLocation.GetQuaternion();
    }
}

