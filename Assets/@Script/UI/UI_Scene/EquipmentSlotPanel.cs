using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotPanel : UIPanel
{
    [SerializeField] private WeaponSlot weaponSlot;
    [SerializeField] private HelmetSlot helmetSlot;
    [SerializeField] private ArmorSlot armorSlot;
    [SerializeField] private BootsSlot bootsSlot;

    public override void Initialize()
    {
        weaponSlot = GetComponentInChildren<WeaponSlot>();
        helmetSlot = GetComponentInChildren<HelmetSlot>();
        armorSlot = GetComponentInChildren<ArmorSlot>();
        bootsSlot = GetComponentInChildren<BootsSlot>();

        Managers.DataManager.CurrentCharacter.CharacterData.OnLoadCharacterData -= LoadPlayerEquipmentSlots;
        Managers.DataManager.CurrentCharacter.CharacterData.OnLoadCharacterData += LoadPlayerEquipmentSlots;
        Managers.DataManager.CurrentCharacter.CharacterData.OnSaveCharacterData -= SavePlayerEquipmentSlots;
        Managers.DataManager.CurrentCharacter.CharacterData.OnSaveCharacterData += SavePlayerEquipmentSlots;
    }

    public void LoadEquipmentSlot<T>(T loadSlot, T thisSlot) where T: EquipmentSlot
    {
        if (loadSlot.Item != null)
        {
            thisSlot.SetSlot(loadSlot);
            thisSlot.EquipItem<EquipmentItem>();
        }
        else
        {
            thisSlot.ClearSlot();
        }
    }
    public void SaveEquipmentSlot<T>(T saveSlot, T thisSlot) where T: EquipmentSlot
    {
        if (thisSlot.Item != null)
        {
            saveSlot = thisSlot;
        }
        else
        {
            saveSlot = null;
        }
    }

    #region Save & Load
    public void LoadPlayerEquipmentSlots(CharacterData characterData)
    {
        LoadEquipmentSlot(characterData.WeaponSlot, weaponSlot);
        LoadEquipmentSlot(characterData.HelmetSlot, helmetSlot);
        LoadEquipmentSlot(characterData.ArmorSlot, armorSlot);
        LoadEquipmentSlot(characterData.BootsSlot, bootsSlot);
    }

    public void SavePlayerEquipmentSlots(CharacterData characterData)
    {
        SaveEquipmentSlot(characterData.WeaponSlot, weaponSlot);
        SaveEquipmentSlot(characterData.HelmetSlot, helmetSlot);
        SaveEquipmentSlot(characterData.ArmorSlot, armorSlot);
        SaveEquipmentSlot(characterData.BootsSlot, bootsSlot);
    }

    #endregion

    #region Property
    public WeaponSlot WeaponSlot { get { return weaponSlot; } set { weaponSlot = value; } }
    public HelmetSlot HelmetSlot { get { return helmetSlot; } set { helmetSlot = value; } }
    public ArmorSlot ArmorSlot { get { return armorSlot; } set { armorSlot = value; } }
    public BootsSlot BootsSlot { get { return bootsSlot; } set { bootsSlot = value; } }

    #endregion
}
