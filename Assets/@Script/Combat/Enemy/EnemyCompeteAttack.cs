using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCompeteAttack : EnemyMeleeAttack
{
    [Header("Enemy Compete Attack")]
    [SerializeField] private Transform playerCompetePoint;
    [SerializeField] private Transform directingCameraPoint;
    [SerializeField] private float cooldown;
    private bool isCompeteReady;
    private ICompetable competableEnemy;

    public void SetCompeteAttack()
    {
        isCompeteReady = true;
        competableEnemy = owner as ICompetable;
    }

    protected override void ExecuteAttackProcess(Collider target)
    {
        base.ExecuteAttackProcess(target);

        if (target.TryGetComponent(out PlayerWeapon combatController))
        {
            if (combatController.CombatType == HIT_TYPE.Parrying && isCompeteReady == true)
            {
                Vector3 triggerPoint = target.bounds.ClosestPoint(transform.position);
                Compete(combatController);
            }
        }
    }

    public void Compete(PlayerWeapon combatController)
    {
        StartCoroutine(CoCompeteCooldown());
        StartCoroutine(CoCompete(combatController));
    }

    public IEnumerator CoCompeteCooldown()
    {
        isCompeteReady = false;
        yield return new WaitForSecondsRealtime(cooldown);
        isCompeteReady = true;
    }

    public IEnumerator CoCompete(PlayerWeapon combatController)
    {
        ICompetable competableCharacter = combatController.Owner as ICompetable;
        competableCharacter?.Compete();
        competableEnemy?.Compete();

        Functions.SetCharacterTransform(combatController.Owner, playerCompetePoint);
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
