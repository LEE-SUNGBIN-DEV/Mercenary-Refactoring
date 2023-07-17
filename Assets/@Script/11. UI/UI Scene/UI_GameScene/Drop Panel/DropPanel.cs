using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RequestDropData
{
    public float experience;
    public int resonanceStone;
    public string[] itemNames;
    public int[] itemAmounts;
}

public class DropPanel : UIPanel
{
    [SerializeField] private Queue<RequestDropData> dropQueue;
    [SerializeField] private DropExpSlot dropExpSlot;
    [SerializeField] private DropResonanceStoneSlot dropResonanceStoneSlot;

    [SerializeField] private DropItemSlot[] dropItemSlots;

    public void Initialize()
    {
        dropExpSlot = GetComponentInChildren<DropExpSlot>();
        dropExpSlot.Initialize();

        dropResonanceStoneSlot = GetComponentInChildren<DropResonanceStoneSlot>();
        dropResonanceStoneSlot.Initialize();

        dropItemSlots = GetComponentsInChildren<DropItemSlot>();
        for (int i = 0; i < dropItemSlots.Length; ++i)
        {
            dropItemSlots[i].Initialize();
        }
    }

    public void ShowDropPanel()
    {

    }
}
