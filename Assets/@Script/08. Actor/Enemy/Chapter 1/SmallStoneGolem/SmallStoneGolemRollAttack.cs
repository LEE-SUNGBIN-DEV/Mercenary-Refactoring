using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallStoneGolemRollAttack : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack rollAttack;
    private AnimationInfo animationInfo;
    private Vector3 attackDirection;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Roll_Attack";
        cooldown = 12f;
        minAttackDistance = 0f;
        maxAttackDistance = 4f;

        rollAttack = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Roll_Attack_Controller", true);
        rollAttack.SetMeleeAttack(enemy);

        animationInfo = new AnimationInfo(skillName, 2.958f, 71, 1f);
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(animationInfo.animationNameHash);
        attackDirection = enemy.TargetDirection;
        attackDirection.y = 0f;

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 13));
        rollAttack.SetCombatController(COMBAT_TYPE.ATTACK_HEAVY, 1.8f);
        rollAttack.OnEnableCollider();

        while(!enemy.Animator.IsAnimationFrameUpTo(animationInfo, 40))
        {
            enemy.CharacterController.SimpleMove(10f * attackDirection);
            yield return null;
        }
        rollAttack.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, animationInfo.maxFrame));
        EndSkill();
    }
}
