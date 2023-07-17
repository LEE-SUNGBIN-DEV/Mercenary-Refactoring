using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UserPanel : UIPanel
{
    public enum IMAGE
    {
        HP_Bar,
        HP_Trace_Bar,
        SP_Bar,
        SP_Trace_Bar,
        Exp_Bar,
        Exp_Trace_Bar,

        Equip_Weapon_Image,
    }
    public enum TEXT
    {
        Equip_Weapon_Name_Text,
        Resonance_Water_Name_Text
    }

    public enum RAW_IMAGE
    {
        Resonance_Water_Raw_Image
    }

    public enum BUTTON
    {
        Prefab_Option_Button,
        Prefab_Quest_Button,
        Prefab_Skill_Button,
        Prefab_Inventory_Button,
        Prefab_Status_Button
    }

    private CharacterData characterData;
    private QuickSlotPanel quickSlotPanel;

    private Image hpBar;
    private Image hpTraceBar;
    private Image spBar;
    private Image spTraceBar;
    private Image expBar;
    private Image expTraceBar;

    private Image equipWeaponImage;
    private RawImage resonanceWaterRawImage;
    private TextMeshProUGUI equipWeaponNameText;
    private TextMeshProUGUI resonanceWaterNameText;

    private float lastHPRatio;
    private float lastSPRatio;
    private float lastExpRatio;

    private Coroutine updateHPBar;
    private Coroutine traceHPBar;
    private Coroutine updateExpBar;
    private Coroutine traceExpBar;

    public void Initialize(CharacterData characterData)
    {
        this.characterData = characterData;

        BindImage(typeof(IMAGE));
        BindText(typeof(TEXT));
        BindObject<RawImage>(typeof(RAW_IMAGE));

        hpBar = GetImage((int)IMAGE.HP_Bar);
        hpTraceBar = GetImage((int)IMAGE.HP_Trace_Bar);
        spBar = GetImage((int)IMAGE.SP_Bar);
        spTraceBar = GetImage((int)IMAGE.SP_Trace_Bar);
        expBar = GetImage((int)IMAGE.Exp_Bar);
        expTraceBar = GetImage((int)IMAGE.Exp_Trace_Bar);
        //
        resonanceWaterNameText = GetText((int)TEXT.Resonance_Water_Name_Text);
        resonanceWaterRawImage = GetObject<RawImage>((int)RAW_IMAGE.Resonance_Water_Raw_Image);
        resonanceWaterRawImage.texture = Managers.ResourceManager.LoadResourceSync<RenderTexture>("Render_Texture_Resonance_Water");

        equipWeaponNameText = GetText((int)TEXT.Equip_Weapon_Name_Text);
        equipWeaponImage = GetImage((int)IMAGE.Equip_Weapon_Image);
        //
        quickSlotPanel = GetComponentInChildren<QuickSlotPanel>(true);
        quickSlotPanel.Initialize(characterData.InventoryData);

        characterData.StatusData.OnChangeStatusData += UpdateUserPanel;
        characterData.InventoryData.OnChangeCharacterWeapon += UpdateEquipWeapon;
        characterData.InventoryData.OnChangeInventoryData += UpdateResonanceWater;

        UpdateUserPanel(characterData.StatusData);
        UpdateEquipWeapon(characterData.InventoryData);
        UpdateResonanceWater(characterData.InventoryData);
    }

    private void OnDestroy()
    {
        if(characterData != null)
        {
            characterData.StatusData.OnChangeStatusData -= UpdateUserPanel;
            characterData.InventoryData.OnChangeCharacterWeapon -= UpdateEquipWeapon;
            characterData.InventoryData.OnChangeInventoryData -= UpdateResonanceWater;
        }
    }

    private void Update()
    {
        // !! SP Bar is called frequently
        lastSPRatio = characterData.StatusData.GetStaminaRatio();

        if (isActiveAndEnabled)
        {
            UpdateBar(spBar, lastSPRatio);
            TraceBar(spBar, spTraceBar);
        }
    }

    private void UpdateBar(Image barImage, float lastRatio)
    {
        float updateSpeed = 4f;

        if (lastRatio > barImage.fillAmount)
            barImage.fillAmount = Mathf.Lerp(barImage.fillAmount, lastRatio, updateSpeed * Time.deltaTime);

        else
            barImage.fillAmount = lastRatio;

    }
    private void TraceBar(Image barImage, Image traceImage)
    {
        float decreaseSpeed = 2f;

        if (traceImage.fillAmount > barImage.fillAmount)
            traceImage.fillAmount = Mathf.Lerp(traceImage.fillAmount, barImage.fillAmount, decreaseSpeed * Time.deltaTime);

        else
            traceImage.fillAmount = barImage.fillAmount;
    }

    public void UpdateEquipWeapon(CharacterInventoryData inventoryData)
    {
        switch (inventoryData.EquippedWeapon)
        {
            case WEAPON_TYPE.None:
                equipWeaponImage.color = Functions.SetColor(Color.white, 0f);
                equipWeaponImage.sprite = null;
                break;
            case WEAPON_TYPE.HALBERD:
                equipWeaponImage.color = Functions.SetColor(Color.white, 1f);
                equipWeaponImage.sprite = Managers.ResourceManager.LoadResourceSync<Sprite>(Constants.Sprite_Thumbnail_Halberd);
                equipWeaponNameText.text = Managers.DataManager.HalberdTable[characterData.InventoryData.HalberdID].name;
                break;
            case WEAPON_TYPE.SWORD_SHIELD:
                equipWeaponImage.color = Functions.SetColor(Color.white, 1f);
                equipWeaponImage.sprite = Managers.ResourceManager.LoadResourceSync<Sprite>(Constants.Sprite_Thumbnail_Sword_Shield);
                equipWeaponNameText.text = Managers.DataManager.SwordShieldTable[characterData.InventoryData.SwordShieldID].name;
                break;
        }
    }

    public void UpdateResonanceWater(CharacterInventoryData inventoryData)
    {
        if (inventoryData.ResonanceWaterGrade == 0)
            resonanceWaterNameText.text = $"공명수 ({inventoryData.ResonanceWaterRemainingCount}/{inventoryData.ResonanceWaterMaxCount})";
        else
            resonanceWaterNameText.text = $"공명수 +{inventoryData.ResonanceWaterGrade} ({inventoryData.ResonanceWaterRemainingCount}/{inventoryData.ResonanceWaterMaxCount})";
    }

    public void UpdateUserPanel(CharacterStatusData status)
    {
        lastHPRatio = status.GetHPRatio();
        lastExpRatio = status.GetExpRatio();

        if (isActiveAndEnabled)
        {
            // HP Bar
            if (lastHPRatio != hpBar.fillAmount)
            {
                if (updateHPBar != null)
                    StopCoroutine(updateHPBar);
                updateHPBar = StartCoroutine(CoUpdateBar(hpBar, lastHPRatio));

                if (traceHPBar != null)
                    StopCoroutine(traceHPBar);
                traceHPBar = StartCoroutine(CoTraceBar(hpBar, hpTraceBar));
            }

            // Exp Bar
            if (lastExpRatio != expBar.fillAmount)
            {
                if (updateExpBar != null)
                    StopCoroutine(updateExpBar);
                updateExpBar = StartCoroutine(CoUpdateBar(expBar, lastExpRatio));

                if (traceExpBar != null)
                    StopCoroutine(traceExpBar);
                traceExpBar = StartCoroutine(CoTraceBar(expBar, expTraceBar));
            }
        }
    }

    private IEnumerator CoUpdateBar(Image barImage, float lastRatio)
    {
        float updateSpeed = 4f;

        while (lastRatio > barImage.fillAmount)
        {
            barImage.fillAmount = Mathf.Lerp(barImage.fillAmount, lastRatio, updateSpeed * Time.deltaTime);
            yield return null;
        }

        barImage.fillAmount = lastRatio;
    }

    private IEnumerator CoTraceBar(Image barImage, Image traceImage)
    {
        float decreaseSpeed = 2f;

        while(traceImage.fillAmount > barImage.fillAmount)
        {
            traceImage.fillAmount = Mathf.Lerp(traceImage.fillAmount, barImage.fillAmount, decreaseSpeed * Time.deltaTime);
            yield return null;
        }

        traceImage.fillAmount = barImage.fillAmount;
    }
}
