using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseCombatController : MonoBehaviour
{
    [Header("Base Combat Controller")]
    [SerializeField] protected COMBAT_TYPE combatType;
    [SerializeField] protected float damageRatio;
    [SerializeField] protected float crowdControlDuration;
    [SerializeField] protected Collider combatCollider;
    protected Dictionary<BaseActor, bool> hitDictionary = new Dictionary<BaseActor, bool>();

    private void Awake()
    {
        TryGetComponent(out combatCollider);
        if (combatCollider != null)
            combatCollider.enabled = false;
    }

    public void SetCombatController(COMBAT_TYPE combatType, float damageRatio, float crowdControlDuration = 0f)
    {
        this.combatType = combatType;
        this.damageRatio = damageRatio;
        this.crowdControlDuration = crowdControlDuration;
    }

    public void SetCombatInformation(CombatInfo combatInformation)
    {
        this.combatType = combatInformation.combatType;
        this.damageRatio = combatInformation.damageRatio;
        this.crowdControlDuration = combatInformation.crowdControlDuration;
    }

    public COMBAT_TYPE CombatType { get { return combatType; } }
    public float DamageRatio { get { return damageRatio; } }
    public float CrowdControlDuration { get { return crowdControlDuration; } }
    public Collider CombatCollider { get { return combatCollider; } }
}
