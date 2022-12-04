using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerSpear : CharacterCombatController
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Vector3 triggerPoint = other.bounds.ClosestPoint(transform.position);

            Enemy monster = other.GetComponentInParent<Enemy>();
            owner.PlayerDamageProcess(monster, DamageRatio);

            switch (CombatType)
            {
                case COMBAT_TYPE.DefaultAttack:
                case COMBAT_TYPE.Counter:
                    {
                        IHitable hitableObject = other.GetComponentInParent<IHitable>();
                        if (hitableObject != null)
                        {
                            Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_PLAYER_ATTACK, triggerPoint);

                            hitableObject.Hit();
                        }

                        break;
                    }
                case COMBAT_TYPE.SmashAttack:
                    {
                        IHeavyHitable heavyHitableObject = other.GetComponentInParent<IHeavyHitable>();
                        if (heavyHitableObject != null)
                        {
                            Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_PLAYER_SMASH, triggerPoint);

                            heavyHitableObject.HeavyHit();
                        }

                        break;
                    }

                case COMBAT_TYPE.StunAttack:
                    {
                        IStunable stunableObject = other.GetComponentInParent<IStunable>();
                        if (stunableObject != null)
                        {
                            Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_PLAYER_SMASH, triggerPoint);

                            stunableObject.Stun();
                        }
                        break;
                    }

                case COMBAT_TYPE.ParryingAttack:
                    {
                        IStunable stunableObject = other.GetComponentInParent<IStunable>();
                        if (stunableObject != null)
                        {
                            Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_PLAYER_COUNTER, triggerPoint);

                            stunableObject.Stun();
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
                    combatType = COMBAT_TYPE.DefaultAttack;
                    break;
                }

            case ATTACK_TYPE.SMASH1:
                {
                    damageRatio = 1.5f;
                    combatType = COMBAT_TYPE.SmashAttack;
                    break;
                }
            case ATTACK_TYPE.SMASH2:
                {
                    damageRatio = 2.5f;
                    combatType = COMBAT_TYPE.SmashAttack;
                    break;
                }
            case ATTACK_TYPE.SMASH3:
                {
                    damageRatio = 4f;
                    combatType = COMBAT_TYPE.SmashAttack;
                    break;
                }
            case ATTACK_TYPE.SMASH4:
                {
                    damageRatio = 3f;
                    combatType = COMBAT_TYPE.SmashAttack;
                    break;
                }
        }
        attackCollider.enabled = true;
    }
    public void EndAttack()
    {
        attackCollider.enabled = false;
    }
}
