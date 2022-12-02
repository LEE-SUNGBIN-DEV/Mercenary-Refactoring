using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerStateController : CharacterStateController
{
    public LancerStateController(Character character) : base(character)
    {
        stateDictionary.Add(CHARACTER_STATE.LANCER_DEFENSE, new LancerStateDefense());
    }
}
