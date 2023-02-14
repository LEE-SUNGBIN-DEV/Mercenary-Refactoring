using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseCombatController : MonoBehaviour
{
    [Header("Base Combat Controller")]
    [SerializeField] protected COMBAT_TYPE combatType;
    [SerializeField] protected float damageRatio;
    [SerializeField] protected ABNORMAL_TYPE abnormalType;
    [SerializeField] protected float abnormalStateDuration;
    [SerializeField] protected Collider combatCollider;
    protected Dictionary<BaseActor, bool> hitDictionary = new Dictionary<BaseActor, bool>();

    private void Awake()
    {
        TryGetComponent(out combatCollider);
        if (combatCollider != null)
            combatCollider.enabled = false;
    }

    public void SetCombatController(COMBAT_TYPE combatType, float damageRatio, ABNORMAL_TYPE abnormalType = ABNORMAL_TYPE.None, float abnormalStateDuration = 0f)
    {
        this.combatType = combatType;
        this.damageRatio = damageRatio;
        this.abnormalType = abnormalType;
        this.abnormalStateDuration = abnormalStateDuration;
    }

    public void SetCombatInformation(CombatInformation combatInformation)
    {
        this.combatType = combatInformation.hitType;
        this.damageRatio = combatInformation.damageRatio;
        this.abnormalType = combatInformation.abnormalType;
        this.abnormalStateDuration = combatInformation.abnormalDuration;
    }

    public COMBAT_TYPE CombatType { get { return combatType; } }
    public ABNORMAL_TYPE AbnormalType { get { return abnormalType; } }
    public float DamageRatio { get { return damageRatio; } }
    public Collider CombatCollider { get { return combatCollider; } }
}
