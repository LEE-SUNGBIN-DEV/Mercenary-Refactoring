using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerShield : CharacterCombatController
{
    public override void Initialize(Character character)
    {
        base.Initialize(character);
        combatType = COMBAT_TYPE.Defense;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy Attack"))
        {
            Vector3 triggerPoint = other.bounds.ClosestPoint(transform.position);

            switch (CombatType)
            {
                case COMBAT_TYPE.Defense:
                    {
                        Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_LANCER_DEFENSE, triggerPoint);
                        owner.Animator.SetBool("isBreakShield", true);
                        break;
                    }

                case COMBAT_TYPE.Parrying:
                    {
                        Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_LANCER_DEFENSE, triggerPoint);
                        Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_LANCER_PERFECT_DEFENSE, triggerPoint);

                        owner.Animator.SetBool("isPerfectShield", true);
                        owner.Animator.SetBool("isBreakShield", false);
                        break;
                    }
            }
            attackCollider.enabled = false;
        }

        if (other.CompareTag("Enemy"))
        {
            Vector3 triggerPoint = other.bounds.ClosestPoint(transform.position);
            if (CombatType == COMBAT_TYPE.ParryingAttack)
            {
                Enemy enemy = other.GetComponentInParent<Enemy>();
                owner.PlayerDamageProcess(enemy, DamageRatio);

                IStunable stunableObject = enemy.GetComponent<IStunable>();
                if (stunableObject != null)
                {
                    Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_PLAYER_COUNTER, triggerPoint);

                    stunableObject.Stun();
                }
            }
        }
    }

    public void StartDefense(COMBAT_TYPE targetType)
    {
        combatType = targetType;
    }
    public void EndDefense()
    {
        combatType = COMBAT_TYPE.Defense;
        attackCollider.enabled = false;
    }
}
