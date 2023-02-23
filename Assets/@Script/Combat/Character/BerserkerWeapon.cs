using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerWeapon : PlayerAttackController
{
    private Dictionary<BERSERKER_ATTACK_TYPE, CombatInformation> attackDictionary;

    public override void SetWeapon(BaseCharacter character)
    {
        base.SetWeapon(character);
        attackDictionary = new Dictionary<BERSERKER_ATTACK_TYPE, CombatInformation>()
        {
            // Combo Attack
            {BERSERKER_ATTACK_TYPE.Light_Attack_01,
                new CombatInformation(COMBAT_TYPE.Light_Attack, 1f, BUFF.None, 0f) },
            {BERSERKER_ATTACK_TYPE.Light_Attack_02,
                new CombatInformation(COMBAT_TYPE.Light_Attack, 1.03f, BUFF.None, 0f) },
            {BERSERKER_ATTACK_TYPE.Light_Attack_03,
                new CombatInformation(COMBAT_TYPE.Light_Attack, 1.06f, BUFF.None, 0f) },
            {BERSERKER_ATTACK_TYPE.Light_Attack_04,
                new CombatInformation(COMBAT_TYPE.Light_Attack, 1.09f, BUFF.None, 0f) },

            // Smash Attack
            {BERSERKER_ATTACK_TYPE.Heavy_Attack_01,
                new CombatInformation(COMBAT_TYPE.Heavy_Attack, 1.5f, BUFF.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f, -194.508f)) },
            {BERSERKER_ATTACK_TYPE.Heavy_Attack_02,
                new CombatInformation(COMBAT_TYPE.Heavy_Attack, 1.8f, BUFF.None, 0f, new Vector3(-0.147f, 1.110f, 1.349f), new Vector3(19.841f, 9.495f, -53.357f)) },
            {BERSERKER_ATTACK_TYPE.Heavy_Attack_03_1,
                new CombatInformation(COMBAT_TYPE.Heavy_Attack, 1.5f, BUFF.None, 0f, new Vector3(0.312f, 1.192f, 0.016f), new Vector3(46.793f, -180f, 49.641f)) },
            {BERSERKER_ATTACK_TYPE.Heavy_Attack_03_2,
                new CombatInformation(COMBAT_TYPE.Heavy_Attack, 1.5f, BUFF.None, 0f, new Vector3(0.3f, 1.504f, 0.903f), new Vector3(43.677f, 218.04f, 139.807f)) },
            {BERSERKER_ATTACK_TYPE.Heavy_Attack_04_1,
                new CombatInformation(COMBAT_TYPE.Heavy_Attack, 1.5f, BUFF.None, 0f, new Vector3(-0.169f, 2.175f, 1.869f), new Vector3(107.975f, -111.439f, -215.095f)) },
            {BERSERKER_ATTACK_TYPE.Heavy_Attack_04_2,
                new CombatInformation(COMBAT_TYPE.Heavy_Attack, 2.6f, BUFF.None, 0f, new Vector3(0.021f, 1.940f, 1.4f), new Vector3(128.932f, -3.225f, -281.598f)) },

            {BERSERKER_ATTACK_TYPE.Parrying_Attack,
                new CombatInformation(COMBAT_TYPE.Heavy_Attack, 1.5f, BUFF.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {BERSERKER_ATTACK_TYPE.Skill_Counter,
                new CombatInformation(COMBAT_TYPE.Light_Attack, 1.5f, BUFF.None, 0f, new Vector3(-0.433f, 1.757f, 3.043f), new Vector3(29.494f, -176.467f, -82.104f)) },
        };

        owner.ObjectPooler.RegisterObject(Constants.VFX_Berserker_Combo_Attack, 3);
        owner.ObjectPooler.RegisterObject(Constants.VFX_Berserker_Smash_Attack, 6);
        owner.ObjectPooler.RegisterObject(Constants.VFX_Berserker_Stinger_Attack, 2);
    }

    #region Called by Owner's Animation Event
    public void OnEnableAttack(BERSERKER_ATTACK_TYPE attackType)
    {
        SetCombatInformation(attackDictionary[attackType]);
        combatCollider.enabled = true;

        GameObject effectObject = null;
        switch (attackType)
        {
            case BERSERKER_ATTACK_TYPE.Skill_Counter:
                effectObject = owner.ObjectPooler.RequestObject(Constants.VFX_Berserker_Combo_Attack);
                break;
            case BERSERKER_ATTACK_TYPE.Heavy_Attack_01:
            case BERSERKER_ATTACK_TYPE.Heavy_Attack_03_1:
            case BERSERKER_ATTACK_TYPE.Heavy_Attack_03_2:
            case BERSERKER_ATTACK_TYPE.Heavy_Attack_04_1:
            case BERSERKER_ATTACK_TYPE.Heavy_Attack_04_2:
                effectObject = owner.ObjectPooler.RequestObject(Constants.VFX_Berserker_Smash_Attack);
                break;

            case BERSERKER_ATTACK_TYPE.Heavy_Attack_02:
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
