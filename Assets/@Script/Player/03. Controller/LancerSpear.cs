using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerSpear : CharacterCombatController
{
    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Vector3 triggerPoint = other.bounds.ClosestPoint(transform.position);

            Enemy monster = other.GetComponentInParent<Enemy>();
            owner.PlayerDamageProcess(monster, DamageRatio);

            switch (CombatType)
            {
                case COMBAT_TYPE.NORMAL:
                case COMBAT_TYPE.COUNTER_SKILL:
                    {
                        IHitable hitableObject = other.GetComponentInParent<IHitable>();
                        if (hitableObject != null)
                        {
                            Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_PLAYER_ATTACK, triggerPoint);

                            hitableObject.Hit();
                        }

                        break;
                    }
                case COMBAT_TYPE.SMASH:
                    {
                        IHeavyHitable heavyHitableObject = other.GetComponentInParent<IHeavyHitable>();
                        if (heavyHitableObject != null)
                        {
                            Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_PLAYER_SMASH, triggerPoint);

                            heavyHitableObject.HeavyHit();
                            CallSlowMotion(0.5f, 0.5f);
                        }

                        break;
                    }

                case COMBAT_TYPE.STUN:
                    {
                        IStunable stunableObject = other.GetComponentInParent<IStunable>();
                        if (stunableObject != null)
                        {
                            Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_PLAYER_SMASH, triggerPoint);

                            stunableObject.Stun();
                            CallSlowMotion(0.5f, 0.5f);
                        }
                        break;
                    }

                case COMBAT_TYPE.COUNTER:
                    {
                        IStunable stunableObject = other.GetComponentInParent<IStunable>();
                        if (stunableObject != null)
                        {
                            Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_PLAYER_COUNTER, triggerPoint);

                            stunableObject.Stun();
                            CallSlowMotion(0.2f, 0.5f);
                        }
                        break;
                    }
            }
        }
    }

    public void StartAttack(ATTACK_TYPE attackType)
    {
        switch (attackType)
        {
            case ATTACK_TYPE.COMBO1:
            case ATTACK_TYPE.COMBO2:
            case ATTACK_TYPE.COMBO3:
            case ATTACK_TYPE.COMBO4:
                {
                    damageRatio = 1f;
                    combatType = COMBAT_TYPE.NORMAL;
                    break;
                }

            case ATTACK_TYPE.SMASH1:
                {
                    damageRatio = 1.5f;
                    combatType = COMBAT_TYPE.SMASH;
                    break;
                }
            case ATTACK_TYPE.SMASH2:
                {
                    damageRatio = 2.5f;
                    combatType = COMBAT_TYPE.SMASH;
                    break;
                }
            case ATTACK_TYPE.SMASH3:
                {
                    damageRatio = 4f;
                    combatType = COMBAT_TYPE.SMASH;
                    break;
                }
            case ATTACK_TYPE.SMASH4:
                {
                    damageRatio = 3f;
                    combatType = COMBAT_TYPE.SMASH;
                    break;
                }
        }
        weaponCollider.enabled = true;
    }
    public void EndAttack()
    {
        weaponCollider.enabled = false;
    }
}
