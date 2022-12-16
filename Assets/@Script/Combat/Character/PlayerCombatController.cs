using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCombatController : BaseCombatController
{
    [Header("Player CombatController")]
    protected BaseCharacter owner;
    protected Dictionary<PLAYER_ATTACK_TYPE, float> ratioDictionary;
    protected Dictionary<BaseEnemy, bool> hitDictionary = new Dictionary<BaseEnemy, bool>();

    public virtual void SetWeapon(BaseCharacter character)
    {
        owner = character;
        owner.ObjectPooler.RegisterObject(Constants.VFX_Player_Attack, 12);
        owner.ObjectPooler.RegisterObject(Constants.VFX_Player_Defense, 3);
        owner.ObjectPooler.RegisterObject(Constants.VFX_Player_Parrying, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
            ExecuteCombatProcess(other);
    }

    public virtual void ExecuteCombatProcess(Collider other)
    {
        AttackProcess(other);
        DefenseProcess(other);
    }
    public void AttackProcess(Collider other)
    {
        if (other.TryGetComponent(out EnemyHitBox hitbox))
        {
            if(hitbox.Owner != null)
            {
                // 01 Invincibility Process
                if (hitbox.Owner.HitState == HIT_STATE.Invincible)
                    return;

                // 02 Prevent Duplicate Damage Process
                if (hitDictionary.ContainsKey(hitbox.Owner))
                    return;

                hitDictionary.Add(hitbox.Owner, true);
                
                // 03 Hitting Effect Process
                GameObject effect = Managers.SceneManagerCS.CurrentScene.RequestObject(Constants.VFX_Player_Attack);
                effect.transform.position = other.bounds.ClosestPoint(transform.position);

                // 04 Damage Process
                owner.DamageProcess(hitbox.Owner, damageRatio);

                // 05 Hit Process
                switch (combatType)
                {
                    case HIT_TYPE.Light:
                        hitbox.Owner.OnHit();
                        break;
                    case HIT_TYPE.Heavy:
                        hitbox.Owner.OnHeavyHit();
                        break;
                }

                // 06 CC Process
                switch (crowdControlType)
                {
                    case CROWD_CONTROL_TYPE.None:
                        break;
                    case CROWD_CONTROL_TYPE.Stun:
                        hitbox.Owner.OnStun();
                        break;
                }
            }
        }
    }
    public void DefenseProcess(Collider other)
    {
        if (other.GetComponent<EnemyMeleeAttack>() != null)
        {
            GameObject effect = null;
            switch (combatType)
            {
                case HIT_TYPE.Defense:
                    {
                        effect = Managers.SceneManagerCS.CurrentScene.RequestObject(Constants.VFX_Player_Defense);
                        owner.Animator.SetBool("isBreaked", true);
                        break;
                    }

                case HIT_TYPE.Parrying:
                    {
                        effect = Managers.SceneManagerCS.CurrentScene.RequestObject(Constants.VFX_Player_Parrying);

                        owner.Animator.SetBool("isParrying", true);
                        owner.Animator.SetBool("isBreaked", false);
                        break;
                    }
            }

            if (effect != null)
            {
                Vector3 triggerPoint = other.bounds.ClosestPoint(transform.position);
                effect.transform.position = triggerPoint;
            }

            combatCollider.enabled = false;
        }
    }

    #region Called by Owner's Animation Event
    public void OnSetWeapon(PLAYER_ATTACK_TYPE playerAttackType)
    {
        switch(playerAttackType)
        {
            case PLAYER_ATTACK_TYPE.PlayerCounterAttack:
            case PLAYER_ATTACK_TYPE.PlayerComboAttack1:
            case PLAYER_ATTACK_TYPE.PlayerComboAttack2:
            case PLAYER_ATTACK_TYPE.PlayerComboAttack3:
            case PLAYER_ATTACK_TYPE.PlayerComboAttack4:
                {
                    combatType = HIT_TYPE.Light;
                    break;
                }
            case PLAYER_ATTACK_TYPE.PlayerSmashAttack1:
            case PLAYER_ATTACK_TYPE.PlayerSmashAttack2:
            case PLAYER_ATTACK_TYPE.PlayerSmashAttack3:
            case PLAYER_ATTACK_TYPE.PlayerSmashAttack4:
                {
                    combatType = HIT_TYPE.Heavy;
                    break;
                }
            case PLAYER_ATTACK_TYPE.PlayerDefense:
                {
                    combatType = HIT_TYPE.Defense;
                    break;
                }
            case PLAYER_ATTACK_TYPE.PlayerParrying:
                {
                    combatType = HIT_TYPE.Parrying;
                    break;
                }
        }
        damageRatio = ratioDictionary[playerAttackType];
        combatCollider.enabled = true;
    }
    public void OnReleaseWeapon()
    {
        combatCollider.enabled = false;
        hitDictionary.Clear();
    }
    #endregion

    public BaseCharacter Owner { get { return owner; } }
}
