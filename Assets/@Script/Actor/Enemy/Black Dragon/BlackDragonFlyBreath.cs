using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonFlyBreath : EnemySkill
{
    public enum SKILL_STATE
    {
        OnBreath,
        OffBreath,
    }
    [SerializeField] private EnemyBreath breath;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 45f;
        maxRange = 15f;

        breath = Functions.FindChild<EnemyBreath>(gameObject, "Breath Controller", true);

        owner.ObjectPooler.RegisterObject(Constants.VFX_Enemy_Breath, 15);
        owner.ObjectPooler.RegisterObject(Constants.VFX_Enemy_Flame_Area, 15);
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        Owner.Animator.SetTrigger("doFlyBreath");
    }

    #region Animation Event Function
    private void OnFlyBreath(SKILL_STATE skillState)
    {
        switch (skillState)
        {
            case SKILL_STATE.OnBreath:
                breath.SetCombatController(HIT_TYPE.Light, CROWD_CONTROL_TYPE.None, 1f);
                breath.SetRayAttack(owner, 30f, 0.1f);
                StartCoroutine(breath.RayCoroutine);
                break;
            case SKILL_STATE.OffBreath:
                StopCoroutine(breath.RayCoroutine);
                break;
        }
    }
    #endregion
}
