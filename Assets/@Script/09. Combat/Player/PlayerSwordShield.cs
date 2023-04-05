using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordShield : PlayerWeapon
{
    [SerializeField] private PlayerCombatController swordController;
    [SerializeField] private PlayerCombatController shieldController;

    public override void InitializeWeapon(PlayerCharacter character)
    {
        if (character != null)
        {
            this.character = character;
            weaponType = WEAPON_TYPE.SWORD_SHIELD;
            swordController = Functions.FindChild<PlayerCombatController>(gameObject, Constants.WEAPON_CONTROLLER_NAME_SWORD, true);
            shieldController = Functions.FindChild<PlayerCombatController>(gameObject, Constants.WEAPON_CONTROLLER_NAME_SHIELD, true);
            swordController.Initialize(character);
            shieldController.Initialize(character);

            AddStateToCharacter(character.State);
            AddCombatInformation();
            AddCommonStateInforamtion();
        }
    }

    public override void AddCombatInformation()
    {
        combatInformationDictionary = new Dictionary<COMBAT_ACTION_TYPE, CombatActionInfomation>()
        {
            // Light Attack
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_01, new CombatActionInfomation(COMBAT_TYPE.ATTACK_LIGHT, 1f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_02, new CombatActionInfomation(COMBAT_TYPE.ATTACK_LIGHT, 1.03f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_03, new CombatActionInfomation(COMBAT_TYPE.ATTACK_LIGHT, 1.06f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_04, new CombatActionInfomation(COMBAT_TYPE.ATTACK_LIGHT, 1.09f) },

            // Heavy Attack
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_01, new CombatActionInfomation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_02, new CombatActionInfomation(COMBAT_TYPE.ATTACK_HEAVY, 1.8f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_03, new CombatActionInfomation(COMBAT_TYPE.ATTACK_HEAVY, 2.16f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_04_1, new CombatActionInfomation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_04_2, new CombatActionInfomation(COMBAT_TYPE.ATTACK_HEAVY, 2.6f) },

            {COMBAT_ACTION_TYPE.SWORD_SHIELD_PARRYING_ATTACK, new CombatActionInfomation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f) },

            {COMBAT_ACTION_TYPE.SWORD_SHIELD_GUARD_IN, new CombatActionInfomation(COMBAT_TYPE.PARRYABLE, 0f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_GUARD_LOOP, new CombatActionInfomation(COMBAT_TYPE.GUARDABLE, 0f) },
        };
    }

    public override void AddStateToCharacter(StateController state)
    {
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_EQUIP, new SwordShieldEquip(character));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_IDLE, new SwordShieldIdle(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_WALK, new SwordShieldWalk(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_RUN, new SwordShieldRun(character));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_01, new SwordShieldLightAttack01(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_02, new SwordShieldLightAttack02(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_03, new SwordShieldLightAttack03(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_04, new SwordShieldLightAttack04(character));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_HEAVY_01, new SwordShieldHeavyAttack01(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_HEAVY_02, new SwordShieldHeavyAttack02(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_HEAVY_03, new SwordShieldHeavyAttack03(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_HEAVY_04, new SwordShieldHeavyAttack04(character));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_IN, new SwordShieldGuardIn(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_LOOP, new SwordShieldGuardLoop(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_OUT, new SwordShieldGuardOut(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_BREAK, new SwordShieldGuardBreak(character));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_PARRYING, new SwordShieldParrying(character));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_PARRYING_ATTACK, new SwordShieldParryingAttack(character));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_COMPETE, new SwordShieldCompete(character));
    }

    public override void AddCommonStateInforamtion()
    {
        basicStateInformation = new BasicStateInformation();
        basicStateInformation.idleState = ACTION_STATE.PLAYER_SWORD_SHIELD_IDLE;
        basicStateInformation.walkState = ACTION_STATE.PLAYER_SWORD_SHIELD_WALK;
        basicStateInformation.runState = ACTION_STATE.PLAYER_SWORD_SHIELD_RUN;
    }

    public override void EquipWeapon()
    {
        swordController.gameObject.SetActive(true);
        shieldController.gameObject.SetActive(true);
    }
    public override void UnequipWeapon()
    {
        swordController.gameObject.SetActive(false);
        shieldController.gameObject.SetActive(false);
    }

    #region Called by Animation Event
    public void SetAndEnableSword(COMBAT_ACTION_TYPE combatActionType)
    {
        swordController.SetCombatInformation(combatInformationDictionary[combatActionType]);
        swordController.CombatCollider.enabled = true;

        GameObject effectObject = null;
        if (effectObject != null)
        {
            effectObject.transform.SetPositionAndRotation(character.transform.position + combatInformationDictionary[combatActionType].effectLocation.position,
                Quaternion.Euler(character.transform.rotation.eulerAngles + combatInformationDictionary[combatActionType].effectLocation.rotation));
        }
    }
    public void DisableSword()
    {
        swordController.CombatCollider.enabled = false;
        swordController.HitDictionary.Clear();
    }
    public void SetAndEnableShield(COMBAT_ACTION_TYPE combatActionType)
    {
        shieldController.SetCombatInformation(combatInformationDictionary[combatActionType]);
        shieldController.CombatCollider.enabled = true;

        GameObject effectObject = null;
        if (effectObject != null)
        {
            effectObject.transform.SetPositionAndRotation(character.transform.position + combatInformationDictionary[combatActionType].effectLocation.position,
                Quaternion.Euler(character.transform.rotation.eulerAngles + combatInformationDictionary[combatActionType].effectLocation.rotation));
        }
    }
    public void DisableShield()
    {
        shieldController.CombatCollider.enabled = false;
        shieldController.HitDictionary.Clear();
    }
    #endregion
}
