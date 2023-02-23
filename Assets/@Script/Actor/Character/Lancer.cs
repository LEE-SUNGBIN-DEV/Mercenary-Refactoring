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

        buffController = new BuffController(this);
        state = new LancerStateController(this);
    }

    protected override void Start()
    {
        base.Start();
        State.SetState(ACTION_STATE.PLAYER_WALK);
    }

    protected override void Update()
    {
        base.Update();
        Managers.InputManager.UpdateCombatInput();
        state?.TryStateSwitchingByWeight(NextState());
        state?.Update();
    }

    public ACTION_STATE NextState()
    {
        ACTION_STATE nextState = ACTION_STATE.PLAYER_WALK;

        if (Managers.InputManager.MouseLeftDown)
            nextState = state.CompareStateWeight(nextState, ACTION_STATE.PLAYER_ATTACK_LIGHT_01);

        if (Managers.InputManager.MouseRightDown || Managers.InputManager.MouseRightPress)
            nextState = state.CompareStateWeight(nextState, ACTION_STATE.PLAYER_DEFENSE_START);

        if (Managers.InputManager.IsSpaceKeyDown && Status.CurrentSP >= Constants.PLAYER_STAMINA_CONSUMPTION_ROLL)
            nextState = state.CompareStateWeight(nextState, ACTION_STATE.PLAYER_ROLL);

        if (Managers.InputManager.IsRKeyDown && Status.CurrentSP >= Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER)
            nextState = state.CompareStateWeight(nextState, ACTION_STATE.PLAYER_SKILL_COUNTER);

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
