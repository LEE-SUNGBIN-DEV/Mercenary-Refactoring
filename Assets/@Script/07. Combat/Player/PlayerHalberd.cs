using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHalberd : PlayerWeapon
{
    [SerializeField] private PlayerCombatController halberdController;

    public override void InitializeWeapon(PlayerCharacter character)
    {
        if (character != null)
        {
            this.character = character;
            weaponType = WEAPON_TYPE.HALBERD;
            halberdController = Functions.FindChild<PlayerCombatController>(gameObject, Constants.WEAPON_CONTROLLER_NAME_HALBERD, true);
            halberdController.Initialize(character);

            AddStateToCharacter(character.State);
            AddCombatInformation();
            AddCommonStateInforamtion();
        }
    }

    public override void AddStateToCharacter(StateController state)
    {
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_EQUIP, new HalberdEquip(character));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_IDLE, new HalberdIdle(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_WALK, new HalberdWalk(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_RUN, new HalberdRun(character));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_01, new HalberdLightAttack01(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_02, new HalberdLightAttack02(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_03, new HalberdLightAttack03(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_04, new HalberdLightAttack04(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_HEAVY_01, new HalberdHeavyAttack01(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_HEAVY_02, new HalberdHeavyAttack02(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_HEAVY_03, new HalberdHeavyAttack03(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_HEAVY_04, new HalberdHeavyAttack04(character));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_GUARD_IN, new HalberdGuardIn(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_GUARD_LOOP, new HalberdGuardLoop(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_GUARD_OUT, new HalberdGuardOut(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_GUARD_BREAK, new HalberdGuardBreak(character));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_PARRYING, new HalberdParrying(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_PARRYING_ATTACK, new HalberdParryingAttack(character));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_SKILL_COUNTER, new HalberdCounter(character));
    }

    public override void AddCombatInformation()
    {
        combatInformationDictionary = new Dictionary<COMBAT_ACTION_TYPE, CombatActionInfomation>()
        {
            // Light Attack
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_01, new CombatActionInfomation(COMBAT_TYPE.ATTACK_LIGHT, 1f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_02, new CombatActionInfomation(COMBAT_TYPE.ATTACK_LIGHT, 1.03f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_03, new CombatActionInfomation(COMBAT_TYPE.ATTACK_LIGHT, 1.06f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_04, new CombatActionInfomation(COMBAT_TYPE.ATTACK_LIGHT, 1.09f) },
            
            // Heavy Attack
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_01, new CombatActionInfomation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_02, new CombatActionInfomation(COMBAT_TYPE.ATTACK_HEAVY, 1.8f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_03, new CombatActionInfomation(COMBAT_TYPE.ATTACK_HEAVY, 2.2f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_04_1, new CombatActionInfomation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_04_2, new CombatActionInfomation(COMBAT_TYPE.ATTACK_HEAVY, 2.6f) },

            {COMBAT_ACTION_TYPE.HALBERD_PARRYING_ATTACK, new CombatActionInfomation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f) },
            {COMBAT_ACTION_TYPE.HALBERD_SKILL_COUNTER, new CombatActionInfomation(COMBAT_TYPE.ATTACK_LIGHT, 1.5f) },

            {COMBAT_ACTION_TYPE.HALBERD_GUARD_IN, new CombatActionInfomation(COMBAT_TYPE.PARRYABLE, 0f) },
            {COMBAT_ACTION_TYPE.HALBERD_GUARD_LOOP, new CombatActionInfomation(COMBAT_TYPE.GUARDABLE, 0f) },
        };
    }

    public override void AddCommonStateInforamtion()
    {
        idleState = ACTION_STATE.PLAYER_HALBERD_IDLE;
        walkState = ACTION_STATE.PLAYER_HALBERD_WALK;
        runState = ACTION_STATE.PLAYER_HALBERD_RUN;
        guardBreakState = ACTION_STATE.PLAYER_HALBERD_GUARD_BREAK;
        parryingState = ACTION_STATE.PLAYER_HALBERD_PARRYING;
    }

    public override void EquipWeapon()
    {
        halberdController.gameObject.SetActive(true);
    }
    public override void UnequipWeapon()
    {
        halberdController.gameObject.SetActive(false);
    }

    #region Called by Animation Event
    public void SetAndEnableHalberd(COMBAT_ACTION_TYPE combatActionType)
    {
        halberdController.SetCombatInformation(combatInformationDictionary[combatActionType]);
        halberdController.CombatCollider.enabled = true;

        GameObject effectObject = null;

        if (effectObject != null)
        {
            effectObject.transform.position = character.transform.TransformPoint(combatInformationDictionary[combatActionType].effectLocation.position);
            effectObject.transform.rotation = Quaternion.Euler(character.transform.rotation.eulerAngles + combatInformationDictionary[combatActionType].effectLocation.rotation);
        }
    }
    public void DisableHalberd()
    {
        halberdController.CombatCollider.enabled = false;
        halberdController.HitDictionary.Clear();
    }
    #endregion
}
