using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCombatController : BaseCombatController
{
    [Header("Player CombatController")]
    protected Character owner;
    protected Dictionary<COMBAT_TYPE, float> ratioDictionary;

    public virtual void Initialize(Character character)
    {
        base.Initialize();
        owner = character;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
            ExecuteAttackProcess(other);
    }

    public virtual void ExecuteAttackProcess(Collider other)
    {
        // Attack
        if (other.TryGetComponent(out Enemy enemy))
        {
            if (enemy.IsInvincible)
            {
                InvincibilityProcess();
                return;
            }

            owner.PlayerDamageProcess(enemy, damageRatio);
            Vector3 triggerPoint = other.bounds.ClosestPoint(transform.position);
            switch (combatType)
            {
                case COMBAT_TYPE.PlayerComboAttack1:
                case COMBAT_TYPE.PlayerComboAttack2:
                case COMBAT_TYPE.PlayerComboAttack3:
                case COMBAT_TYPE.PlayerComboAttack4:
                case COMBAT_TYPE.PlayerCounterAttack:
                    {
                        GameObject effect = owner.ObjectPoolController.RequestObject("Prefab_Effect_Player_Normal_Attack");
                        effect.transform.position = triggerPoint;
                        enemy.OnHit();
                        break;
                    }
                case COMBAT_TYPE.PlayerSmashAttack1:
                case COMBAT_TYPE.PlayerSmashAttack2:
                case COMBAT_TYPE.PlayerSmashAttack3:
                case COMBAT_TYPE.PlayerSmashAttack4:
                    {
                        GameObject effect = owner.ObjectPoolController.RequestObject("Prefab_Effect_Player_Smash_Attack");
                        effect.transform.position = triggerPoint;
                        enemy.OnHeavyHit();
                        break;
                    }
                case COMBAT_TYPE.StunAttack:
                case COMBAT_TYPE.PlayerParryingAttack:
                    {
                        GameObject effect = owner.ObjectPoolController.RequestObject("Prefab_Effect_Player_Parrying_Attack");
                        effect.transform.position = triggerPoint;
                        enemy.OnStun();
                        break;
                    }
            }
        }

        // Defense
        if (other.GetComponent<EnemyCombatController>() != null)
        {
            GameObject effect = null;
            switch (CombatType)
            {
                case COMBAT_TYPE.PlayerDefense:
                    {
                        effect = owner.ObjectPoolController.RequestObject("Prefab_Effect_Player_Defense");
                        owner.Animator.SetBool("isBreakShield", true);
                        break;
                    }

                case COMBAT_TYPE.PlayerParrying:
                    {
                        effect = owner.ObjectPoolController.RequestObject("Prefab_Effect_Player_Parrying");

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
    public void ExecuteDamageProcess(Enemy enemy)
    {
        owner.PlayerDamageProcess(enemy, damageRatio);
    }
    public void InvincibilityProcess()
    {

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
    }
    #endregion
    public Character Owner { get { return owner; } }
}
