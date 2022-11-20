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

    public void Initialize(Character character)
    {
        BindImage(typeof(IMAGE));

        character.Status.OnCharacterStatusChanged -= UpdateHPBar;
        character.Status.OnCharacterStatusChanged += UpdateHPBar;
        character.Status.OnCharacterStatusChanged -= UpdateSPBar;
        character.Status.OnCharacterStatusChanged += UpdateSPBar;

        character.CharacterData.StatData.OnChangeStatData -= UpdateExpBar;
        character.CharacterData.StatData.OnChangeStatData += UpdateExpBar;

        UpdateHPBar(character.Status);
        UpdateSPBar(character.Status);
        UpdateExpBar(character.CharacterData.StatData);
    }

    public void UpdateExpBar(StatData statData)
    {
        float ratio = statData.CurrentExperience / statData.MaxExperience;
        GetImage((int)IMAGE.ExpBar).fillAmount = ratio;
    }
    public void UpdateHPBar(CharacterStatus status)
    {
        float ratio = status.CurrentHitPoint / status.MaxHitPoint;
        GetImage((int)IMAGE.HPBar).fillAmount = ratio;
    }
    public void UpdateSPBar(CharacterStatus status)
    {
        float ratio = status.CurrentStamina / status.MaxStamina;
        GetImage((int)IMAGE.SPBar).fillAmount = ratio;
    }
}
