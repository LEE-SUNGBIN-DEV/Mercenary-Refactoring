using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerStateController : CharacterStateController
{
    public BerserkerStateController(Berserker berserker) : base(berserker)
    {
        stateDictionary.Add(CHARACTER_STATE.Defense, new BerserkerStateDefense(berserker));
    }
}
