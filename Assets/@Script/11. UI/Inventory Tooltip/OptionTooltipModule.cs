using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionTooltipModule : UIBase, IItemTooltipModule
{
    [SerializeField] private Transform fixedOptionTransform;
    [SerializeField] private Transform randomOptionTransform;

    [SerializeField] private StatusOptionSlot[] fixedOptionSlots;
    [SerializeField] private StatusOptionSlot[] randomOptionSlots;

    public void Initialize()
    {
        fixedOptionTransform = Functions.FindChild<Transform>(gameObject, "Fixed_Options", true);
        randomOptionTransform = Functions.FindChild<Transform>(gameObject, "Random_Options", true);

        fixedOptionSlots = new StatusOptionSlot[Constants.MAX_FIXED_OPTION_SLOT_COUNTS];
        for (int i = 0; i < fixedOptionSlots.Length; i++)
        {
            fixedOptionSlots[i] = Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_STATUS_OPTION_SLOT, fixedOptionTransform).GetComponent<StatusOptionSlot>();
            fixedOptionSlots[i].Initialize();
            fixedOptionSlots[i].HideSlot();
        }

        randomOptionSlots = new StatusOptionSlot[Constants.MAX_RANDOM_OPTION_SLOT_COUNTS];
        for (int i = 0; i < randomOptionSlots.Length; i++)
        {
            randomOptionSlots[i] = Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_STATUS_OPTION_SLOT, randomOptionTransform).GetComponent<StatusOptionSlot>();
            randomOptionSlots[i].Initialize();
            randomOptionSlots[i].HideSlot();
        }

        fixedOptionTransform.gameObject.SetActive(false);
        randomOptionTransform.gameObject.SetActive(false);
    }

    public void UpdateModule<T>(T item, CharacterInventoryData inventoryData) where T : BaseItem
    {
        fixedOptionTransform.gameObject.SetActive(false);
        randomOptionTransform.gameObject.SetActive(false);
        if (item != null)
        {
            if (item.FixedOptions.IsNullOrEmpty() && item.RandomOptions.IsNullOrEmpty())
            {
                gameObject.SetActive(false);
                return;
            }

            if (item.FixedOptions != null)
            {
                for (int i = 0; i < fixedOptionSlots.Length; ++i)
                {
                    if (item.FixedOptions.Length > i)
                        fixedOptionSlots[i].ShowSlot(item.FixedOptions[i]);
                    else
                        fixedOptionSlots[i].HideSlot();
                }
                fixedOptionTransform.gameObject.SetActive(true);
            }

            if (item.RandomOptions != null)
            {
                for (int i = 0; i < randomOptionSlots.Length; ++i)
                {
                    if (item.RandomOptions.Length > i)
                        randomOptionSlots[i].ShowSlot(item.RandomOptions[i]);
                    else
                        randomOptionSlots[i].HideSlot();
                }
                randomOptionTransform.gameObject.SetActive(true);
            }

            gameObject.SetActive(true);
        }
    }
}
