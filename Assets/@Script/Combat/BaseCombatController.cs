using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseCombatController : MonoBehaviour
{
    [Header("Base Combat Controller")]
    [SerializeField] protected HIT_TYPE combatType;
    [SerializeField] protected CROWD_CONTROL_TYPE crowdControlType;
    [SerializeField] protected float damageRatio;
    [SerializeField] protected Collider combatCollider;

    private void Awake()
    {
        TryGetComponent(out combatCollider);
        if (combatCollider != null)
            combatCollider.enabled = false;
    }

    public void SetCombatController(HIT_TYPE combatType, CROWD_CONTROL_TYPE controlType, float damageRatio)
    {
        this.combatType = combatType;
        this.crowdControlType = controlType;
        this.damageRatio = damageRatio;
    }

    public HIT_TYPE CombatType { get { return combatType; } }
    public CROWD_CONTROL_TYPE CrowdControlType { get { return crowdControlType; } }
    public float DamageRatio { get { return damageRatio; } }
    public Collider CombatCollider { get { return combatCollider; } }
}
