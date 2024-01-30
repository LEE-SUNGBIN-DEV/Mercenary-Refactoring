using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyCombatController : BaseCombatController
{
    public event UnityAction OnHitPlayer;

    [Header("Enemy Combat Controller")]
    [SerializeField] protected BaseEnemy enemy;
    protected Coroutine delayAttackCoroutine;

    protected virtual void ExecuteAttackProcess(Collider other)
    {
        // Hit With Character
        if (other.TryGetComponent(out PlayerCharacter character))
        {
            // Prevent Duplicate Damage Process
            if (hitDictionary.ContainsKey(character))
                return;

            hitDictionary.Add(character, true);
            OnHitPlayer?.Invoke();

            switch (character.HitState)
            {
                case HIT_STATE.INVINCIBLE:
                    break;

                case HIT_STATE.HITTABLE:
                    character.TakeHit(enemy, damageRatio, hitType, crowdControlDuration);
                    break;

                case HIT_STATE.GUARDABLE:
                    character.CurrentWeapon.GuardController.ExecuteDefenseProcess(this, other.ClosestPoint(other.transform.position));
                    break;

                case HIT_STATE.PARRYABLE:
                    character.CurrentWeapon.GuardController.ExecuteDefenseProcess(this, other.ClosestPoint(other.transform.position));
                    break;
            }
        }
    }

    // Delay
    public void OnDelayAttack(float delayTime, float attackDuration)
    {
        if (delayAttackCoroutine != null)
            StopCoroutine(delayAttackCoroutine);

        delayAttackCoroutine = StartCoroutine(CoDelayAttack(delayTime, attackDuration));
    }

    public IEnumerator CoDelayAttack(float delayTime, float attackDuration)
    {
        yield return new WaitForSeconds(delayTime);
        OnEnableCollider();
        yield return new WaitForSeconds(attackDuration);
        OnDisableCollider();
    }

    // Enable / Disable
    public virtual void OnEnableCollider()
    {
        if(combatCollider != null && combatCollider.enabled == false)
            combatCollider.enabled = true;
    }
    public virtual void OnDisableCollider()
    {
        if(combatCollider != null && combatCollider.enabled == true)
        {
            combatCollider.enabled = false;
            hitDictionary.Clear();
        }
    }

    #region Property
    public BaseEnemy Enemy { get { return enemy; } set { enemy = value; } }
    #endregion
}
