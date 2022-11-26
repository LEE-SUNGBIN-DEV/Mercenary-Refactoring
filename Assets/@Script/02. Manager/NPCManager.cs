using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager
{
    private Dictionary<uint, NPC> npcDictionary = new Dictionary<uint, NPC>();

    public void Initialize()
    {

    }

    #region Property
    public Dictionary<uint, NPC> NPCDictionary { get { return npcDictionary; } }
    #endregion
}
