using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseCombatController : MonoBehaviour
{
    [Header("Base Combat Controller")]
    [SerializeField] protected COMBAT_TYPE combatType;
    [SerializeField] protected float damageRatio;
    [SerializeField] protected Collider attackCollider;

    public virtual void Initialize()
    {
        attackCollider = GetComponent<Collider>();
        attackCollider.enabled = false;
    }

    public COMBAT_TYPE CombatType { get { return combatType; } }
    public float DamageRatio { get { return damageRatio; } }
    public Collider AttackCollider { get { return attackCollider; } }
}
