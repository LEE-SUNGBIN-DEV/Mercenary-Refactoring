using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public class PlayerHalberd : PlayerWeapon
{
    [SerializeField] private PlayerCombatController halberdController;

    public override void InitializeWeapon(PlayerCharacter character)
    {
        base.InitializeWeapon(character);
        weaponType = WEAPON_TYPE.HALBERD;
        halberdController = Functions.FindChild<PlayerCombatController>(character.gameObject, Constants.WEAPON_CONTROLLER_NAME_HALBERD, true);
        halberdController.Initialize(character);

        weaponRenderers = new MeshRenderer[] { halberdController.GetComponentInChildren<MeshRenderer>(true) };
        materialController = new MaterialController();
        materialController.Initialize(weaponRenderers);

        AddWeaponState(character.State);
        AddCombatTable();
        AddCommonStateInforamtion();
    }

    public override void AddWeaponState(StateController state)
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

    public override void AddCombatTable()
    {
        weaponCombatTable = new Dictionary<COMBAT_ACTION_TYPE, CombatControllerInfomation>()
        {
            // Light Attack
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_01, new CombatControllerInfomation(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.1f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_02, new CombatControllerInfomation(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.13f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_03, new CombatControllerInfomation(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.16f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_LIGHT_04, new CombatControllerInfomation(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.19f) },
            
            // Heavy Attack
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_01, new CombatControllerInfomation(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.6f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_02, new CombatControllerInfomation(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.9f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_03, new CombatControllerInfomation(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2.3f) },
            {COMBAT_ACTION_TYPE.HALBERD_ATTACK_HEAVY_04, new CombatControllerInfomation(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2.8f) },

            {COMBAT_ACTION_TYPE.HALBERD_PARRYING_ATTACK, new CombatControllerInfomation(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2f) },
            {COMBAT_ACTION_TYPE.HALBERD_SKILL_COUNTER, new CombatControllerInfomation(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1.5f) },

            {COMBAT_ACTION_TYPE.HALBERD_GUARD_IN, new CombatControllerInfomation(HIT_TYPE.NONE, GUARD_TYPE.PARRYABLE, 0f) },
            {COMBAT_ACTION_TYPE.HALBERD_GUARD_LOOP, new CombatControllerInfomation(HIT_TYPE.NONE, GUARD_TYPE.GUARDABLE, 0f) },
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
        ShowWeapon();
    }

    public override void UnequipWeapon()
    {
        halberdController.gameObject.SetActive(false);
    }

    public void EnableHalberd(COMBAT_ACTION_TYPE combatActionType)
    {
        halberdController.SetCombatInformation(weaponCombatTable[combatActionType]);
        halberdController.CombatCollider.enabled = true;
    }

    public void DisableHalberd()
    {
        halberdController.CombatCollider.enabled = false;
        halberdController.HitDictionary.Clear();
    }
}
