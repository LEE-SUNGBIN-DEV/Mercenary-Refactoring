using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseCombatController : MonoBehaviour
{
    [Header("Base Combat Controller")]
    [SerializeField] protected HIT_TYPE hitType;
    [SerializeField] protected GUARD_TYPE guardType;
    [SerializeField] protected float damageRatio;
    [SerializeField] protected float crowdControlDuration;

    [SerializeField] protected Collider combatCollider;
    protected SFXPlayer sfxPlayer;

    protected Dictionary<Object, bool> hitDictionary = new Dictionary<Object, bool>();

    public virtual void Awake()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        if (combatCollider == null)
            TryGetComponent(out combatCollider);

        if (combatCollider != null)
            combatCollider.enabled = false;

        TryGetComponent(out sfxPlayer);
    }

    public void SetCombatController(HIT_TYPE hitType = HIT_TYPE.NONE, GUARD_TYPE guardType = GUARD_TYPE.NONE, float damageRatio = 1f, float crowdControlDuration = 0f)
    {
        this.hitType = hitType;
        this.guardType = guardType;
        this.damageRatio = damageRatio;
        this.crowdControlDuration = crowdControlDuration;
    }

    public void SetCombatInformation(CombatControllerInfo combatInformation)
    {
        this.hitType = combatInformation.hitType;
        this.guardType = combatInformation.guardType;
        this.damageRatio = combatInformation.damageRatio;
        this.crowdControlDuration = combatInformation.crowdControlDuration;
    }

    public IEnumerator CoPlaySFX(string sfxName, float delayTime = 0f)
    {
        yield return new WaitForSeconds(delayTime);
        if (sfxPlayer != null)
            sfxPlayer.PlaySFX(sfxName);
    }

    public HIT_TYPE HitType { get { return hitType; } }
    public GUARD_TYPE GuardType { get { return guardType; } }
    public float DamageRatio { get { return damageRatio; } }
    public float CrowdControlDuration { get { return crowdControlDuration; } }
    public Collider CombatCollider { get { return combatCollider; } }
    public Dictionary<Object, bool> HitDictionary { get { return hitDictionary; } }
}
