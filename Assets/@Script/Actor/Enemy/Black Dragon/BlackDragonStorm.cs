using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonStorm : EnemySkill
{
    [SerializeField] private EnemyStormField stormField;
    [SerializeField] private int amount;
    [SerializeField] private float interval;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 30f;
        maxRange = 15f;

        stormField = Functions.FindChild<EnemyStormField>(gameObject, "Storm Field", true);
        stormField.Initialize(owner, amount, interval);
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        StartCoroutine(OnLandBreath());
    }

    public IEnumerator OnLandBreath()
    {
        Owner.Animator.SetTrigger("doStorm");
        stormField.enabled = true;

        yield return new WaitUntil(() =>
        owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Storm") && owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f);

        yield return new WaitUntil(() =>
        owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Storm") && owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5413f);

        yield return new WaitUntil(() =>
        owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Storm End") && owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        stormField.enabled = false;
    }

    
}
