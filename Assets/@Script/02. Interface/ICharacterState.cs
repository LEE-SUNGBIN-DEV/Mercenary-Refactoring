using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterState
{
    public int StateWeight { get; }

    public void Enter(BaseCharacter character);
    public void Update(BaseCharacter character);
    public void Exit(BaseCharacter character);
}