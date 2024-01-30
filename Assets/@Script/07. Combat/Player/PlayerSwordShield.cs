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

    public override void Initialize(PlayerCharacter character)
    {
        weaponType = WEAPON_TYPE.SWORD_SHIELD;

        attackController = Functions.FindChild<PlayerCombatController>(character.gameObject, Constants.UNIQUE_EQUIPMENT_SWORD_POLAR_NIGHT, true);
        attackController.Initialize(character);

        guardController = Functions.FindChild<PlayerCombatController>(character.gameObject, Constants.UNIQUE_EQUIPMENT_SHIELD_POLAR_NIGHT, true);
        guardController.Initialize(character);

        attackController.OnHitting += PlayWeaponHittingSFX;
        guardController.OnHitting += PlayWeaponHittingSFX;

        weaponRenderers = new MeshRenderer[] { attackController.GetComponentInChildren<MeshRenderer>(true), guardController.GetComponentInChildren<MeshRenderer>(true) };
        materialController = new MaterialController();
        materialController.Initialize(weaponRenderers);
        weaponSFXPlayer = character.SFXPlayer;

        AddWeaponState(character);
        AddCombatTable();
        SetDefaultWeaponState();
    }

    public override void AddCombatTable()
    {
        weaponCombatTable = new Dictionary<COMBAT_ACTION_TYPE, CombatControllerInfo>()
        {
            // Light Attack
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_01, new CombatControllerInfo(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_02, new CombatControllerInfo(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.03f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_03, new CombatControllerInfo(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.06f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_LIGHT_04, new CombatControllerInfo(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.09f) },

            // Heavy Attack
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_01, new CombatControllerInfo(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.5f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_02, new CombatControllerInfo(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.8f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_03, new CombatControllerInfo(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2.2f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_04_01, new CombatControllerInfo(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.5f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_ATTACK_HEAVY_04_02, new CombatControllerInfo(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2.2f) },

            {COMBAT_ACTION_TYPE.SWORD_SHIELD_PARRYING_ATTACK, new CombatControllerInfo(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2f) },

            {COMBAT_ACTION_TYPE.SWORD_SHIELD_GUARD_IN, new CombatControllerInfo(HIT_TYPE.NONE, GUARD_TYPE.PARRYABLE, 0f) },
            {COMBAT_ACTION_TYPE.SWORD_SHIELD_GUARD_LOOP, new CombatControllerInfo(HIT_TYPE.NONE, GUARD_TYPE.GUARDABLE, 0f) },
        };
    }

    public override void AddWeaponState(PlayerCharacter character)
    {
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_EQUIP, new SwordShieldEquip(character));

        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_IDLE, new SwordShieldIdle(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_WALK, new SwordShieldWalk(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_RUN, new SwordShieldRun(character));

        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_01, new SwordShieldLightAttack01(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_02, new SwordShieldLightAttack02(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_03, new SwordShieldLightAttack03(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_04, new SwordShieldLightAttack04(character));

        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_HEAVY_01, new SwordShieldHeavyAttack01(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_HEAVY_02, new SwordShieldHeavyAttack02(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_HEAVY_03, new SwordShieldHeavyAttack03(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_HEAVY_04, new SwordShieldHeavyAttack04(character));

        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_IN, new SwordShieldGuardIn(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_LOOP, new SwordShieldGuardLoop(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_OUT, new SwordShieldGuardOut(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_BREAK, new SwordShieldGuardBreak(character));

        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_PARRYING, new SwordShieldParrying(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_SWORD_SHIELD_PARRYING_ATTACK, new SwordShieldParryingAttack(character));

        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_COMPETE, new SwordShieldCompete(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_COMPETE_SUCCESS, new SwordShieldCompeteSuccess(character));
    }

    public override void SetDefaultWeaponState()
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
        weaponSFXPlayer.PlaySFX("Audio_Player_Hitting");
    }
}
