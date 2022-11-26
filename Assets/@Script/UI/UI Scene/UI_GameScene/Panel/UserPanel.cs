using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserPanel : UIPanel
{
    public enum IMAGE
    {
        HPBar,
        SPBar,
        ExpBar
    }

    private QuickSlotPanel quickSlotPanel;

    public void Initialize(CharacterData characterData)
    {
        BindImage(typeof(IMAGE));

        quickSlotPanel = GetComponentInChildren<QuickSlotPanel>(true);
        quickSlotPanel.Initialize(characterData.InventoryData);

        characterData.StatusData.OnCharacterStatusChanged -= UpdateUserPanel;
        characterData.StatusData.OnCharacterStatusChanged += UpdateUserPanel;

        UpdateUserPanel(characterData.StatusData);
    }

    public void UpdateUserPanel(StatusData status)
    {
        float ratio = status.CurrentHP / status.MaxHP;
        GetImage((int)IMAGE.HPBar).fillAmount = ratio;

        ratio = status.CurrentExp / status.MaxExp;
        GetImage((int)IMAGE.ExpBar).fillAmount = ratio;

        ratio = status.CurrentSP / status.MaxSP;
        GetImage((int)IMAGE.SPBar).fillAmount = ratio;
    }
}
