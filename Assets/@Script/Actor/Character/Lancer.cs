using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lancer : BaseCharacter
{
    [SerializeField] private LancerWeapon weapon;
    [SerializeField] private LancerShield shield;

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
        State.SwitchCharacterState(CHARACTER_STATE.Move);
    }

    protected override void Update()
    {
        base.Update();
        playerInput?.UpdateCharacterInput();
        state?.TrySwitchCharacterState(NextCharacterState());
        state?.Update();
    }

    public override CHARACTER_STATE NextCharacterState()
    {
        CHARACTER_STATE nextState = CHARACTER_STATE.Move;

        if (playerInput.IsMouseLeftDown)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.Attack);

        if (playerInput.IsMouseRightDown)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.Defense);

        if (playerInput.IsSpaceKeyDown && StatusData.CurrentSP >= Constants.CHARACTER_STAMINA_CONSUMPTION_ROLL)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.Roll);

        if (playerInput.IsRKeyDown && StatusData.CurrentSP >= Constants.CHARACTER_STAMINA_CONSUMPTION_COUNTER)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.Skill);

        return nextState;
    }

    #region Animation Event
    private void OnEnableAttack(LANCER_ATTACK_TYPE attackType)
    {
        weapon.OnEnableAttack(attackType);
    }
    private void OnDisableAttack()
    {
        weapon.OnDisableAttack();
    }
    private void OnEnableDefense(LANCER_DEFENSE_TYPE attackType)
    {
        shield.OnEnableDefense(attackType);
    }
    private void OnDisableDefense()
    {
        shield.OnDisableDefense();
    }
    #endregion

    #region Property
    public LancerWeapon Spear { get { return weapon; } }
    public LancerShield Shield { get { return shield; } }
    #endregion
}
