using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonLandBreath : EnemySkill
{
    [SerializeField] private EnemyBreath breath;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 30f;
        maxRange = 15f;

        breath = Functions.FindChild<EnemyBreath>(gameObject, "Breath Controller", true);
        breath.Initialize(owner);
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        StartCoroutine(OnLandBreath());
    }

    public IEnumerator OnLandBreath()
    {
        Owner.Animator.SetTrigger("doLandBreath");
        yield return new WaitUntil(() =>
        owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Land Breath") && owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1729f);
        breath.SetCombatController(HIT_TYPE.Light, CC_TYPE.None, 1f);
        breath.SetRay(20f, 0.15f);
        StartCoroutine(breath.RayCoroutine);

        yield return new WaitUntil(() =>
        owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Land Breath") && owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5413f);
        StopCoroutine(breath.RayCoroutine);
    }
}
