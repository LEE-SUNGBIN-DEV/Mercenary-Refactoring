using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPanel : UIPanel
{
    [SerializeField] private Image bossHPBar;

    public void Initialize()
    {
        BossRoomController.OnUpdateBossHPBar -= SetBossHPBar;
        BossRoomController.OnUpdateBossHPBar += SetBossHPBar;
    }

    public void SetBossHPBar(float ratio)
    {
        bossHPBar.fillAmount = ratio;
    }
}
