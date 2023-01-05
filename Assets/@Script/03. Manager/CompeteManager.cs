using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompeteManager : MonoBehaviour
{
    private BaseCharacter character;
    private BaseEnemy enemy;
    private ICompetable competableCharacter;
    private ICompetable competableEnemy;
    private Transform competePoint;
    private Transform cameraPoint;

    private bool isReady;
    private float competeCooldown;
    private float competeDuration;
    private float cumulativeTime;
    private float competePower;

    private void Awake()
    {
        isReady = true;
        competeCooldown = Constants.TIME_COMPETE_COOLDOWN;
        competeDuration = Constants.TIME_COMPETE;
        cumulativeTime = 0;
        competePower = 0.5f;
    }

    public bool TryCompete(PlayerDefenseController defenseController, EnemyCompeteAttack competeController)
    {
        character = defenseController.Owner;
        enemy = competeController.Owner;
        competableCharacter = character as ICompetable;
        competableEnemy = enemy as ICompetable;

        if (competableCharacter != null && competableEnemy != null && isReady)
        {
            competePoint = competeController.CompetePoint;
            cameraPoint = competeController.CameraPoint;

            Functions.SetCharacterTransform(defenseController.Owner, competeController.CompetePoint);
            OnStartCompete();

            return true;
        }

        return false;
    }

    public void OnStartCompete()
    {
        StartCoroutine(CoStartCooldown());

        // Camera
        Managers.GameManager.PlayerCamera.SetCameraTransform(competePoint);
        Managers.GameManager.DirectingCamera.SetCameraTransform(cameraPoint);

        Managers.GameManager.DirectingCamera.OriginalPosition = cameraPoint.position;
        Managers.GameManager.PlayerCamera.gameObject.SetActive(false);
        Managers.GameManager.DirectingCamera.gameObject.SetActive(true);

        // Actor
        competableCharacter?.OnCompete();
        competableEnemy?.OnCompete();
    }
    public void OnSuccessCompete()
    {
        OnEndCompete();
    }
    public void OnFailCompete()
    {
        character.SwitchCharacterState(CHARACTER_STATE.HeavyHit);
        OnEndCompete();
    }
    public void OnEndCompete()
    {
        character = null;
        enemy = null;
        competableCharacter = null;
        competableEnemy = null;

        cumulativeTime = 0;
        competePower = 0.5f;
        Managers.GameManager.DirectingCamera.gameObject.SetActive(false);
        Managers.GameManager.PlayerCamera.gameObject.SetActive(true);
    }

    public IEnumerator CoStartCooldown()
    {
        isReady = false;
        yield return new WaitForSeconds(competeCooldown);
        isReady = true;
    }

    #region Property
    public ICompetable CompetableCharacter { get { return competableCharacter; } }
    public ICompetable CompetableEnemy { get { return competableEnemy; } }
    public Transform CompetePoint { get { return competePoint; } }
    public Transform CameraPoint { get { return cameraPoint; } }
    public float CompeteDuration { get { return competeDuration; } }
    public float CumulativeTime { get { return cumulativeTime; } set { cumulativeTime = value; } }
    public float CompetePower
    {
        get { return competePower; }
        set
        {
            competePower = value;

            if (competePower < 0f)
                competePower = 0f;

            if (competePower > 1f)
                competePower = 1f;
        }
    }
    #endregion
}
