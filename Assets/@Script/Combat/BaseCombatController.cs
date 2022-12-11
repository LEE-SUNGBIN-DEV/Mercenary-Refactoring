using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseCombatController : MonoBehaviour
{
    [Header("Base Combat Controller")]
    protected COMBAT_TYPE combatType;
    [SerializeField] protected float damageRatio;
    [SerializeField] protected Collider combatCollider;
    [SerializeField] private ParticleSystem[] particleSystems;

    public virtual void Initialize()
    {
        TryGetComponent(out combatCollider);
        if(combatCollider != null)
            combatCollider.enabled = false;

        particleSystems = GetComponentsInChildren<ParticleSystem>(true);
    }
    public void SetCombatController(COMBAT_TYPE combatType, float damageRatio)
    {
        this.combatType = combatType;
        this.damageRatio = damageRatio;
    }
    public void PlayParticles()
    {
        for (int i = 0; i < particleSystems.Length; ++i)
        {
            particleSystems[i].Play();
        }
    }
    public void StopParticles()
    {
        for (int i = 0; i < particleSystems.Length; ++i)
        {
            particleSystems[i].Stop();
        }
    }
    public void SetParticlesDuration(float duration)
    {
        for (int i = 0; i < particleSystems.Length; ++i)
        {
            var main = particleSystems[i].main;
            main.duration = duration;
        }
    }

    public COMBAT_TYPE CombatType { get { return combatType; } }
    public float DamageRatio { get { return damageRatio; } }
    public Collider CombatCollider { get { return combatCollider; } }
}
