using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionState
{
    public int StateWeight { get; }

    public void Enter();
    public void Update();
    public void Exit();
}