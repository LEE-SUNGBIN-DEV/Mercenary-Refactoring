using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CompetePanel : UIPanel
{
    public enum SLIDER
    {
        Compete_Bar_Slider,
    }

    public enum BUTTON
    {
        AKeyButton,
        DKeyButton
    }

    public enum TEXT
    {
        Compete_Time_Text
    }

    private Slider competeBarSlider;
    private Button AKeyButton;
    private Button DKeyButton;
    private TextMeshProUGUI competeTimeText;

    #region Private
    private void OnEnable()
    {
        if (Managers.Instance != null)
        {
            Managers.SpecialCombatManager.OnChangeCompetePower -= UpdateProgressBar;
            Managers.SpecialCombatManager.OnChangeCompetePower += UpdateProgressBar;

            Managers.SpecialCombatManager.OnChangeCompeteTime -= UpdateCompeteTimeText;
            Managers.SpecialCombatManager.OnChangeCompeteTime += UpdateCompeteTimeText;

            Managers.SpecialCombatManager.OnPressAKey -= OnPressAKey;
            Managers.SpecialCombatManager.OnPressAKey += OnPressAKey;

            Managers.SpecialCombatManager.OnPressDKey -= OnPressDKey;
            Managers.SpecialCombatManager.OnPressDKey += OnPressDKey;
        }
    }

    private void OnDisable()
    {
        if(Managers.Instance != null)
        {
            Managers.SpecialCombatManager.OnChangeCompetePower -= UpdateProgressBar;
            Managers.SpecialCombatManager.OnChangeCompeteTime -= UpdateCompeteTimeText;
            Managers.SpecialCombatManager.OnPressAKey -= OnPressAKey;
            Managers.SpecialCombatManager.OnPressDKey -= OnPressDKey;
        }
    }

    private void UpdateProgressBar(float value)
    {
        competeBarSlider.value = value;
    }
    private void UpdateCompeteTimeText(float value)
    {
        competeTimeText.text = $"{Constants.TIME_COMPETE - value:F2}s";
    }

    private void OnPressAKey()
    {
        ExecuteEvents.Execute(AKeyButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
    }
    private void OnPressDKey()
    {
        ExecuteEvents.Execute(DKeyButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
    }
    #endregion

    public void Initialize()
    {
        BindSlider(typeof(SLIDER));
        BindButton(typeof(BUTTON));
        BindText(typeof(TEXT));

        competeBarSlider = GetSlider((int)SLIDER.Compete_Bar_Slider);
        AKeyButton = GetButton((int)BUTTON.AKeyButton);
        DKeyButton = GetButton((int)BUTTON.DKeyButton);
        competeTimeText = GetText((int)TEXT.Compete_Time_Text);
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
