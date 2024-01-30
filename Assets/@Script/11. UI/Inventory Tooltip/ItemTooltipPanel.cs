using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltipPanel : UIBase
{
    private IItemTooltipModule[] tooltipModules;
    private LayoutGroup[] layoutGroups;

    public void Initialize()
    {
        layoutGroups = GetComponentsInChildren<LayoutGroup>(true);
        tooltipModules = GetComponentsInChildren<IItemTooltipModule>(true);
        for (int i = 0; i < tooltipModules.Length; i++)
        {
            tooltipModules[i].Initialize();
        }
        HideTooltip();
    }

    public void ShowTooltip<T>(T item, CharacterInventoryData inventoryData) where T : BaseItem
    {
        if (item == null)
        {
            HideTooltip();
            return;
        }

        for (int i = 0; i < tooltipModules.Length; i++)
        {
            tooltipModules[i].UpdateModule(item, inventoryData);
        }
        gameObject.SetActive(true);
        Functions.RebuildLayout(layoutGroups);
    }

    public void HideTooltip()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
