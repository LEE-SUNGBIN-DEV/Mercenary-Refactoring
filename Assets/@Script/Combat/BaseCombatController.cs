using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseCombatController : MonoBehaviour
{
    [Header("Base Combat Controller")]
    protected COMBAT_TYPE combatType;
    [SerializeField] protected float damageRatio;
    [SerializeField] protected Collider combatCollider;

    public virtual void Initialize()
    {
        combatCollider = GetComponent<Collider>();
        combatCollider.enabled = false;
    }

    public COMBAT_TYPE CombatType { get { return combatType; } }
    public float DamageRatio { get { return damageRatio; } }
    public Collider AttackCollider { get { return combatCollider; } }
}
