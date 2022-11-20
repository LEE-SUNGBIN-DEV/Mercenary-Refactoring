using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossRoomController : MonoBehaviour
{
    public static event UnityAction<float> OnUpdateBossHPBar;

    [SerializeField] private Enemy boss;
    [SerializeField] private Vector3 bossSpawnPoint;
    [SerializeField] private GameObject[] bossRoomBarriers;
    private Collider entranceCollider;

    private void OnEnable()
    {
        entranceCollider = GetComponent<Collider>();

        boss.OnChangeCurrentHP -= UpdateBossHPBar;
        boss.OnChangeCurrentHP += UpdateBossHPBar;

        boss.OnDie -= ClearDungeon;
        boss.OnDie += ClearDungeon;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartBattle();
        }
    }

    public void StartBattle()
    {
        entranceCollider.enabled = false;

        if (bossRoomBarriers.Length > 0)
        {
            for(int i=0; i<bossRoomBarriers.Length; ++i)
            {
                bossRoomBarriers[i].SetActive(true);
            }
        }

        //Managers.UIManager.UIGameScene.MonsterPanel.SetBossHPBar(1f);
    }

    public void UpdateBossHPBar(Enemy enemy)
    {
        float ratio = enemy.CurrentHitPoint / enemy.MaxHitPoint;
        if (OnUpdateBossHPBar != null)
        {
            OnUpdateBossHPBar(ratio);
        }
    }

    public void ClearDungeon(Enemy monster)
    {
        StartCoroutine(CoDungeonClear());
    }

    IEnumerator CoDungeonClear()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(2f);
        Time.timeScale = 1f;

        boss.OnChangeCurrentHP -= UpdateBossHPBar;
        boss.OnDie -= ClearDungeon;

        //Managers.UIManager.BossPanel.SetBossHPBar(1f);
        Managers.UIManager.RequestNotice("던전 클리어");

        Managers.ResourceManager.LoadResourceAsync<GameObject>("Prefab_Vilage_Portal");
    }
}
