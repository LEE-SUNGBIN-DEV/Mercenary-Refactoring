using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private uint npcID;
    [SerializeField] private string npcName;

    public virtual void Initialize()
    {
        if (!Managers.NPCManager.NpcDictionary.ContainsKey(NpcID))
        {
            Managers.NPCManager.NpcDictionary.Add(NpcID, this);
        }
    }

    public virtual void OnDisable()
    {
        if (Managers.NPCManager.NpcDictionary.ContainsKey(NpcID))
        {
            Managers.NPCManager.NpcDictionary.Remove(NpcID);
        }
    }

    #region Property
    public uint NpcID { get { return npcID; } }
    public string NpcName { get { return npcName; } }
    #endregion
}
