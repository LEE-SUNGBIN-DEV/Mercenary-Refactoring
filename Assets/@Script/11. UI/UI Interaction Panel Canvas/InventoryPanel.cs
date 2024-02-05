using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InventoryPanel : UIPanel, IFocusPanel
{
    public enum TEXT
    {
        Response_Stone_Value_Text
    }

    public event UnityAction<IFocusPanel> OnOpenFocusPanel;
    public event UnityAction<IFocusPanel> OnCloseFocusPanel;

    private bool isOpen;

    [Header("Inventory Slots")]
    [SerializeField] private Transform inventorySlotRootTransform;
    [SerializeField] private InventorySlot[] inventorySlots;

    [Header("Rune Slots")]
    [SerializeField] private Transform runeSlotRootTransform;
    [SerializeField] private RuneSlot[] runeSlots;

    [Header("Unique Equipments")]
    [SerializeField] private ResponseWaterSlot responseWaterSlot;
    [SerializeField] private HalberdSlot halberdSlot;
    [SerializeField] private SwordShieldSlot swordShieldSlot;
    [SerializeField] private ArmorSlot armorSlot;

    [Header("Response Stone")]
    [SerializeField] private TextMeshProUGUI responseStoneValueText;

    [Header("Tooltip Panel")]
    [SerializeField] private ItemTooltipPanel itemTooltipPanel;

    [Header("Inventory Data")]
    private CharacterInventoryData inventoryData;

    #region Private
    private void OnEnable()
    {
        ConnectData();
    }
    private void OnDisable()
    {
        DisconnectData();
    }
    private void ConnectData()
    {
        inventoryData = Managers.DataManager.CurrentCharacterData.InventoryData;
        if(inventoryData != null)
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

    public void Initialize()
    {
        isOpen = false;
        BindText(typeof(TEXT));
        responseStoneValueText = GetText((int)TEXT.Response_Stone_Value_Text);

        inventorySlotRootTransform = Functions.FindChild<Transform>(gameObject, "Inventory_Slot_Contents", true);
        inventorySlots = new InventorySlot[Constants.MAX_INVENTORY_SLOT_COUNTS];
        for (int index = 0; index < Constants.MAX_INVENTORY_SLOT_COUNTS; ++index)
        {
            if (Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_INVENTORY_SLOT, inventorySlotRootTransform).TryGetComponent(out inventorySlots[index]))
                inventorySlots[index].Initialize(index);
        }

        runeSlots = new RuneSlot[Constants.MAX_RUNE_SLOT_COUNTS];
        runeSlotRootTransform = Functions.FindChild<Transform>(gameObject, "Rune_Slot_Contents", true);
        for (int index = 0; index < Constants.MAX_RUNE_SLOT_COUNTS; ++index)
        {
            if (Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_RUNE_SLOT, runeSlotRootTransform).TryGetComponent(out runeSlots[index]))
                runeSlots[index].Initialize(index);
        }

        responseWaterSlot = GetComponentInChildren<ResponseWaterSlot>(true);
        responseWaterSlot.Initialize();
        halberdSlot = GetComponentInChildren<HalberdSlot>(true);
        halberdSlot.Initialize();
        swordShieldSlot = GetComponentInChildren<SwordShieldSlot>(true);
        swordShieldSlot.Initialize();
        armorSlot = GetComponentInChildren<ArmorSlot>(true);
        armorSlot.Initialize();

        itemTooltipPanel = GetComponentInChildren<ItemTooltipPanel>(true);
        itemTooltipPanel.Initialize();
        ITooltipItemSlot[] tooltipableSlots = GetComponentsInChildren<ITooltipItemSlot>(true);
        for (int i = 0; i < tooltipableSlots.Length; ++i)
        {
            tooltipableSlots[i].TooltipPanel = itemTooltipPanel;
        }
    }

    public void TogglePanel()
    {
        if (isOpen)
            ClosePanel();
        else
            OpenPanel();
    }

    public void OpenPanel()
    {
        isOpen = true;
        OnOpenFocusPanel?.Invoke(this);
        Managers.InputManager.PushInputMode(CHARACTER_INPUT_MODE.UI);
        Managers.UIManager.SetCursorMode(CURSOR_MODE.VISIBLE);
        FadeInPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE);
    }
    public void ClosePanel()
    {
        isOpen = false;
        OnCloseFocusPanel?.Invoke(this);
        Managers.InputManager.PopInputMode();
        Managers.UIManager.SetCursorMode(CURSOR_MODE.INVISIBLE);
        FadeOutPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE, () => gameObject.SetActive(false));
    }

    public void UpdatePanel(CharacterInventoryData inventoryData)
    {
        responseStoneValueText.text = Functions.GetIntCommaString(inventoryData.ResponseStone);

        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            inventorySlots[i].UpdateSlot(inventoryData);
        }

        for(int i=0; i<runeSlots.Length; ++i)
        {
            runeSlots[i].UpdateSlot(inventoryData);
        }

        responseWaterSlot.UpdateSlot(inventoryData);
        halberdSlot.UpdateSlot(inventoryData);
        swordShieldSlot.UpdateSlot(inventoryData);
        armorSlot.UpdateSlot(inventoryData);
    }

    public void ClearInventory()
    {
        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            inventorySlots[i].ClearSlot();
        }
    }

    #region Property
    public InventorySlot[] InventorySlots { get { return inventorySlots; } }
    public RuneSlot[] RuneSlots { get { return runeSlots; } }
    public HalberdSlot HalberdSlot { get { return halberdSlot; } }
    public SwordShieldSlot SwordShieldSlot { get { return swordShieldSlot; } }
    public ArmorSlot ArmorSlot { get { return armorSlot; } }
    #endregion
}
