using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum COMBAT_TYPE
{
    NORMAL,
    SMASH,
    STUN,
    COUNTER,
    COMPETE,

    DEFENSE,
    PARRYING,
    COUNTER_SKILL,

    COUNTERABLE
}

public class BaseCombatController : MonoBehaviour
{
    [SerializeField] protected COMBAT_TYPE combatType;
    [SerializeField] protected float damageRatio;

    
    #region Property
    public COMBAT_TYPE CombatType
    {
        get { return combatType; }
    }
    public float DamageRatio
    {
        get { return damageRatio; }
    }
    #endregion
}
