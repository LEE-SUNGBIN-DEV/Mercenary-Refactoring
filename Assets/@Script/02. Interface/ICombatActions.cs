using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILightHitable
{
    public void OnLightHit();
}
public interface IHeavyHitable
{
    public void OnHeavyHit();
}
public interface IStunable
{
    public void OnStun(float duration);
}
public interface IStaggerable
{
    public void OnStagger();
}
public interface ICompetable
{
    public void OnCompete();
}
public interface ICanAbnormalState
{
}