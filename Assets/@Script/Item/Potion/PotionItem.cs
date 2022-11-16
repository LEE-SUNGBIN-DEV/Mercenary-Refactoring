using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class PotionItem : BaseItem
{
    public abstract void Consume(Character character);
}
