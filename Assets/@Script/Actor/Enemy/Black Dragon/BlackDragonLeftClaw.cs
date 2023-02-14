using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonLeftClaw : EnemySkill
{
    public enum SKILL_STATE
    {
        OnLeftClaw,
        OffLeftClaw
    }
    [SerializeField] private EnemyMeleeAttack leftClaw;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 8f;
        maxRange = 8f;
        leftClaw = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Left Claw Controller", true);
        leftClaw.SetMeleeAttack(owner);
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        Owner.Animator.SetTrigger("doLeftClaw");
    }

    #region Animation Event Function
    private void OnLeftClaw(SKILL_STATE skillState)
    {
        switch (skillState)
        {
            case SKILL_STATE.OnLeftClaw:
                {
                    leftClaw.SetCombatController(COMBAT_TYPE.Light_Attack, 1f);
                    leftClaw.OnEnableCollider();
                    return;
                }
            case SKILL_STATE.OffLeftClaw:
                {
                    leftClaw.OnDisableCollider();
                    return;
                }
        }
    }
    #endregion
}
