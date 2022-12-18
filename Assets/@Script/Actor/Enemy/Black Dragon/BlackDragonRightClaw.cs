using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonRightClaw : EnemySkill
{
    public enum SKILL_STATE
    {
        OnRightClaw,
        OffRightClaw
    }
    [SerializeField] private EnemyMeleeAttack rightClaw;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 8f;
        maxRange = 8f;
        rightClaw = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Right Claw Controller", true);
        rightClaw.SetMeleeAttack(owner);
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        Owner.Animator.SetTrigger("doRightClaw");
    }

    #region Animation Event Function
    private void OnRightClaw(SKILL_STATE skillState)
    {
        switch(skillState)
        {
            case SKILL_STATE.OnRightClaw:
                {
                    rightClaw.SetCombatController(HIT_TYPE.Heavy, ABNORMAL_STATE.Stun, 2f);
                    rightClaw.CombatCollider.enabled = true;
                    return;
                }
            case SKILL_STATE.OffRightClaw:
                {
                    rightClaw.CombatCollider.enabled = false;
                    return;
                }
        }
    }
    #endregion
}
