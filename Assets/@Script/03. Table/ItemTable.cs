using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTable<T> where T : BaseItem
{
    public T[] items;
}
