using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTooltipPanel : UIBase
{
    private ISkillTooltipModule[] tooltipModules;
    private LayoutGroup[] layoutGroups;

    private RectTransform rectTransform;

    public void Initialize()
    {
        TryGetComponent(out rectTransform);

        layoutGroups = GetComponentsInChildren<LayoutGroup>(true);
        tooltipModules = GetComponentsInChildren<ISkillTooltipModule>(true);
        for (int i = 0; i < tooltipModules.Length; i++)
        {
            tooltipModules[i].Initialize();
        }
        HideTooltip();
    }

    public void ShowTooltip(SkillData skillData, RectTransform targetRectTransform)
    {
        if (skillData == null || rectTransform == null)
        {
            HideTooltip();
            return;
        }

        for (int i = 0; i < tooltipModules.Length; i++)
        {
            tooltipModules[i].UpdateModule(skillData);
        }
        gameObject.SetActive(true);
        Functions.RebuildLayout(layoutGroups);
        rectTransform.position = targetRectTransform.position + new Vector3(targetRectTransform.sizeDelta.x * 0.5f, targetRectTransform.sizeDelta.y * 0.5f, 0);
    }

    public void HideTooltip()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
