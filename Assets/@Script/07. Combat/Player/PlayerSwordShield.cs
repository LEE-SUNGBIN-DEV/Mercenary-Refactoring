using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public class PlayerSwordShield : PlayerWeapon
{
    private void OnDestroy()
    {
        attackController.OnHitting -= PlayWeaponHittingSFX;
        guardController.OnHitting -= PlayWeaponHittingSFX;
    }

    public override void InitializeWeapon(PlayerCharacter character)
    {
        base.InitializeWeapon(character);
        weaponType = WEAPON_TYPE.SWORD_SHIELD;

        attackController = Functions.FindChild<PlayerCombatController>(character.gameObject, Constants.WEAPON_CONTROLLER_NAME_SWORD, true);
        attackController.Initialize(character);

        guardController = Functions.FindChild<PlayerCombatController>(character.gameObject, Constants.WEAPON_CONTROLLER_NAME_SHIELD, true);
        guardController.Initialize(character);

        attackController.OnHitting += PlayWeaponHittingSFX;
        guardController.OnHitting += PlayWeaponHittingSFX;

        weaponRenderers = new MeshRenderer[] { attackController.GetComponentInChildren<MeshRenderer>(true), guardController.GetComponentInChildren<MeshRenderer>(true) };
        materialController = new MaterialController();
        materialController.Initialize(weaponRenderers);

        AddWeaponState(character.State);
        AddCombatTable();
        AddCommonStateInforamtion();
    }

    public override void AddCombatTable()
    {
        weaponCombatTable = new Dictionary<COMBAT_ACTION_TYPE, CombatControllerInfomation>()
        {
            // Light Attack
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_01, new CombatControllerInfomation(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_02, new CombatControllerInfomation(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.03f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_03, new CombatControllerInfomation(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.06f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_04, new CombatControllerInfomation(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.09f) },

            // Heavy Attack
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_01, new CombatControllerInfomation(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.5f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_02, new CombatControllerInfomation(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.8f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_03, new CombatControllerInfomation(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2.2f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_04_01, new CombatControllerInfomation(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.5f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_04_02, new CombatControllerInfomation(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2.2f) },

            {COMBAT_ACTION_TYPE.SWORD_SHIELD_PARRYING_ATTACK, new CombatControllerInfomation(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2f) },

            {COMBAT_ACTION_TYPE.SWORD_SHIELD_GUARD_IN, new CombatControllerInfomation(HIT_TYPE.NONE, GUARD_TYPE.PARRYABLE, 0f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_GUARD_LOOP, new CombatControllerInfomation(HIT_TYPE.NONE, GUARD_TYPE.GUARDABLE, 0f) },
        };
    }

    public override void AddWeaponState(StateController state)
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
        state.StateDictionary.Add(ACTION_STATE.PLAYER_COMPETE_SUCCESS, new SwordShieldCompeteSuccess(character));
    }

    public override void AddCommonStateInforamtion()
    {
        idleState = ACTION_STATE.PLAYER_SWORD_SHIELD_IDLE;
        walkState = ACTION_STATE.PLAYER_SWORD_SHIELD_WALK;
        runState = ACTION_STATE.PLAYER_SWORD_SHIELD_RUN;
        guardBreakState = ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_BREAK;
        parryingState = ACTION_STATE.PLAYER_SWORD_SHIELD_PARRYING;
    }

    public override void EquipWeapon()
    {
        attackController.gameObject.SetActive(true);
        guardController.gameObject.SetActive(true);
        ShowWeapon();
    }

    public override void UnequipWeapon()
    {
        attackController.gameObject.SetActive(false);
        guardController.gameObject.SetActive(false);
    }

    public void EnableSword(COMBAT_ACTION_TYPE combatActionType)
    {
        attackController.SetCombatInformation(weaponCombatTable[combatActionType]);
        attackController.CombatCollider.enabled = true;
    }

    public void DisableSword()
    {
        attackController.CombatCollider.enabled = false;
        attackController.HitDictionary.Clear();
    }

    public void EnableShield(COMBAT_ACTION_TYPE combatActionType)
    {
        guardController.SetCombatInformation(weaponCombatTable[combatActionType]);
        guardController.CombatCollider.enabled = true;
    }

    public void DisableShield()
    {
        guardController.CombatCollider.enabled = false;
        guardController.HitDictionary.Clear();
    }

    public override void PlayWeaponHittingSFX()
    {
        character.SFXPlayer.PlaySFX("Audio_Player_Hitting");
    }
}
