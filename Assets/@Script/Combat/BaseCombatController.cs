using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseCombatController : MonoBehaviour
{
    [Header("Base Combat Controller")]
    [SerializeField] protected HIT_TYPE combatType;
    [SerializeField] protected CC_TYPE controlType;
    [SerializeField] protected float damageRatio;
    [SerializeField] protected Collider combatCollider;
    protected bool isInitialized = false;

    public virtual void Initialize()
    {
        if (isInitialized == false)
        {
            TryGetComponent(out combatCollider);
            if (combatCollider != null)
                combatCollider.enabled = false;

            isInitialized = true;
        }
    }
    public void SetCombatController(HIT_TYPE combatType, CC_TYPE controlType, float damageRatio)
    {
        this.combatType = combatType;
        this.controlType = controlType;
        this.damageRatio = damageRatio;
    }

    public HIT_TYPE CombatType { get { return combatType; } }
    public CC_TYPE ControlType { get { return controlType; } }
    public float DamageRatio { get { return damageRatio; } }
    public Collider CombatCollider { get { return combatCollider; } }
}
