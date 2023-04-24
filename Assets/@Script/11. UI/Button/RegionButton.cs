using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class RegionButton : UIBase
{
    public enum BUTTON
    {
        Prefab_Region_Button
    }

    public enum TEXT
    {
        Prefab_Region_Text
    }

    public event UnityAction<RegionButton> OnClickRegionButton;

    [SerializeField] private ResonanceObjectData resonanceObjectData;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI regionText;

    public void Initialize(ResonanceObjectData resonanceObjectData)
    {
        this.resonanceObjectData = resonanceObjectData;

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        button = GetButton((int)BUTTON.Prefab_Region_Button);
        button.onClick.AddListener(ClickRegionButton);
        regionText = GetText((int)TEXT.Prefab_Region_Text);

        SetRegionButton();
    }

    public void SetRegionButton()
    {
        regionText.text = resonanceObjectData.regionName;
    }

    public void ClickRegionButton()
    {
        OnClickRegionButton?.Invoke(this);
    }

    #region Property
    public ResonanceObjectData ResonanceObjectData { get { return resonanceObjectData; } }
    public Button Button { get { return button; } }
    public TextMeshProUGUI QuestTitleText { get { return regionText; } }
    #endregion
}
