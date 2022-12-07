using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : BaseCombatController
{
    [Header("Enemy Combat Controller")]
    protected Enemy owner;

    public override void Initialize()
    {
        base.Initialize();
        owner = GetComponentInParent<Enemy>(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
            ExecuteAttackProcess(other);
    }

    public virtual void ExecuteAttackProcess(Collider target)
    {
        if (target.TryGetComponent(out Character character))
        {
            if (character.IsInvincible)
            {
                InvincibilityProcess();
                return;
            }

            Functions.EnemyDamageProcess(owner, character, damageRatio);
            switch (combatType)
            {
                case COMBAT_TYPE.EnemyNormalAttack:
                    {
                        character.OnHit();
                        break;
                    }
                case COMBAT_TYPE.EnemySmashAttack:
                    {
                        character.OnHeavyHit();
                        break;
                    }
                case COMBAT_TYPE.EnemyCounterableAttack:
                case COMBAT_TYPE.EnemyCompetableAttack:
                case COMBAT_TYPE.StunAttack:
                    {
                        character.OnStun();
                        break;
                    }
            }
        }
    }
    public void ExecuteDamageProcess(Character character)
    {
        Functions.EnemyDamageProcess(owner, character, DamageRatio);
    }
    public void InvincibilityProcess()
    {

    }

    #region Property
    public Enemy Owner { get { return owner; } set { owner = value; } }
    #endregion
}
