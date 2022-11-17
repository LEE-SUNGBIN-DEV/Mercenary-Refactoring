using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class ConsumptionItem : BaseItem
{
    public abstract void Consume(Character character);
}
