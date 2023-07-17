using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyPanel : UIPanel
{
    public enum IMAGE
    {
        Enemy_HP_Bar,
        Enemy_HP_Trace_Bar
    }
    public enum TEXT
    {
        Enemy_Name_Text
    }

    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private Image enemyHPBar;
    [SerializeField] private Image enemyHPTraceBar;

    private Coroutine updateHPBarCoroutine;
    private Coroutine traceHPBarCoroutine;

    private EnemyStatus enemyStatus;
    private float lastHPRatio;

    public void Initialize()
    {
        BindImage(typeof(IMAGE));
        BindText(typeof(TEXT));

        enemyNameText = GetText((int)TEXT.Enemy_Name_Text);
        enemyHPBar = GetImage((int)IMAGE.Enemy_HP_Bar);
        enemyHPTraceBar = GetImage((int)IMAGE.Enemy_HP_Trace_Bar);
    }

    public void SetTargetEnemy(BaseEnemy targetEnemy)
    {
        enemyStatus = targetEnemy.Status;
        enemyStatus.OnChangeEnemyData += UpdateEnemyPanel;

        enemyNameText.text = enemyStatus.EnemyName;
        enemyHPBar.fillAmount = enemyStatus.GetHPRatio();
    }

    public void UpdateEnemyPanel(EnemyStatus enemyStatus)
    {
        lastHPRatio = enemyStatus.GetHPRatio();

        if(lastHPRatio <= 0)
        {
            StartCoroutine(CoDisablePanel());
        }

        if (isActiveAndEnabled)
        {
            // HP Bar
            if (lastHPRatio != enemyHPBar.fillAmount)
            {
                if (updateHPBarCoroutine != null)
                    StopCoroutine(updateHPBarCoroutine);
                updateHPBarCoroutine = StartCoroutine(CoUpdateBar(enemyHPBar, lastHPRatio));

                if (traceHPBarCoroutine != null)
                    StopCoroutine(traceHPBarCoroutine);
                traceHPBarCoroutine = StartCoroutine(CoTraceBar(enemyHPBar, enemyHPTraceBar));
            }
        }
    }

    private IEnumerator CoDisablePanel(float duration = 3f)
    {
        yield return new WaitForSeconds(duration);

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

        while (traceImage.fillAmount > barImage.fillAmount)
        {
            traceImage.fillAmount = Mathf.Lerp(traceImage.fillAmount, barImage.fillAmount, decreaseSpeed * Time.deltaTime);
            yield return null;
        }

        traceImage.fillAmount = barImage.fillAmount;
    }
}
