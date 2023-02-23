using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerWeapon : PlayerAttackController
{
    private Dictionary<LANCER_ATTACK_TYPE, CombatInformation> combatDictionary;

    public override void SetWeapon(BaseCharacter character)
    {
        base.SetWeapon(character);
        combatDictionary = new Dictionary<LANCER_ATTACK_TYPE, CombatInformation>()
        {
            // Combo Attack
            {LANCER_ATTACK_TYPE.Light_Attack_01, new CombatInformation(COMBAT_TYPE.Light_Attack, 1f, BUFF.None, 0f, new Vector3(0,0,0), new Vector3(-185.053f, 346.911f, 42.899f)) },
            {LANCER_ATTACK_TYPE.Light_Attack_02, new CombatInformation(COMBAT_TYPE.Light_Attack, 1.03f, BUFF.None, 0f, new Vector3(0,0,0), new Vector3(168.597f, 359.226f, 118.723f)) },
            {LANCER_ATTACK_TYPE.Light_Attack_03, new CombatInformation(COMBAT_TYPE.Light_Attack, 1.06f, BUFF.None, 0f, new Vector3(0,0,0), new Vector3(55.857f, 210.166f, 147.803f)) },
            {LANCER_ATTACK_TYPE.Light_Attack_04, new CombatInformation(COMBAT_TYPE.Light_Attack, 1.09f, BUFF.None, 0f, new Vector3(0,0,0), new Vector3(194.172f, -19.111f, 108.139f)) },

            // Smash Attack
            {LANCER_ATTACK_TYPE.Heavy_Attack_01, new CombatInformation(COMBAT_TYPE.Heavy_Attack, 1.5f, BUFF.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCER_ATTACK_TYPE.Heavy_Attack_02, new CombatInformation(COMBAT_TYPE.Heavy_Attack, 1.8f, BUFF.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCER_ATTACK_TYPE.Heavy_Attack_03, new CombatInformation(COMBAT_TYPE.Heavy_Attack, 2.16f, BUFF.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCER_ATTACK_TYPE.Heavy_Attack_04_1, new CombatInformation(COMBAT_TYPE.Heavy_Attack, 1.5f, BUFF.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCER_ATTACK_TYPE.Heavy_Attack_04_2, new CombatInformation(COMBAT_TYPE.Heavy_Attack, 2.6f, BUFF.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },

            {LANCER_ATTACK_TYPE.Parrying_Attack, new CombatInformation(COMBAT_TYPE.Heavy_Attack, 1.5f, BUFF.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCER_ATTACK_TYPE.Skill_Counter, new CombatInformation(COMBAT_TYPE.Light_Attack, 1.5f, BUFF.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
        };
    }

    #region Called by Owner's Animation Event
    public void OnEnableAttack(LANCER_ATTACK_TYPE attackType)
    {
        SetCombatInformation(combatDictionary[attackType]);
        combatCollider.enabled = true;

        GameObject effectObject = null;
        switch (attackType)
        {
            case LANCER_ATTACK_TYPE.Light_Attack_01:
            case LANCER_ATTACK_TYPE.Light_Attack_02:
            case LANCER_ATTACK_TYPE.Light_Attack_03:
            case LANCER_ATTACK_TYPE.Light_Attack_04:
            case LANCER_ATTACK_TYPE.Heavy_Attack_01:
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
