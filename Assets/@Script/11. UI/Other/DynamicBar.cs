using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DYNAMIC_BAR_UPDATE_TYPE
{
    NONE,
    REDUCTION,
    RECOVERY
}

public class DynamicBar : UIBase
{
    public enum IMAGE
    {
        Main_Fill_Bar,
        Sub_Fill_Bar
    }

    [SerializeField] protected Image mainFillBar;
    [SerializeField] protected Image subFillBar;
    [SerializeField] protected DYNAMIC_BAR_UPDATE_TYPE updateType;

    public void Initialize()
    {
        BindImage(typeof(IMAGE));

        mainFillBar = GetImage((int)IMAGE.Main_Fill_Bar);
        subFillBar = GetImage((int)IMAGE.Sub_Fill_Bar);
        updateType = DYNAMIC_BAR_UPDATE_TYPE.NONE;
    }

    public void UpdateBar(float lastFillRatio, float fillSpeed = 2f)
    {
        // None
        if (mainFillBar.fillAmount == subFillBar.fillAmount)
        {
            updateType = DYNAMIC_BAR_UPDATE_TYPE.NONE;
        }
        // Reduction
        if (lastFillRatio < mainFillBar.fillAmount)
        {
            updateType = DYNAMIC_BAR_UPDATE_TYPE.REDUCTION;
        }
        // Recovery
        else if (lastFillRatio > mainFillBar.fillAmount)
        {
            updateType = DYNAMIC_BAR_UPDATE_TYPE.RECOVERY;
        }
        
        switch (updateType)
        {
            case DYNAMIC_BAR_UPDATE_TYPE.REDUCTION:
                mainFillBar.fillAmount = lastFillRatio;
                subFillBar.fillAmount = Mathf.Lerp(subFillBar.fillAmount, lastFillRatio, fillSpeed * Time.deltaTime);
                break;

            case DYNAMIC_BAR_UPDATE_TYPE.RECOVERY:
                subFillBar.fillAmount = lastFillRatio;
                mainFillBar.fillAmount = Mathf.Lerp(mainFillBar.fillAmount, lastFillRatio, fillSpeed * Time.deltaTime);
                break;
        }
    }
}
