using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseCombatController : MonoBehaviour
{
    [Header("Base Combat Controller")]
    [SerializeField] protected HIT_TYPE combatType;
    [SerializeField] protected ABNORMAL_STATE crowdControlType;
    [SerializeField] protected float damageRatio;
    [SerializeField] protected Collider combatCollider;

    private void Awake()
    {
        TryGetComponent(out combatCollider);
        if (combatCollider != null)
            combatCollider.enabled = false;
    }

    public void SetCombatController(HIT_TYPE combatType, ABNORMAL_STATE controlType, float damageRatio)
    {
        this.combatType = combatType;
        this.crowdControlType = controlType;
        this.damageRatio = damageRatio;
    }

    public HIT_TYPE CombatType { get { return combatType; } }
    public ABNORMAL_STATE CrowdControlType { get { return crowdControlType; } }
    public float DamageRatio { get { return damageRatio; } }
    public Collider CombatCollider { get { return combatCollider; } }
}
