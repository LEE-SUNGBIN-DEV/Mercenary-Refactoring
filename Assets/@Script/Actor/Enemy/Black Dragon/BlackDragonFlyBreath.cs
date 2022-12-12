using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonFlyBreath : EnemySkill
{
    [SerializeField] private EnemyBreath breath;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 45f;
        maxRange = 15f;

        breath = Functions.FindChild<EnemyBreath>(gameObject, "Breath Controller", true);
        breath.Initialize(owner);
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        StartCoroutine(OnFlyBreath());
    }

    public IEnumerator OnFlyBreath()
    {
        Owner.Animator.SetTrigger("doFlyBreath");
        yield return new WaitUntil(() =>
        owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Fly Breath") && owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3926);
        breath.SetCombatController(COMBAT_TYPE.EnemyNormalAttack, 1f);
        breath.SetRay(30f, 0.1f);
        StartCoroutine(breath.RayCoroutine);

        yield return new WaitUntil(() =>
        owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Fly Breath") && owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.740f);
        StopCoroutine(breath.RayCoroutine);
    }
}
