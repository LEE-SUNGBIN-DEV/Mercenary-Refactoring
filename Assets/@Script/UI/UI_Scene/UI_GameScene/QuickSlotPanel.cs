using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotPanel : UIPanel
{
    [SerializeField] private QuickSlot[] quickSlots;

    public override void Initialize()
    {
        quickSlots = GetComponentsInChildren<QuickSlot>();
    }

    private void OnEnable()
    {
        ShowQuickSlot();
    }

    private void Update()
    {
        if (Managers.GameSceneManager.CurrentScene.SceneType == SCENE_TYPE.DUNGEON)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                UseQuickSlotItem(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                UseQuickSlotItem(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                UseQuickSlotItem(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                UseQuickSlotItem(3);
            }
        }
    }

    public void ShowQuickSlot()
    {
        for (int i = 0; i < quickSlots.Length; ++i)
        {

        }
        
    }

    public void UseQuickSlotItem(int slotIndex)
    {
        if (quickSlots[slotIndex].Item != null)
        {
            quickSlots[slotIndex].ConsumeItem();
        }
    }

    public QuickSlot[] QuickSlots { get { return quickSlots; } set { quickSlots = value; } }
}
