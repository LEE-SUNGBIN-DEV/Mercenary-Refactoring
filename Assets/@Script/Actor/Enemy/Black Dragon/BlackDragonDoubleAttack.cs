using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonDoubleAttack : EnemySkill
{
    public enum SKILL_STATE
    {
        OnLeftClaw,
        OffLeftClaw,
        OnCounterable,
        OnRightClaw,
        OffRightClaw
    }
    [SerializeField] private EnemyMeleeAttack leftClaw;
    [SerializeField] private EnemyMeleeAttack rightClaw;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 16f;
        maxRange = 8f;

        leftClaw = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Left Claw Controller", true);
        rightClaw = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Right Claw Controller", true);
        leftClaw.SetMeleeAttack(owner);
        rightClaw.SetMeleeAttack(owner);
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        owner.Animator.SetTrigger("doDoubleClaw");
    }

    #region Animation Event Function
    private void OnDoubleClaw(SKILL_STATE skillState)
    {
        switch(skillState)
        {
            case SKILL_STATE.OnLeftClaw:
                {
                    leftClaw.SetCombatController(HIT_TYPE.Light, 1f);
                    leftClaw.OnEnableCollider();
                    break;
                }
            case SKILL_STATE.OffLeftClaw:
                {
                    leftClaw.OnDisableCollider();
                    break;
                }
            case SKILL_STATE.OnCounterable:
                {
                    owner.SetMaterial("Outline");
                    owner.MeshRenderer.material.color = Color.blue;
                    break;
                }
            case SKILL_STATE.OnRightClaw:
                {
                    owner.SetMaterial("Default");
                    owner.MeshRenderer.material.color = Color.white;
                    rightClaw.SetCombatController(HIT_TYPE.Heavy, 1.2f, ABNORMAL_TYPE.Stun, 4f);
                    rightClaw.OnEnableCollider();
                    break;
                }
            case SKILL_STATE.OffRightClaw:
                {
                    rightClaw.OnDisableCollider();
                    break;
                }
        }
    }
    #endregion
}
