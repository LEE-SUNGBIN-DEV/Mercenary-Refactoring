using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lancer : Character
{
    [SerializeField] private LancerSpear spear;
    [SerializeField] private LancerShield shield;
    [SerializeField] private CharacterCombatController skill; 

    protected override void Awake()
    {
        base.Awake();
        state = new LancerStateController(this);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        State.SwitchCharacterState(CHARACTER_STATE.MOVE);
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
        CHARACTER_STATE nextState = CHARACTER_STATE.MOVE;

        if (playerInput.IsMouseLeftDown)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.ATTACK);

        if (playerInput.IsMouseRightDown)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.LANCER_DEFENSE);

        if (playerInput.IsSpaceKeyDown && StatusData.CurrentSP >= Constants.CHARACTER_STAMINA_CONSUMPTION_ROLL)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.ROLL);

        if (playerInput.IsRKeyDown && StatusData.CurrentSP >= Constants.CHARACTER_STAMINA_CONSUMPTION_COUNTER)
            nextState = state.CompareStateWeight(nextState, CHARACTER_STATE.SKILL);

        return nextState;
    }

    #region Animation Event Function
    // Weapon
    private void OnStartAttack(ATTACK_TYPE attackType)
    {
        spear.StartAttack(attackType);
    }
    private void OnEndAttack()
    {
        spear.EndAttack();
    }
    private void OnStartDefense(COMBAT_TYPE combatType)
    {
        shield.StartDefense(combatType);
    }
    private void OnEndDefense()
    {
        shield.EndDefense();
    }
    private void OnStartSkill()
    {
        skill.StartSkill();
    }
    private void OnEndSkill()
    {
        skill.EndSkill();
    }
    #endregion

    #region Property
    public LancerSpear Spear { get { return spear; } }
    public LancerShield Shield { get { return shield; } }
    #endregion
}
