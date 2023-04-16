using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPanel : UIPanel
{
    [SerializeField] private Image bossHPBar;

    public void Initialize()
    {
    }

    public void SetBossHPBar(float ratio)
    {
        bossHPBar.fillAmount = ratio;
    }
}
