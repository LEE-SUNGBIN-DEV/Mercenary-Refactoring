using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CompetePanel : UIPanel
{
    public enum IMAGE
    {
        ProgressBar,
    }

    public enum BUTTON
    {
        AKeyButton,
        DKeyButton
    }

    private Image progressBarImage;
    private Button AKeyButton;
    private Button DKeyButton;

    public void Initialize()
    {
        BindImage(typeof(IMAGE));
        BindButton(typeof(BUTTON));

        progressBarImage = GetImage((int)IMAGE.ProgressBar);
        AKeyButton = GetButton((int)BUTTON.AKeyButton);
        DKeyButton = GetButton((int)BUTTON.DKeyButton);

        Managers.SpecialCombatManager.OnChangeCompetePower -= UpdateProgressBar;
        Managers.SpecialCombatManager.OnChangeCompetePower += UpdateProgressBar;

        Managers.SpecialCombatManager.OnPressAKey -= OnPressAKey;
        Managers.SpecialCombatManager.OnPressAKey += OnPressAKey;

        Managers.SpecialCombatManager.OnPressDKey -= OnPressDKey;
        Managers.SpecialCombatManager.OnPressDKey += OnPressDKey;
    }

    public void UpdateProgressBar(float value)
    {
        progressBarImage.fillAmount = value;
    }

    public void OnPressAKey()
    {
        ExecuteEvents.Execute(AKeyButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
    }

    public void OnPressDKey()
    {
        ExecuteEvents.Execute(DKeyButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
    }
}
