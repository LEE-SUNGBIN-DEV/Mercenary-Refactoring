using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHeaderTooltipModule : UIBase, IItemTooltipModule
{
    public enum TEXT
    {
        Item_Name_Text,
        Item_Type_Text,
        Item_Rank_Text
    }

    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemRankText;

    public void Initialize()
    {
        BindText(typeof(TEXT));

        itemNameText = GetText((int)TEXT.Item_Name_Text);
        itemTypeText = GetText((int)TEXT.Item_Type_Text);
        itemRankText = GetText((int)TEXT.Item_Rank_Text);
    }

    public void UpdateModule<T>(T item, CharacterInventoryData inventoryData) where T : BaseItem
    {
        if (item != null)
        {
            itemNameText.color = item.GetItemRankColor();
            itemTypeText.color = item.GetItemRankColor();
            itemRankText.color = item.GetItemRankColor();

            itemNameText.text = item.GetItemName();
            itemTypeText.text = item.GetItemTypeText();
            itemRankText.text = item.GetItemRankText();
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
