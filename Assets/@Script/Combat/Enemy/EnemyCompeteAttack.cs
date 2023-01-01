using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCompeteAttack : EnemyCombatController
{
    [Header("Enemy Compete Attack")]
    [SerializeField] private Transform playerCompetePoint;
    [SerializeField] private Transform directingCameraPoint;
    private float competeCooldown;
    private bool isCompeteReady;
    private ICompetable competableEnemy;

    public void SetCompeteAttack(BaseEnemy owner)
    {
        this.owner = owner;
        isCompeteReady = true;
        competeCooldown = Constants.TIME_COMPETE_COOLDOWN;
        competableEnemy = owner as ICompetable;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other != null)
            ExecuteAttackProcess(other);
    }

    public bool TryCompete(PlayerShield shieldController)
    {
        if (!isCompeteReady || competableEnemy == null || shieldController.Owner is not ICompetable competableCharacter)
            return false;

        StartCoroutine(CoStartCooldown());
        StartCoroutine(CoStartCompete(competableCharacter));
        return true;
    }

    public IEnumerator CoStartCooldown()
    {
        isCompeteReady = false;
        yield return new WaitForSecondsRealtime(competeCooldown);
        isCompeteReady = true;
    }

    public IEnumerator CoStartCompete(ICompetable competableCharacter)
    {
        competableCharacter?.OnCompete();
        competableEnemy?.OnCompete();

        Functions.SetCharacterTransform(competableCharacter as BaseCharacter, playerCompetePoint);
        Managers.GameManager.PlayerCamera.SetCameraTransform(playerCompetePoint);
        Managers.GameManager.DirectingCamera.SetCameraTransform(directingCameraPoint);

        Managers.GameManager.DirectingCamera.OriginalPosition = directingCameraPoint.position;
        Managers.GameManager.PlayerCamera.gameObject.SetActive(false);
        Managers.GameManager.DirectingCamera.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(Constants.TIME_COMPETE);
        Managers.GameManager.DirectingCamera.gameObject.SetActive(false);
        Managers.GameManager.PlayerCamera.gameObject.SetActive(true);
    }
}
