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
        state.SwitchState(CHARACTER_STATE.Move);
    }

    protected override void Update()
    {
        base.Update();
        Managers.InputManager?.UpdateUIInput();
        if (!abnormalStateController.UpdateState())
        {
            Managers.InputManager?.UpdateMoveInput();
            Managers.InputManager?.UpdateCombatInput();
            state?.TrySwitchState(NextState());
        }
        state?.Update();
    }

    public override CHARACTER_STATE NextState()
    {
        CHARACTER_STATE nextState = CHARACTER_STATE.Move;

        if (Managers.InputManager.MouseLeftDown || Managers.InputManager.MouseLeftPress)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.Combo_1);

        if (Managers.InputManager.MouseRightDown || Managers.InputManager.MouseRightPress)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.Defense);

        if (Managers.InputManager.IsSpaceKeyDown && StatusData.CurrentSP >= Constants.CHARACTER_STAMINA_CONSUMPTION_ROLL)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.Roll);

        if (Managers.InputManager.IsRKeyDown && StatusData.CurrentSP >= Constants.CHARACTER_STAMINA_CONSUMPTION_COUNTER)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.Skill);

        return nextState;
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
    private void OnEnableDefense(BERSERKER_DEFENSE_TYPE attackType)
    {
        BerserkerShield.OnEnableDefense(attackType);
    }
    private void OnDisableDefense()
    {
        BerserkerShield.OnDisableDefense();
    }
    #endregion
}
