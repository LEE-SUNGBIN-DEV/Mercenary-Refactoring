using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDescriptionTooltipModule : UIBase, IItemTooltipModule
{
    public enum TEXT
    {
        Item_Description_Text
    }

    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    public void Initialize()
    {
        BindText(typeof(TEXT));
        itemDescriptionText = GetText((int)TEXT.Item_Description_Text);
    }

    public void UpdateModule<T>(T item, CharacterInventoryData inventoryData) where T : BaseItem
    {
        if (item != null)
        {
            itemDescriptionText.text = item.GetItemDescription();
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
