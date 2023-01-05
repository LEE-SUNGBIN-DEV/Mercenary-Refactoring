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
            {LANCER_ATTACK_TYPE.ComboAttack1, new CombatInformation(HIT_TYPE.Light, 1f, ABNORMAL_TYPE.None, 0f, new Vector3(0,0,0), new Vector3(-185.053f, 346.911f, 42.899f)) },
            {LANCER_ATTACK_TYPE.ComboAttack2, new CombatInformation(HIT_TYPE.Light, 1.03f, ABNORMAL_TYPE.None, 0f, new Vector3(0,0,0), new Vector3(168.597f, 359.226f, 118.723f)) },
            {LANCER_ATTACK_TYPE.ComboAttack3, new CombatInformation(HIT_TYPE.Light, 1.06f, ABNORMAL_TYPE.None, 0f, new Vector3(0,0,0), new Vector3(55.857f, 210.166f, 147.803f)) },
            {LANCER_ATTACK_TYPE.ComboAttack4, new CombatInformation(HIT_TYPE.Light, 1.09f, ABNORMAL_TYPE.None, 0f, new Vector3(0,0,0), new Vector3(194.172f, -19.111f, 108.139f)) },

            // Smash Attack
            {LANCER_ATTACK_TYPE.SmashAttack1, new CombatInformation(HIT_TYPE.Heavy, 1.5f, ABNORMAL_TYPE.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCER_ATTACK_TYPE.SmashAttack2, new CombatInformation(HIT_TYPE.Heavy, 1.8f, ABNORMAL_TYPE.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCER_ATTACK_TYPE.SmashAttack3, new CombatInformation(HIT_TYPE.Heavy, 2.16f, ABNORMAL_TYPE.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCER_ATTACK_TYPE.SmashAttack4_1, new CombatInformation(HIT_TYPE.Heavy, 1.5f, ABNORMAL_TYPE.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCER_ATTACK_TYPE.SmashAttack4_2, new CombatInformation(HIT_TYPE.Heavy, 2.6f, ABNORMAL_TYPE.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },

            {LANCER_ATTACK_TYPE.ParryingAttack, new CombatInformation(HIT_TYPE.Heavy, 1.5f, ABNORMAL_TYPE.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {LANCER_ATTACK_TYPE.CounterAttack, new CombatInformation(HIT_TYPE.Light, 1.5f, ABNORMAL_TYPE.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
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
            case LANCER_ATTACK_TYPE.ComboAttack1:
            case LANCER_ATTACK_TYPE.ComboAttack2:
            case LANCER_ATTACK_TYPE.ComboAttack3:
            case LANCER_ATTACK_TYPE.ComboAttack4:
            case LANCER_ATTACK_TYPE.SmashAttack1:
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
