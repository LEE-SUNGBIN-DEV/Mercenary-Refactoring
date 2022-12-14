using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lancer : BaseCharacter
{
    [SerializeField] private LancerWeapon spear;
    [SerializeField] private LancerShield shield;

    public override void Awake()
    {
        base.Awake();
        state = new LancerStateController(this);
        spear = GetComponentInChildren<LancerWeapon>();
        shield = GetComponentInChildren<LancerShield>();
        spear.Initialize(this);
        shield.Initialize(this);
    }

    protected override void Start()
    {
        base.Start();
        State.SwitchCharacterState(CHARACTER_STATE.Move);
    }

    protected override void Update()
    {
        base.Update();
        playerInput?.GetPlayerInput();
        state?.SwitchCharacterStateByWeight(DetermineCharacterState());
        state?.CurrentState?.Update(this);
    }

    public override CHARACTER_STATE DetermineCharacterState()
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

    #region Animation Event Function
    private void OnSetWeapon(PLAYER_ATTACK_TYPE playerAttackType)
    {
        spear.OnSetWeapon(playerAttackType);
    }
    private void OnReleaseWeapon()
    {
        spear.OnReleaseWeapon();
    }
    private void OnSetShield(PLAYER_ATTACK_TYPE playerAttackType)
    {
        shield.OnSetWeapon(playerAttackType);
    }
    private void OnReleaseShield()
    {
        shield.OnReleaseWeapon();
    }
    #endregion

    #region Property
    public LancerWeapon Spear { get { return spear; } }
    public LancerShield Shield { get { return shield; } }
    #endregion
}
