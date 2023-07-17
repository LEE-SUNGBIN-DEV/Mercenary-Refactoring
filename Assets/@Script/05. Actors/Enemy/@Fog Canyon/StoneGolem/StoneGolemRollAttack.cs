using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGolemRollAttack : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack rollAttack;
    private AnimationClipInformation attackAnimationInfo;
    private AnimationClipInformation finishAnimationInfo;
    private bool isHitPlayer;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Roll_Attack";
        cooldown = 12f;
        minAttackDistance = 0f;
        maxAttackDistance = 4f;

        rollAttack = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Roll_Attack_Controller", true);
        rollAttack.SetMeleeAttack(enemy);

        attackAnimationInfo = enemy.AnimationClipTable["Skill_Roll_Attack"];
        finishAnimationInfo = enemy.AnimationClipTable["Skill_Roll_Attack_Finish"];
    }

    public override IEnumerator CoStartSkill()
    {
        rollAttack.OnHitPlayer += HitPlayer;
        isHitPlayer = false;

        enemy.Animator.Play(attackAnimationInfo.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(attackAnimationInfo, 13));
        rollAttack.SetCombatController(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.8f);
        rollAttack.OnEnableCollider();

        while(!enemy.Animator.IsAnimationFrameUpTo(attackAnimationInfo, attackAnimationInfo.maxFrame))
        {
            if (isHitPlayer)
                break;

            enemy.MoveController.SetMovementAndRotation(transform.forward, 10f);
            yield return null;
        }
        rollAttack.OnDisableCollider();
        rollAttack.OnHitPlayer -= HitPlayer;
        enemy.MoveController.SetMovementAndRotation(Vector3.zero, 0f);

        enemy.Animator.Play(finishAnimationInfo.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(finishAnimationInfo, finishAnimationInfo.maxFrame));
        EndSkill();
    }

    public override IEnumerator CoLookTarget()
    {
        while (!enemy.Animator.IsAnimationFrameUpTo(attackAnimationInfo, 13))
        {
            enemy.LookTarget();
            yield return null;
        }
    }

    public override void DisableSkill()
    {
        base.DisableSkill();
        rollAttack.OnHitPlayer -= HitPlayer;
        enemy.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    public void HitPlayer()
    {
        isHitPlayer = true;
    }
}
