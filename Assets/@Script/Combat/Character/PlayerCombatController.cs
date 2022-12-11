using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCombatController : BaseCombatController
{
    [Header("Player CombatController")]
    protected BaseCharacter owner;
    protected Dictionary<COMBAT_TYPE, float> ratioDictionary;
    protected Dictionary<BaseEnemy, bool> hitDictionary = new Dictionary<BaseEnemy, bool>();

    public virtual void Initialize(BaseCharacter character)
    {
        base.Initialize();
        owner = character;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
            ExecuteCombatProcess(other);
    }

    public virtual void ExecuteCombatProcess(Collider other)
    {
        AttackProcess(other);
        DefenseProcess(other);
    }
    public void InvincibilityProcess()
    {

    }
    public void AttackProcess(Collider other)
    {
        if (other.TryGetComponent(out EnemyHitBox hitbox))
        {
            if(hitbox.Owner != null)
            {
                // 01 Invincibility Process
                if (hitbox.Owner.SubState == SUB_STATE.Invincible)
                {
                    InvincibilityProcess();
                    return;
                }

                // 02 Prevent Duplicate Damage Process
                if (hitDictionary.ContainsKey(hitbox.Owner))
                    return;

                hitDictionary.Add(hitbox.Owner, true);

                // 03 Hitting Effect Process
                GameObject effect = owner.RequestObject("Prefab_Effect_Player_Attack");
                effect.transform.position = other.bounds.ClosestPoint(transform.position); ;

                // 04 Damage Process
                owner.DamageProcess(hitbox.Owner, damageRatio);

                // 05 Enemy Interaction Process
                switch (combatType)
                {
                    // Hit
                    case COMBAT_TYPE.PlayerComboAttack1:
                    case COMBAT_TYPE.PlayerComboAttack2:
                    case COMBAT_TYPE.PlayerComboAttack3:
                    case COMBAT_TYPE.PlayerComboAttack4:
                        {
                            hitbox.Owner.OnHit();
                            break;
                        }
                    case COMBAT_TYPE.PlayerCounterAttack:
                        {
                            if(hitbox.Owner.SubState == SUB_STATE.Countable)
                            {
                                hitbox.Owner.OnStun();
                                break;
                            }
                            hitbox.Owner.OnHit();
                            break;
                        }                        
                    // Heavy Hit
                    case COMBAT_TYPE.PlayerSmashAttack1:
                    case COMBAT_TYPE.PlayerSmashAttack2:
                    case COMBAT_TYPE.PlayerSmashAttack3:
                    case COMBAT_TYPE.PlayerSmashAttack4:
                        {
                            hitbox.Owner.OnHeavyHit();
                            break;
                        }
                    // Stun
                    case COMBAT_TYPE.StunAttack:
                    case COMBAT_TYPE.PlayerParryingAttack:
                        {
                            hitbox.Owner.OnStun();
                            break;
                        }
                }
            }
        }
    }
    public void DefenseProcess(Collider other)
    {
        if (other.GetComponent<EnemyCombatController>() != null)
        {
            GameObject effect = null;
            switch (combatType)
            {
                case COMBAT_TYPE.PlayerDefense:
                    {
                        effect = owner.RequestObject("Prefab_Effect_Player_Defense");
                        owner.Animator.SetBool("isBreakShield", true);
                        break;
                    }

                case COMBAT_TYPE.PlayerParrying:
                    {
                        effect = owner.RequestObject("Prefab_Effect_Player_Parrying");

                        owner.Animator.SetBool("isPerfectShield", true);
                        owner.Animator.SetBool("isBreakShield", false);
                        break;
                    }
            }

            if (effect != null)
            {
                Vector3 triggerPoint = other.bounds.ClosestPoint(transform.position);
                effect.transform.position = triggerPoint;
            }

            combatCollider.enabled = false;
        }
    }

    #region Animation Event
    public void OnSetWeapon(COMBAT_TYPE requestType)
    {
        combatType = requestType;
        damageRatio = ratioDictionary[requestType];
        combatCollider.enabled = true;
    }
    public void OnReleaseWeapon()
    {
        combatCollider.enabled = false;
        hitDictionary.Clear();
    }
    #endregion

    public BaseCharacter Owner { get { return owner; } }
}
