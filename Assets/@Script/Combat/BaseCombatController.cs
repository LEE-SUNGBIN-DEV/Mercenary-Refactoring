using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseCombatController : MonoBehaviour
{
    [Header("Base Combat Controller")]
    protected COMBAT_TYPE combatType;
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
    public void SetCombatController(COMBAT_TYPE combatType, float damageRatio)
    {
        this.combatType = combatType;
        this.damageRatio = damageRatio;
    }

    public COMBAT_TYPE CombatType { get { return combatType; } }
    public float DamageRatio { get { return damageRatio; } }
    public Collider CombatCollider { get { return combatCollider; } }
}
