using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

public class SpecialCombatManager : MonoBehaviour
{
    private PlayerCharacter character;
    private BaseEnemy enemy;
    private bool isReady;
    private float cooldown;

    // Counter
    public event UnityAction OnCounter;

    // Compete
    public event UnityAction<float> OnChangeCompetePower;
    public event UnityAction OnStartCompete;
    public event UnityAction OnEndCompete;
    public event UnityAction OnPressAKey;
    public event UnityAction OnPressDKey;

    private ICompetable competableCharacter;
    private ICompetable competableEnemy;
    private Transform competePoint;
    private Transform cameraPoint;

    private Coroutine competeCooldownCoroutine;
    private Coroutine competeControlCoroutine;

    private float competeDuration;
    private float cumulativeTime;
    private float competePower;

    private GameObject competeStartVFX;
    private GameObject competeSuccessVFX;
    private GameObject competingVFX;

    public void Initialize()
    {
        isReady = true;
        cooldown = Constants.TIME_COMPETE_COOLDOWN;
        competeDuration = Constants.TIME_COMPETE;
        cumulativeTime = 0;
        competePower = 0.5f;

        competeStartVFX = Managers.ResourceManager.InstantiatePrefabSync("VFX_Compete_Start");
        competeStartVFX.transform.SetParent(transform);

        competeSuccessVFX = Managers.ResourceManager.InstantiatePrefabSync("VFX_Compete_Start");
        competeSuccessVFX.transform.SetParent(transform);

        competingVFX = Managers.ResourceManager.InstantiatePrefabSync("VFX_Competing");
        competingVFX.transform.SetParent(transform);
    }

    private void OnDestroy()
    {
        OnStartCompete = null;
        OnEndCompete = null;
        OnChangeCompetePower = null;
        OnPressAKey = null;
        OnPressDKey = null;
    }

    public bool TryCounter(PlayerCombatController weaponController, EnemyCompeteAttack competeController)
    {
        character = weaponController.Character;
        enemy = competeController.Enemy;
        competableEnemy = enemy as ICompetable;

        if (character != null && competableEnemy != null && isReady)
        {
            competeController.OnDisableCollider();

            OnCounter?.Invoke();

            // Camera
            character.PlayerCamera.ShakeCamera(1f, 0.1f);

            Managers.PostProcessingManager.BeatBloom(120f, 0.3f);
            Managers.PostProcessingManager.BeatChromaticAberration(1f, 1f);
            Managers.AudioManager.PlaySFX(Constants.Audio_Shield_Parrying);

            competeStartVFX.transform.SetPositionAndRotation(character.transform.position, character.transform.rotation);
            competeStartVFX.SetActive(true);

            enemy.State.SetState(ACTION_STATE.ENEMY_STAGGER, STATE_SWITCH_BY.FORCED, 5f);
            competeCooldownCoroutine = StartCoroutine(CoStartCooldown());

            competableEnemy = null;
            character = null;
            enemy = null;

            return true;
        }
        return false;
    }

    public bool TryCompete(PlayerCombatController weaponController, EnemyCompeteAttack competeController)
    {
        character = weaponController.Character;
        enemy = competeController.Enemy;
        competableCharacter = character as ICompetable;
        competableEnemy = enemy as ICompetable;

        if (competableCharacter != null && competableEnemy != null && isReady)
        {
            competeController.OnDisableCollider();

            OnStartCompete?.Invoke();
            competePoint = competeController.CompetePoint;
            cameraPoint = competeController.CameraPoint;

            character.transform.SetPositionAndRotation(competeController.CompetePoint.position, competeController.CompetePoint.rotation);

            // Camera
            character.PlayerCamera.ActiveFixedMode(true);
            character.PlayerCamera.ShakeCameraInplace(Constants.TIME_COMPETE);
            character.PlayerCamera.SetCameraPositionAndRotation(competeController.CameraPoint.position, competeController.CameraPoint.rotation);

            Managers.PostProcessingManager.BeatBloom(120f, 0.5f);
            Managers.PostProcessingManager.BeatChromaticAberration(1f, 1f);
            Managers.AudioManager.PlaySFX(Constants.Audio_Shield_Parrying);

            competeStartVFX.transform.SetPositionAndRotation(character.transform.position, character.transform.rotation);
            competeStartVFX.SetActive(true);
            competingVFX.transform.SetPositionAndRotation(character.transform.position, character.transform.rotation);
            competingVFX.SetActive(true);

            competeCooldownCoroutine = StartCoroutine(CoStartCooldown());
            competeControlCoroutine = StartCoroutine(CoCompeteControl());

            return true;
        }
        return false;
    }

    public IEnumerator CoStartCooldown()
    {
        isReady = false;
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }

    public IEnumerator CoCompeteControl()
    {
        float competingTime = 0f;
        // Actor
        competableCharacter?.OnCompete();
        competableEnemy?.OnCompete();

        while (true)
        {
            competingTime += Time.deltaTime;
            CompetePower -= (0.3f * Time.deltaTime);

            if (competingTime > 1f)
            {
                Managers.AudioManager.PlaySFX(Constants.Audio_Competing);
                competingTime = 0f;
            }

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
                PlayerSuccessCompete();

            // Compete Fail Condition
            if (competePower <= 0f || cumulativeTime >= competeDuration)
                PlayerFailCompete();

            yield return null;
        }
    }

    public void PlayerSuccessCompete()
    {
        competeSuccessVFX.transform.SetPositionAndRotation(character.transform.position, character.transform.rotation);
        competeSuccessVFX.SetActive(true);

        enemy.TakeDamage(character, 5f);
        enemy.State.SetState(ACTION_STATE.ENEMY_STAGGER, STATE_SWITCH_BY.FORCED, 5f);

        character.State.SetState(ACTION_STATE.PLAYER_COMPETE_SUCCESS, STATE_SWITCH_BY.FORCED);

        EndCompete();
    }

    public void PlayerFailCompete()
    {
        enemy.State.SetState(ACTION_STATE.ENEMY_COMPETE_SUCCESS, STATE_SWITCH_BY.FORCED);

        character.Status.ReduceHP(50f, CALCULATE_MODE.Ratio);
        character.State.SetState(ACTION_STATE.PLAYER_HIT_HEAVY, STATE_SWITCH_BY.FORCED);

        EndCompete();
    }

    public void EndCompete()
    {
        OnEndCompete?.Invoke();

        StopCoroutine(competeControlCoroutine);

        character.PlayerCamera.StopShakeCameraInplace();
        character.PlayerCamera.ActiveFixedMode(false);
        character.PlayerCamera.SetCameraPositionAndRotation(character.PlayerCamera.OriginalPosition, character.PlayerCamera.OriginalRotation);

        competingVFX.SetActive(false);

        cumulativeTime = 0;
        competePower = 0.5f;

        competableCharacter = null;
        competableEnemy = null;
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
            competePower = Mathf.Clamp01(competePower);
            OnChangeCompetePower?.Invoke(competePower);
        }
    }
    #endregion
}
