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
    [SerializeField] private EnemyCompeteAttack rightClaw;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 8f;
        maxRange = 8f;
        rightClaw = Functions.FindChild<EnemyCompeteAttack>(gameObject, "Compete Controller", true);
        rightClaw.SetCompeteAttack(owner);
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
                    rightClaw.SetCombatController(COMBAT_TYPE.ATTACK_STUN, 2f, 3f);
                    rightClaw.OnEnableCollider();
                    return;
                }
            case SKILL_STATE.OffRightClaw:
                {
                    rightClaw.OnDisableCollider();
                    return;
                }
        }
    }
    #endregion
}
