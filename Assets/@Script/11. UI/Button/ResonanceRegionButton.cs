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

    [SerializeField] private ResonanceObjectData resonanceObjectData;
    [SerializeField] private Button resgionButton;
    [SerializeField] private TextMeshProUGUI regionText;

    public void Initialize(ResonanceObjectData resonanceObjectData)
    {
        this.resonanceObjectData = resonanceObjectData;

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        resgionButton = GetButton((int)BUTTON.Prefab_Button_Region);
        resgionButton.onClick.AddListener(ClickRegionButton);
        regionText = GetText((int)TEXT.Region_Text);

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
    public Button RegionButton { get { return resgionButton; } }
    public TextMeshProUGUI RegionText { get { return regionText; } }
    #endregion
}
