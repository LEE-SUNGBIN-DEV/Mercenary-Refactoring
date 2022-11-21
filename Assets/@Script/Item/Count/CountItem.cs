using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class CountItem : BaseItem
{
    protected int itemCount;

    public override void Initialize<T>(T item)
    {
        base.Initialize(item);
        if (item is CountItem targetItem)
        {
            itemCount = targetItem.itemCount;
        }
    }

    public int ItemCount { get { return itemCount; } set { itemCount = value; } }
}
