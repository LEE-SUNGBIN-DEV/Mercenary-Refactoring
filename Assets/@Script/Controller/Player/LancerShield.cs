using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerShield : CharacterCombatController
{
    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        weaponCollider.enabled = false;
        combatType = COMBAT_TYPE.DEFENSE;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy Attack"))
        {
            Vector3 triggerPoint = other.bounds.ClosestPoint(transform.position);

            switch (CombatType)
            {
                case COMBAT_TYPE.DEFENSE:
                    {
                        Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_LANCER_DEFENSE, triggerPoint);
                        owner.Animator.SetBool("isBreakShield", true);
                        break;
                    }

                case COMBAT_TYPE.PARRYING:
                    {
                        Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_LANCER_DEFENSE, triggerPoint);
                        Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_LANCER_PERFECT_DEFENSE, triggerPoint);

                        owner.Animator.SetBool("isPerfectShield", true);
                        owner.Animator.SetBool("isBreakShield", false);
                        CallSlowMotion(0.5f, 0.5f);
                        break;
                    }
            }
            WeaponCollider.enabled = false;
        }

        if (other.CompareTag("Enemy"))
        {
            Vector3 triggerPoint = other.bounds.ClosestPoint(transform.position);
            if (CombatType == COMBAT_TYPE.COUNTER)
            {
                Enemy enemy = other.GetComponentInParent<Enemy>();
                owner.PlayerDamageProcess(enemy, DamageRatio);

                IStunable stunableObject = enemy.GetComponent<IStunable>();
                if (stunableObject != null)
                {
                    Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_PLAYER_COUNTER, triggerPoint);

                    stunableObject.Stun();
                    CallSlowMotion(0.2f, 0.5f);
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
        combatType = COMBAT_TYPE.DEFENSE;
        weaponCollider.enabled = false;
    }
}
