using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

public struct AnimationClipInfo
{
    public string name;
    public int nameHash;
    public float length;
    public float frameRate;
    public int maxFrame;

    public AnimationClipInfo(string name, float length, float frameRate)
    {
        this.name = name;
        nameHash = Animator.StringToHash(name);
        this.length = length;
        this.frameRate = frameRate;
        this.maxFrame = Mathf.RoundToInt(frameRate * length);
    }
}

public struct CombatControllerInfo
{
    public HIT_TYPE hitType;
    public GUARD_TYPE guardType;
    public float damageRatio;
    public float crowdControlDuration;

    public CombatControllerInfo(HIT_TYPE hitType = HIT_TYPE.NONE, GUARD_TYPE guardType = GUARD_TYPE.NONE, float damageRatio = 1f, float crowdControlDuration = 0f)
    {
        this.hitType = hitType;
        this.guardType = guardType;
        this.damageRatio = damageRatio;
        this.crowdControlDuration = crowdControlDuration;
    }
}
