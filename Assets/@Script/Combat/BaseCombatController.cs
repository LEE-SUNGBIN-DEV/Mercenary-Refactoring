using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseCombatController : MonoBehaviour
{
    [Header("Base Combat Controller")]
    [SerializeField] protected COMBAT_TYPE combatType;
    [SerializeField] protected float damageRatio;
    [SerializeField] protected BUFF debuffType;
    [SerializeField] protected float debuffDuration;
    [SerializeField] protected Collider combatCollider;
    protected Dictionary<BaseActor, bool> hitDictionary = new Dictionary<BaseActor, bool>();

    private void Awake()
    {
        TryGetComponent(out combatCollider);
        if (combatCollider != null)
            combatCollider.enabled = false;
    }

    public void SetCombatController(COMBAT_TYPE combatType, float damageRatio, BUFF debuffType = BUFF.None, float debuffDuration = 0f)
    {
        this.combatType = combatType;
        this.damageRatio = damageRatio;
        this.debuffType = debuffType;
        this.debuffDuration = debuffDuration;
    }

    public void SetCombatInformation(CombatInformation combatInformation)
    {
        this.combatType = combatInformation.hitType;
        this.damageRatio = combatInformation.damageRatio;
        this.debuffType = combatInformation.debuffType;
        this.debuffDuration = combatInformation.debuffDuration;
    }

    public COMBAT_TYPE CombatType { get { return combatType; } }
    public BUFF DebuffType { get { return debuffType; } }
    public float DamageRatio { get { return damageRatio; } }
    public Collider CombatCollider { get { return combatCollider; } }
}
