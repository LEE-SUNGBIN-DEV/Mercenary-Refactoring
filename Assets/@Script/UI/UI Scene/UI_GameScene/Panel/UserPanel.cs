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

        characterData.StatusData.OnCharacterStatusChanged -= UpdateHPBar;
        characterData.StatusData.OnCharacterStatusChanged += UpdateHPBar;
        characterData.StatusData.OnCharacterStatusChanged -= UpdateSPBar;
        characterData.StatusData.OnCharacterStatusChanged += UpdateSPBar;

        characterData.StatData.OnChangeStatData -= UpdateExpBar;
        characterData.StatData.OnChangeStatData += UpdateExpBar;

        UpdateHPBar(characterData.StatusData);
        UpdateSPBar(characterData.StatusData);
        UpdateExpBar(characterData.StatData);
    }

    public void UpdateExpBar(StatData statData)
    {
        float ratio = statData.CurrentExperience / statData.MaxExperience;
        GetImage((int)IMAGE.ExpBar).fillAmount = ratio;
    }
    public void UpdateHPBar(StatusData status)
    {
        float ratio = status.CurrentHP / status.MaxHP;
        GetImage((int)IMAGE.HPBar).fillAmount = ratio;
    }
    public void UpdateSPBar(StatusData status)
    {
        float ratio = status.CurrentSP / status.MaxSP;
        GetImage((int)IMAGE.SPBar).fillAmount = ratio;
    }
}
