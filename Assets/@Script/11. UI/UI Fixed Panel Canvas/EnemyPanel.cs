using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyPanel : UIPanel
{
    public enum TEXT
    {
        Enemy_Name_Text
    }
    public enum DYNAMIC_BAR
    {
        HP_Dynamic_Bar,
    }

    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private DynamicBar hpDynamicBar;

    [SerializeField] private BaseEnemy targetEnemy;

    #region Private
    private void OnEnable()
    {
        ConnectData();
    }
    private void OnDisable()
    {
        DisconnectData();
    }
    private void Update()
    {
        if(targetEnemy != null)
        {
            hpDynamicBar.UpdateBar(targetEnemy.Status.GetHPRatio());
        }
    }
    private void ConnectData()
    {
        if (targetEnemy != null)
        {
            targetEnemy.Status.OnChangeEnemyData -= UpdateEnemyPanel;
            targetEnemy.Status.OnChangeEnemyData += UpdateEnemyPanel;

            enemyNameText.text = targetEnemy.Status.EnemyName;
        }
    }
    private void DisconnectData()
    {
        if (targetEnemy != null)
        {
            targetEnemy.Status.OnChangeEnemyData -= UpdateEnemyPanel;
            targetEnemy = null;
        }
    }

    private void UpdateEnemyPanel(EnemyStatus enemyStatus)
    {
        if(enemyNameText.text != enemyStatus.EnemyName)
        {
            enemyNameText.text = enemyStatus.EnemyName;
        }

        if (enemyStatus == null || enemyStatus.GetHPRatio() <= 0)
        {
            ClosePanel();
        }
    }
    #endregion
    public void Initialize()
    {
        base.Awake();
        BindObject<DynamicBar>(typeof(DYNAMIC_BAR));
        BindText(typeof(TEXT));

        enemyNameText = GetText((int)TEXT.Enemy_Name_Text);
        hpDynamicBar = GetObject<DynamicBar>((int)DYNAMIC_BAR.HP_Dynamic_Bar);
        hpDynamicBar.Initialize();
    }

    public void OpenPanel(BaseEnemy enemy)
    {
        targetEnemy = enemy;
        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
