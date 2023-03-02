using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BaseCharacter
{
    public GreatSword playerWeapon { get { return weapon as GreatSword; } }
    public GreatSwordShield playerShield { get { return shield as GreatSwordShield; } }

    public override void Awake()
    {
        base.Awake();
        weapon = GetComponentInChildren<GreatSword>();
        shield = GetComponentInChildren<GreatSwordShield>();
        weapon.SetWeapon(this);
        shield.SetShield(this);
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
        state?.Update();
    }

    #region Animation Event
    private void OnEnableAttack(GREAT_SWORD_ATTACK_TYPE attackType)
    {
        playerWeapon.OnEnableAttack(attackType);
    }
    private void OnDisableAttack()
    {
        playerWeapon.OnDisableAttack();
    }
    #endregion
}
