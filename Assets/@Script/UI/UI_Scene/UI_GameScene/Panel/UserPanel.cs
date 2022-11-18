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

    public override void Initialize()
    {
        BindImage(typeof(IMAGE));

        Managers.DataManager.CurrentCharacter.CharacterData.StatData.OnChangeStatData -= UpdateExpBar;
        Managers.DataManager.CurrentCharacter.CharacterData.StatData.OnChangeStatData += UpdateExpBar;

        Managers.DataManager.CurrentCharacter.CharacterStatus.OnCharacterStatusChanged -= UpdateHPBar;
        Managers.DataManager.CurrentCharacter.CharacterStatus.OnCharacterStatusChanged += UpdateHPBar;

        Managers.DataManager.CurrentCharacter.CharacterStatus.OnCharacterStatusChanged -= UpdateSPBar;
        Managers.DataManager.CurrentCharacter.CharacterStatus.OnCharacterStatusChanged += UpdateSPBar;
    }

    public void UpdateExpBar(CharacterStatData statData)
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
