using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPC_ID
{
    LEADER_NPC = 1000,
    STORE_NPC = 2000,
    FORGE_NPC = 3000
}

public class NPC : MonoBehaviour
{
    [SerializeField] private uint npcID;
    [SerializeField] private string npcName;

    public virtual void Initialize() { }

    #region Property
    public uint NpcID { get { return npcID; } }
    public string NpcName { get { return npcName; } }
    #endregion
}
