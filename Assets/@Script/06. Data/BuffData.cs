using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffData
{
    public BUFF_TYPE buff;
    public string name;
    public string tooltip;
    public float lifetime;
    public bool isRemovable;
}
