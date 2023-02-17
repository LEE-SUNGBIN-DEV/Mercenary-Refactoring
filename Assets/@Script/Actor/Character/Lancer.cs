using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lancer : BaseCharacter
{
    public LancerWeapon LancerWeapon { get { return weapon as LancerWeapon; } }
    public LancerShield LancerShield { get { return shield as LancerShield; } }

    public override void Awake()
    {
        base.Awake();
        weapon = GetComponentInChildren<LancerWeapon>();
        shield = GetComponentInChildren<LancerShield>();
        weapon.SetWeapon(this);
        shield.SetShield(this);

        abnormalStateController = new AbnormalStateController(this);
        state = new LancerStateController(this);
    }

    protected override void Start()
    {
        base.Start();
        State.SetState(CHARACTER_STATE.Walk);
    }

    protected override void Update()
    {
        base.Update();
        Managers.InputManager.UpdateCombatInput();
        state?.TryStateSwitchingByWeight(NextState());
        state?.Update();
    }

    public CHARACTER_STATE NextState()
    {
        CHARACTER_STATE nextState = CHARACTER_STATE.Walk;

        if (Managers.InputManager.MouseLeftDown)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.Light_Attack_01);

        if (Managers.InputManager.MouseRightDown || Managers.InputManager.MouseRightPress)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.Defense);

        if (Managers.InputManager.IsSpaceKeyDown && StatusData.CurrentSP >= Constants.PLAYER_STAMINA_CONSUMPTION_ROLL)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.Roll);

        if (Managers.InputManager.IsRKeyDown && StatusData.CurrentSP >= Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.Skill);

        return nextState;
    }

    #region Animation Event
    private void OnEnableAttack(LANCER_ATTACK_TYPE attackType)
    {
        LancerWeapon.OnEnableAttack(attackType);
    }
    private void OnDisableAttack()
    {
        LancerWeapon.OnDisableAttack();
    }
    private void OnEnableDefense(DEFENSE_TYPE attackType)
    {
        LancerShield.OnEnableDefense(attackType);
    }
    private void OnDisableDefense()
    {
        LancerShield.OnDisableDefense();
    }
    #endregion
}
