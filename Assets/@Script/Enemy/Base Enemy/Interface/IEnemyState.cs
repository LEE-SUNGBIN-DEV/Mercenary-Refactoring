using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    public int StateWeight { get; }

    public void Enter(Enemy enemy);
    public void Update(Enemy enemy);
    public void Exit(Enemy enemy);
}
