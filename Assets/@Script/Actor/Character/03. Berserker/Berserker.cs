using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : BaseCharacter
{
    [Header("Berserker")]
    [SerializeField] private BerserkerWeapon weapon;

    public override void Awake()
    {
        base.Awake();

        weapon = GetComponentInChildren<BerserkerWeapon>();
        weapon.SetWeapon(this);

        abnormalStateController = new AbnormalStateController(this);
        state = new BerserkerStateController(this);
    }

    protected override void Start()
    {
        base.Start();
        State.SwitchCharacterState(CHARACTER_STATE.Move);
    }

    protected override void Update()
    {
        base.Update();
        if (!abnormalStateController.UpdateState())
        {
            playerInput?.UpdateCharacterInput();
            state?.TrySwitchCharacterState(NextCharacterState());
        }
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

    #region Animation Event Function
    private void OnSetWeapon(PLAYER_ATTACK_TYPE playerAttackType)
    {
        weapon.OnSetWeapon(playerAttackType);
    }
    private void OnReleaseWeapon()
    {
        weapon.OnReleaseWeapon();
    }
    #endregion

    public BerserkerWeapon Weapon { get { return weapon; } }
}
