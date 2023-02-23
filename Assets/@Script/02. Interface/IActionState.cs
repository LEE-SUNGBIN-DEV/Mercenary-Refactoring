using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionState<T> where T : BaseActor
{
    public int StateWeight { get; }

    public void Enter(T character);
    public void Update(T character);
    public void Exit(T character);
}