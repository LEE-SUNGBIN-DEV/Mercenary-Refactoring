using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonDoubleAttack : EnemySkill
{
    [SerializeField] private EnemyCombatController leftClaw;
    [SerializeField] private EnemyCombatController rightClaw;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 16f;
        maxRange = 8f;

        leftClaw = Functions.FindChild<EnemyCombatController>(gameObject, "Left Claw Controller", true);
        rightClaw = Functions.FindChild<EnemyCombatController>(gameObject, "Right Claw Controller", true);
        leftClaw.Initialize(owner);
        rightClaw.Initialize(owner);
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        owner.Animator.SetTrigger("doDoubleClaw");
        StartCoroutine(OnDoubleClaw());
    }

    public IEnumerator OnDoubleClaw()
    {
        yield return new WaitUntil(() => owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.158f);
        leftClaw.SetCombatController(COMBAT_TYPE.EnemyNormalAttack, 1f);
        leftClaw.CombatCollider.enabled = true;

        yield return new WaitUntil(() => owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.198f);
        leftClaw.CombatCollider.enabled = false;

        yield return new WaitUntil(() => owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.594f);
        owner.SetMaterial("Outline");
        owner.MeshRenderer.material.color = Color.blue;

        yield return new WaitUntil(() => owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.693f);
        owner.SetMaterial("Default");
        owner.MeshRenderer.material.color = Color.white;
        rightClaw.SetCombatController(COMBAT_TYPE.StunAttack, 2f);
        rightClaw.CombatCollider.enabled = true;

        yield return new WaitUntil(() => owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.703f);
        rightClaw.CombatCollider.enabled = false;
    }

    #region Property
    public EnemyCombatController LeftClaw { get { return leftClaw; } }
    public EnemyCombatController RightClaw { get { return rightClaw; } }
    #endregion
}
