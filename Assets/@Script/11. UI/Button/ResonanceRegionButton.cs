using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ResonanceRegionButton : UIBase
{
    public enum BUTTON
    {
        Prefab_Button_Region
    }

    public enum TEXT
    {
        Region_Text
    }

    public event UnityAction<ResonanceRegionButton> OnClickRegionButton;

    [SerializeField] private ResonanceCrystalData resonanceCrystalData;
    [SerializeField] private Button resgionButton;
    [SerializeField] private TextMeshProUGUI regionText;

    public void Initialize(ResonanceCrystalData resonanceCrystalData)
    {
        this.resonanceCrystalData = resonanceCrystalData;

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        resgionButton = GetButton((int)BUTTON.Prefab_Button_Region);
        resgionButton.onClick.AddListener(ClickRegionButton);
        regionText = GetText((int)TEXT.Region_Text);

        SetRegionButton();
    }

    public void SetRegionButton()
    {
        regionText.text = resonanceCrystalData.regionName;
    }

    public void ClickRegionButton()
    {
        OnClickRegionButton?.Invoke(this);
    }

    #region Property
    public ResonanceCrystalData ResonanceCrystalData { get { return resonanceCrystalData; } }
    public Button RegionButton { get { return resgionButton; } }
    public TextMeshProUGUI RegionText { get { return regionText; } }
    #endregion
}
