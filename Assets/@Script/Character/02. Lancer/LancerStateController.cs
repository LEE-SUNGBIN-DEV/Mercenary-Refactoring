using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerStateController : CharacterStateController
{
    public LancerStateController(Lancer lancer) : base(lancer)
    {
        stateDictionary.Add(CHARACTER_STATE.LancerDefense, new LancerStateDefense(lancer));
    }
}
