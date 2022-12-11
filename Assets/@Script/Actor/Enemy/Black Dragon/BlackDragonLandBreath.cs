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
        yield return new WaitUntil(() => owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1729f);
        breath.SetCombatController(COMBAT_TYPE.EnemyNormalAttack, 1f);
        breath.SetParticlesDuration(2.1f);
        breath.PlayParticles();
        breath.SetRay(20f, 0.2f);

        while(owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5488f)
        {
            StartCoroutine(breath.RayCoroutine);
            yield return null;
        }
        StopCoroutine(breath.RayCoroutine);
    }
}
