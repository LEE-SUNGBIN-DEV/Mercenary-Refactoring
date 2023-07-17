using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryResonanceSlot : UIBase
{
    public enum RAW_IMAGE
    {
        Resonance_Water_Raw_Image
    }

    public enum TEXT
    {
        Resonance_Water_Name_Text,
        Remaining_Amount_Text,
        HP_Recovery_Amount_Text,
        SP_Recovery_Amount_Text,
    }

    private RawImage resonanceWaterRawImage;
    private TextMeshProUGUI resonanceWaterNameText;
    private TextMeshProUGUI remainingAmountText;
    private TextMeshProUGUI hpAmountText;
    private TextMeshProUGUI spAmountText;

    public void Initialize(CharacterInventoryData inventoryData)
    {
        BindText(typeof(TEXT));
        BindObject<RawImage>(typeof(RAW_IMAGE));

        resonanceWaterRawImage = GetObject<RawImage>((int)RAW_IMAGE.Resonance_Water_Raw_Image);
        resonanceWaterRawImage.texture = Managers.ResourceManager.LoadResourceSync<RenderTexture>("Render_Texture_Resonance_Water");

        resonanceWaterNameText = GetText((int)TEXT.Resonance_Water_Name_Text);
        remainingAmountText = GetText((int)TEXT.Remaining_Amount_Text);
        hpAmountText = GetText((int)TEXT.HP_Recovery_Amount_Text);
        spAmountText = GetText((int)TEXT.SP_Recovery_Amount_Text);
    }

    public void LoadData(CharacterInventoryData inventoryData)
    {
        if (inventoryData.ResonanceWaterGrade == 0)
            resonanceWaterNameText.text = "공명수";
        else
            resonanceWaterNameText.text = $"공명수 +{inventoryData.ResonanceWaterGrade}";

        remainingAmountText.text = $"({inventoryData.ResonanceWaterRemainingCount}/{inventoryData.ResonanceWaterMaxCount})";
        hpAmountText.text = inventoryData.ResonanceWaterRecoverAmount.ToString() + "%";
        spAmountText.text = inventoryData.ResonanceWaterRecoverAmount.ToString() + "%";
    }
}
