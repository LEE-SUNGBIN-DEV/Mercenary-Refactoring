using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DebuffData
{
    public DEBUFF_TYPE debuff;
    public string name;
    public string tooltip;
    public float lifetime;
    public bool isRemovable;
}