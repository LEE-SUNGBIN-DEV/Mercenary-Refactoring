using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonStorm : EnemySkill
{
    public enum SKILL_STATE
    {
        OnStorm,
        OnStrike,
        OffStorm
    }
    [SerializeField] private int amount;
    [SerializeField] private float interval;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 30f;
        maxRange = 15f;

        owner.ObjectPooler.RegisterObject(Constants.VFX_Black_Dragon_Storm_Field, 1);
        owner.ObjectPooler.RegisterObject(Constants.VFX_Black_Dragon_Lightning_Strike, amount);
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        Owner.Animator.SetTrigger("doStorm");
    }

    public IEnumerator GenerateLightningStrike()
    {
        WaitForSeconds waitTime = new WaitForSeconds(interval);

        for (int i = 0; i < amount; ++i)
        {
            Vector3 generateCoordinate = Functions.GetRandomCircleCoordinate(12f);
            if (owner.ObjectPooler.RequestObject(Constants.VFX_Black_Dragon_Lightning_Strike).TryGetComponent(out EnemyPositioningAttack lightningStrike))
            {
                lightningStrike.SetCombatController(COMBAT_TYPE.Light_Attack, 1.3f, BUFF.Stun, 1.5f);
                lightningStrike.SetPositioningAttack(owner, owner.transform.position + generateCoordinate, 1f, 0.2f);
                lightningStrike.OnAttack();
            }

            yield return waitTime;
        }
    }

    #region Animation Event Function
    private void OnStorm(SKILL_STATE skillState)
    {
        switch(skillState)
        {
            case SKILL_STATE.OnStorm:
                GameObject stormField = owner.ObjectPooler.RequestObject(Constants.VFX_Black_Dragon_Storm_Field);
                if (stormField != null)
                    stormField.transform.position = owner.transform.position;
                break;
            case SKILL_STATE.OnStrike:
                StartCoroutine(GenerateLightningStrike());
                break;
            case SKILL_STATE.OffStorm:
                break;
        }
    }
    #endregion
}
