using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResonanceWater : MonoBehaviour
{
    private PlayerCharacter character;
    private CharacterInventoryData inventoryData;
    private Material liquidMaterial;
    private string fillAmount = "_FillAmount";

    private void OnDestroy()
    {
        if(inventoryData != null)
            inventoryData.OnChangeInventoryData -= UpdateResonanceWater;
    }

    public void Initialize(PlayerCharacter character)
    {
        this.character = character;
        inventoryData = character.InventoryData;

        if (TryGetComponent(out Renderer resonanceWaterRenderer))
            liquidMaterial = resonanceWaterRenderer.sharedMaterials[2];

        inventoryData.OnChangeInventoryData += UpdateResonanceWater;

        UpdateResonanceWater(inventoryData);
    }

    public void ShowResonanceWater(bool isShow)
    {
        character.WeaponController.HideWeapon(isShow);
        gameObject.SetActive(isShow);
    }

    public void UpdateResonanceWater(CharacterInventoryData inventoryData)
    {
        liquidMaterial.SetFloat(fillAmount, inventoryData.GetRemainingResonanceWaterRatio());
    }
}
