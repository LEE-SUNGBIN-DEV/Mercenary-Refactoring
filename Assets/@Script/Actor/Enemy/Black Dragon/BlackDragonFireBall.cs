using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonFireBall : EnemySkill
{
    [SerializeField] private float minRange;
    [SerializeField] private Transform muzzle;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 5;
        minRange = 15f;
        maxRange = 30f;

        muzzle = Functions.FindChild<Transform>(gameObject, "Muzzle", true);
        Managers.SceneManagerCS.CurrentScene.RegisterObject("Prefab_Projectile_Enemy_Fire_Ball", 3);
    }

    public override bool CheckCondition(float targetDistance)
    {
        return (isReady && (targetDistance <= maxRange) && (targetDistance >= minRange));
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        StartCoroutine(OnLandBreath());
    }

    public IEnumerator OnLandBreath()
    {
        Owner.Animator.SetTrigger("doFireBall");
        yield return new WaitUntil(() =>
        owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Fire Ball") && owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.428f);
        
        GameObject fireBall = Managers.SceneManagerCS.CurrentScene.RequestObject("Prefab_Projectile_Enemy_Fire_Ball");
        fireBall.transform.position = muzzle.transform.position;

        if (fireBall.TryGetComponent(out EnemyProjectile projectile))
        {
            projectile.Initialize(owner);
            projectile.SetCombatController(COMBAT_TYPE.EnemySmashAttack, 1.5f);
            projectile.SetProjectile(15f, transform.forward);
        }
    }
}
