using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatController : BaseCombatController
{
    protected Character owner;

    public override void Initialize()
    {
        base.Initialize();
        owner = GetComponentInParent<Character>(true);
    }
    public virtual void Initialize(Character character)
    {
        base.Initialize();
        owner = character;
    }

    public void StartSkill()
    {
        combatType = COMBAT_TYPE.Counter;
        damageRatio = 2f;
        attackCollider.enabled = true;
    }
    public void EndSkill()
    {
        attackCollider.enabled = false;
    }

    public Character Owner { get { return owner; } }
}
