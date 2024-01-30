using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public class PlayerHalberd : PlayerWeapon
{
    private void OnDestroy()
    {
        attackController.OnHitting -= PlayWeaponHittingSFX;
    }

    public override void Initialize(PlayerCharacter character)
    {
        weaponType = WEAPON_TYPE.HALBERD;

        attackController = Functions.FindChild<PlayerCombatController>(character.gameObject, Constants.UNIQUE_EQUIPMENT_HALBERD_WHITE_NIGHT, true);
        attackController.Initialize(character);

        guardController = attackController;

        attackController.OnHitting += PlayWeaponHittingSFX;

        weaponRenderers = new MeshRenderer[] { attackController.GetComponentInChildren<MeshRenderer>(true) };
        materialController = new MaterialController();
        materialController.Initialize(weaponRenderers);
        weaponSFXPlayer = character.SFXPlayer;

        AddWeaponState(character);
        AddCombatTable();
        SetDefaultWeaponState();
    }

    public override void AddWeaponState(PlayerCharacter character)
    {
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_EQUIP, new HalberdEquip(character));

        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_IDLE, new HalberdIdle(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_WALK, new HalberdWalk(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_RUN, new HalberdRun(character));

        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_01, new HalberdLightAttack01(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_02, new HalberdLightAttack02(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_03, new HalberdLightAttack03(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_04, new HalberdLightAttack04(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_HEAVY_01, new HalberdHeavyAttack01(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_HEAVY_02, new HalberdHeavyAttack02(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_HEAVY_03, new HalberdHeavyAttack03(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_ATTACK_HEAVY_04, new HalberdHeavyAttack04(character));

        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_GUARD_IN, new HalberdGuardIn(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_GUARD_LOOP, new HalberdGuardLoop(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_GUARD_OUT, new HalberdGuardOut(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_GUARD_BREAK, new HalberdGuardBreak(character));

        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_PARRYING, new HalberdParrying(character));
        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_PARRYING_ATTACK, new HalberdParryingAttack(character));

        character.State.StateDictionary.Add(ACTION_STATE.PLAYER_HALBERD_SKILL_COUNTER, new HalberdCounter(character));
    }

    public override void AddCombatTable()
    {
        weaponCombatTable = new Dictionary<COMBAT_ACTION_TYPE, CombatControllerInfo>()
        {
            // Light Attack
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_01, new CombatControllerInfo(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.1f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_02, new CombatControllerInfo(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.13f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_03, new CombatControllerInfo(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.16f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_04, new CombatControllerInfo(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.19f) },
            
            // Heavy Attack
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_01, new CombatControllerInfo(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.6f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_02, new CombatControllerInfo(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.9f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_03, new CombatControllerInfo(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2.3f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_04, new CombatControllerInfo(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2.8f) },

            {COMBAT_ACTION_TYPE.HALBERD_PARRYING_ATTACK, new CombatControllerInfo(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2f) },
            {COMBAT_ACTION_TYPE.HALBERD_SKILL_COUNTER, new CombatControllerInfo(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.5f) },

            {COMBAT_ACTION_TYPE.HALBERD_GUARD_IN, new CombatControllerInfo(HIT_TYPE.NONE, GUARD_TYPE.PARRYABLE, 0f) },
            {COMBAT_ACTION_TYPE.HALBERD_GUARD_LOOP, new CombatControllerInfo(HIT_TYPE.NONE, GUARD_TYPE.GUARDABLE, 0f) },
        };
    }

    public override void SetDefaultWeaponState()
    {
        idleState = ACTION_STATE.PLAYER_HALBERD_IDLE;
        walkState = ACTION_STATE.PLAYER_HALBERD_WALK;
        runState = ACTION_STATE.PLAYER_HALBERD_RUN;
        guardBreakState = ACTION_STATE.PLAYER_HALBERD_GUARD_BREAK;
        parryingState = ACTION_STATE.PLAYER_HALBERD_PARRYING;
    }

    public override void EquipWeapon()
    {
        attackController.gameObject.SetActive(true);
        ShowWeapon();
    }

    public override void UnequipWeapon()
    {
        attackController.gameObject.SetActive(false);
    }

    public void EnableHalberd(COMBAT_ACTION_TYPE combatActionType)
    {
        attackController.SetCombatInformation(weaponCombatTable[combatActionType]);
        attackController.CombatCollider.enabled = true;
    }

    public void DisableHalberd()
    {
        attackController.CombatCollider.enabled = false;
        attackController.HitDictionary.Clear();
    }

    public override void PlayWeaponHittingSFX()
    {
        weaponSFXPlayer.PlaySFX("Audio_Player_Hitting");
    }
}
