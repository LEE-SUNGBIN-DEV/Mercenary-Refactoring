using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : BaseCharacter
{
    public BerserkerWeapon BerserkerWeapon { get { return weapon as BerserkerWeapon; } }
    public BerserkerShield BerserkerShield { get { return shield as BerserkerShield; } }

    public override void Awake()
    {
        base.Awake();
        weapon = GetComponentInChildren<BerserkerWeapon>();
        shield = GetComponentInChildren<BerserkerShield>();
        weapon.SetWeapon(this);
        shield.SetShield(this);

        state = new BerserkerStateController(this);
    }

    protected override void Start()
    {
        base.Start();
        state.SetState(ACTION_STATE.PLAYER_IDLE);
    }

    protected override void Update()
    {
        base.Update();
        Managers.InputManager?.UpdateUIInput();
        buffController.UpdateBuff();
        state?.Update();
    }

    #region Animation Event
    private void OnEnableAttack(BERSERKER_ATTACK_TYPE attackType)
    {
        BerserkerWeapon.OnEnableAttack(attackType);
    }
    private void OnDisableAttack()
    {
        BerserkerWeapon.OnDisableAttack();
    }
    #endregion
}
