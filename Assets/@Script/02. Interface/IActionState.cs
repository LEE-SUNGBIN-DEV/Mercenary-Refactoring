using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionState<T> where T : BaseActor
{
    public int StateWeight { get; }

    public void Enter(T actor);
    public void Update(T actor);
    public void Exit(T actor);
}