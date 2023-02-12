using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CompeteManager : MonoBehaviour
{
    public event UnityAction OnStartCompete;
    public event UnityAction OnEndCompete;
    public event UnityAction<float> OnChangeCompetePower;
    public event UnityAction OnPressAKey;
    public event UnityAction OnPressDKey;

    private IEnumerator competeControlFunction;

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

    private bool isSuccess;

    private void Awake()
    {
        isReady = true;
        competeCooldown = Constants.TIME_COMPETE_COOLDOWN;
        competeDuration = Constants.TIME_COMPETE;
        cumulativeTime = 0;
        competePower = 0.5f;
        isSuccess = false;
    }

    public bool TryCompete(PlayerDefenseController defenseController, EnemyCompeteAttack competeController)
    {
        character = defenseController.Owner;
        enemy = competeController.Owner;
        competableCharacter = character as ICompetable;
        competableEnemy = enemy as ICompetable;

        if (competableCharacter != null && competableEnemy != null && isReady)
        {
            competeControlFunction = CoCompeteControl();

            competePoint = competeController.CompetePoint;
            cameraPoint = competeController.CameraPoint;

            OnStartCompete?.Invoke();
            StartCoroutine(CoStartCooldown());
            StartCoroutine(competeControlFunction);
            Functions.SetCharacterTransform(character, competeController.CompetePoint);

            return true;
        }

        return false;
    }

    public IEnumerator CoStartCooldown()
    {
        isReady = false;
        yield return new WaitForSeconds(competeCooldown);
        isReady = true;
        Debug.Log("Compete Ready");
    }

    public IEnumerator CoCompeteControl()
    {
        Debug.Log("Compete Control");
        // Camera
        Managers.GameManager.PlayerCamera.SetCameraTransform(competePoint);
        Managers.GameManager.DirectingCamera.SetCameraTransform(cameraPoint);

        Managers.GameManager.DirectingCamera.OriginalPosition = cameraPoint.position;
        Managers.GameManager.PlayerCamera.gameObject.SetActive(false);
        Managers.GameManager.DirectingCamera.gameObject.SetActive(true);

        // Actor
        competableCharacter?.OnCompete();
        competableEnemy?.OnCompete();

        isSuccess = false;

        while (true)
        {
            CompetePower -= (0.3f * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.A))
            {
                OnPressAKey?.Invoke();
                CompetePower += 0.06f;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                OnPressDKey?.Invoke();
                CompetePower += 0.06f;
            }

            if (cumulativeTime < competeDuration)
                cumulativeTime += Time.deltaTime;

            // Compete Success Condition
            if (CompetePower >= 1.0f)
                SuccessCompete();

            // Compete Fail Condition
            if (competePower <= 0f || cumulativeTime >= competeDuration)
                FailCompete();

            yield return null;
        }
    }

    public void SuccessCompete()
    {
        isSuccess = true;
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_COMPETE_SUCCESS);
        enemy.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_COMPETE_FAIL);
        EndCompete();
    }

    public void FailCompete()
    {
        isSuccess = false;
        enemy.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_COMPETE_SUCCESS);
        character.SetState(CHARACTER_STATE.HeavyHit);
        EndCompete();
    }

    public void EndCompete()
    {
        OnEndCompete?.Invoke();
        StopCoroutine(competeControlFunction);

        if(!isSuccess)
        {
            character = null;
            enemy = null;
        }
        competableCharacter = null;
        competableEnemy = null;

        cumulativeTime = 0;
        competePower = 0.5f;

        Managers.GameManager.DirectingCamera.gameObject.SetActive(false);
        Managers.GameManager.PlayerCamera.gameObject.SetActive(true);
    }

    public void OnEnemyFail()
    {
        enemy.SwitchState(ENEMY_STATE.Stagger);
        character = null;
        enemy = null;
    }

    #region Property
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

            OnChangeCompetePower?.Invoke(competePower);
        }
    }
    #endregion
}
