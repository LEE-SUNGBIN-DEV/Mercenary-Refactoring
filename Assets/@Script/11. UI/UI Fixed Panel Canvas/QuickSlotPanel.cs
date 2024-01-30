using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotPanel : UIPanel
{
    [SerializeField] private CharacterInventoryData inventoryData;
    [SerializeField] private QuickSlot[] quickSlots;

    #region Private
    protected override void Awake()
    {
        base.Awake();
        quickSlots = GetComponentsInChildren<QuickSlot>(true);
        for (int i = 0; i < quickSlots.Length; ++i)
        {
            quickSlots[i].SlotIndex = i;
        }
    }

    private void OnEnable()
    {
        ConnectData();
    }
    private void OnDisable()
    {
        DisconnectData();
    }
    private void Update()
    {
        if (Managers.InputManager.UIQuickSlot1Button.WasPressedThisFrame())
        {
            UseQuickSlot(0);
        }
        if (Managers.InputManager.UIQuickSlot2Button.WasPressedThisFrame())
        {
            UseQuickSlot(1);
        }
        if (Managers.InputManager.UIQuickSlot3Button.WasPressedThisFrame())
        {
            UseQuickSlot(2);
        }
        if (Managers.InputManager.UIQuickSlot4Button.WasPressedThisFrame())
        {
            UseQuickSlot(3);
        }
    }
    private void ConnectData()
    {
        inventoryData = Managers.DataManager.CurrentCharacterData.InventoryData;
        if (inventoryData != null)
        {
            inventoryData.OnChangeInventoryData -= UpdatePanel;
            inventoryData.OnChangeInventoryData += UpdatePanel;
            UpdatePanel(inventoryData);
        }
    }
    private void DisconnectData()
    {
        if (inventoryData != null)
        {
            inventoryData.OnChangeInventoryData -= UpdatePanel;
            inventoryData = null;
        }
    }
    #endregion

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void UpdatePanel(CharacterInventoryData inventoryData)
    {
        for (int i = 0; i < inventoryData.QuickSlotItemIDs.Length; ++i)
        {
            quickSlots[i].ConnectData(inventoryData);
        }
    }

    public void UseQuickSlot(int slotIndex)
    {
        inventoryData.UseQuickSlot(quickSlots[slotIndex]);
    }

    public QuickSlot[] QuickSlots { get { return quickSlots; } set { quickSlots = value; } }
}
