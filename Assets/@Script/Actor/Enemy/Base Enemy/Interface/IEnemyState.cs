using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    public int StateWeight { get; }

    public void Enter(BaseEnemy enemy);
    public void Update(BaseEnemy enemy);
    public void Exit(BaseEnemy enemy);
}
