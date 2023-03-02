using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSword : PlayerAttackController
{
    private Dictionary<GREAT_SWORD_ATTACK_TYPE, CombatInformation> attackDictionary;

    public override void SetWeapon(BaseCharacter character)
    {
        base.SetWeapon(character);
        attackDictionary = new Dictionary<GREAT_SWORD_ATTACK_TYPE, CombatInformation>()
        {
            // Combo Attack
            {GREAT_SWORD_ATTACK_TYPE.ATTACK_LIGHT_01,
                new CombatInformation(COMBAT_TYPE.ATTACK_LIGHT, 1f) },
            {GREAT_SWORD_ATTACK_TYPE.ATTACK_LIGHT_02,
                new CombatInformation(COMBAT_TYPE.ATTACK_LIGHT, 1.03f) },
            {GREAT_SWORD_ATTACK_TYPE.ATTACK_LIGHT_03,
                new CombatInformation(COMBAT_TYPE.ATTACK_LIGHT, 1.06f) },
            {GREAT_SWORD_ATTACK_TYPE.ATTACK_LIGHT_04,
                new CombatInformation(COMBAT_TYPE.ATTACK_LIGHT, 1.09f) },

            // Smash Attack
            {GREAT_SWORD_ATTACK_TYPE.ATTACK_HEAVY_01,
                new CombatInformation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f, -194.508f)) },
            {GREAT_SWORD_ATTACK_TYPE.ATTACK_HEAVY_02,
                new CombatInformation(COMBAT_TYPE.ATTACK_HEAVY, 1.8f, new Vector3(-0.147f, 1.110f, 1.349f), new Vector3(19.841f, 9.495f, -53.357f)) },
            {GREAT_SWORD_ATTACK_TYPE.ATTACK_HEAVY_03_1,
                new CombatInformation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f, new Vector3(0.312f, 1.192f, 0.016f), new Vector3(46.793f, -180f, 49.641f)) },
            {GREAT_SWORD_ATTACK_TYPE.ATTACK_HEAVY_03_2,
                new CombatInformation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f, new Vector3(0.3f, 1.504f, 0.903f), new Vector3(43.677f, 218.04f, 139.807f)) },
            {GREAT_SWORD_ATTACK_TYPE.ATTACK_HEAVY_04_1,
                new CombatInformation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f, new Vector3(-0.169f, 2.175f, 1.869f), new Vector3(107.975f, -111.439f, -215.095f)) },
            {GREAT_SWORD_ATTACK_TYPE.ATTACK_HEAVY_04_2,
                new CombatInformation(COMBAT_TYPE.ATTACK_HEAVY, 2.6f, new Vector3(0.021f, 1.940f, 1.4f), new Vector3(128.932f, -3.225f, -281.598f)) },

            {GREAT_SWORD_ATTACK_TYPE.PARRYING_ATTACK,
                new CombatInformation(COMBAT_TYPE.ATTACK_HEAVY, 1.5f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {GREAT_SWORD_ATTACK_TYPE.SKILL_COUNTER,
                new CombatInformation(COMBAT_TYPE.ATTACK_LIGHT, 1.5f, new Vector3(-0.433f, 1.757f, 3.043f), new Vector3(29.494f, -176.467f, -82.104f)) },
        };

        owner.ObjectPooler.RegisterObject(Constants.VFX_Berserker_Smash_Attack, 6);
        owner.ObjectPooler.RegisterObject(Constants.VFX_Berserker_Stinger_Attack, 2);
    }

    #region Called by Owner's Animation Event
    public void OnEnableAttack(GREAT_SWORD_ATTACK_TYPE attackType)
    {
        SetCombatInformation(attackDictionary[attackType]);
        combatCollider.enabled = true;

        GameObject effectObject = null;
        switch (attackType)
        {
            case GREAT_SWORD_ATTACK_TYPE.ATTACK_HEAVY_01:
            case GREAT_SWORD_ATTACK_TYPE.ATTACK_HEAVY_03_1:
            case GREAT_SWORD_ATTACK_TYPE.ATTACK_HEAVY_03_2:
            case GREAT_SWORD_ATTACK_TYPE.ATTACK_HEAVY_04_1:
            case GREAT_SWORD_ATTACK_TYPE.ATTACK_HEAVY_04_2:
                effectObject = owner.ObjectPooler.RequestObject(Constants.VFX_Berserker_Smash_Attack);
                break;

            case GREAT_SWORD_ATTACK_TYPE.ATTACK_HEAVY_02:
                effectObject = owner.ObjectPooler.RequestObject(Constants.VFX_Berserker_Stinger_Attack);
                break;

            default:
                break;
        }
        
        if (effectObject != null)
        {
            effectObject.transform.position = owner.transform.TransformPoint(attackDictionary[attackType].effectLocation.position);
            effectObject.transform.rotation = Quaternion.Euler(owner.transform.rotation.eulerAngles + attackDictionary[attackType].effectLocation.rotation);
        }
    }
    public virtual void OnDisableAttack()
    {
        combatCollider.enabled = false;
        hitDictionary.Clear();
    }
    #endregion
}
