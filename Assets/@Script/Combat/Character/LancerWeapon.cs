using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerWeapon : PlayerAttackController
{
    private Dictionary<LANCE_ATTACK_TYPE, CombatInformation> combatDictionary;

    public override void SetWeapon(BaseCharacter character)
    {
        base.SetWeapon(character);
        combatDictionary = new Dictionary<LANCE_ATTACK_TYPE, CombatInformation>()
        {
            // Combo Attack
            {LANCE_ATTACK_TYPE.ATTACK_LIGHT_01, new CombatInformation(COMBAT_TYPE.ATTACK_LIGHT, 1f, new Vector3(0,0,0), new Vector3(-185.053f, 346.911f, 42.899f)) },
            {LANCE_ATTACK_TYPE.ATTACK_LIGHT_02, new CombatInformation(COMBAT_TYPE.ATTACK_LIGHT, 1.03f, new Vector3(0,0,0), new Vector3(168.597f, 359.226f, 118.723f)) },
            {LANCE_ATTACK_TYPE.ATTACK_LIGHT_03, new CombatInformation(COMBAT_TYPE.ATTACK_LIGHT, 1.06f, new Vector3(0,0,0), new Vector3(55.857f, 210.166f, 147.803f)) },
            {LANCE_ATTACK_TYPE.ATTACK_LIGHT_04, new CombatInformation(COMBAT_TYPE.ATTACK_LIGHT, 1.09f, new Vector3(0,0,0), new Vector3(194.172f, -19.111f, 108.139f)) },

            // Smash Attack
            {LANCE_ATTACK_TYPE.ATTACK_HEAVY_01, new CombatInformation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCE_ATTACK_TYPE.ATTACK_HEAVY_02, new CombatInformation(COMBAT_TYPE.ATTACK_HEAVY, 1.8f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCE_ATTACK_TYPE.ATTACK_HEAVY_03, new CombatInformation(COMBAT_TYPE.ATTACK_HEAVY, 2.16f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCE_ATTACK_TYPE.ATTACK_HEAVY_04_1, new CombatInformation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCE_ATTACK_TYPE.ATTACK_HEAVY_04_2, new CombatInformation(COMBAT_TYPE.ATTACK_HEAVY, 2.6f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },

            {LANCE_ATTACK_TYPE.PARRYING_ATTACK, new CombatInformation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCE_ATTACK_TYPE.SKILL_COUNTER, new CombatInformation(COMBAT_TYPE.ATTACK_LIGHT, 1.5f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
        };
    }

    #region Called by Owner's Animation Event
    public void OnEnableAttack(LANCE_ATTACK_TYPE attackType)
    {
        SetCombatInformation(combatDictionary[attackType]);
        combatCollider.enabled = true;

        GameObject effectObject = null;
        switch (attackType)
        {
            case LANCE_ATTACK_TYPE.ATTACK_LIGHT_01:
            case LANCE_ATTACK_TYPE.ATTACK_LIGHT_02:
            case LANCE_ATTACK_TYPE.ATTACK_LIGHT_03:
            case LANCE_ATTACK_TYPE.ATTACK_LIGHT_04:
            case LANCE_ATTACK_TYPE.ATTACK_HEAVY_01:
                break;

            default:
                break;
        }

        if (effectObject != null)
        {
            effectObject.transform.SetPositionAndRotation(owner.transform.position + combatDictionary[attackType].effectLocation.position,
                Quaternion.Euler(owner.transform.rotation.eulerAngles + combatDictionary[attackType].effectLocation.rotation));
        }
    }
    public virtual void OnDisableAttack()
    {
        combatCollider.enabled = false;
        hitDictionary.Clear();
    }
    #endregion
}
