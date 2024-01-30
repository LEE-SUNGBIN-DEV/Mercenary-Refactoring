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
    public event UnityAction<float> OnChangeCompeteTime;
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

    private float cumulativeTime;
    private float competePower;

    private GameObject competeStartVFX;
    private GameObject competeSuccessVFX;
    private GameObject competingVFX;

    public void Initialize()
    {
        isReady = true;
        cooldown = Constants.TIME_COMPETE_COOLDOWN;
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
            Managers.PostProcessingManager.EnableShockWave(1f);
            Managers.AudioManager.PlaySFX(Constants.Audio_Shield_Parrying);

            competeStartVFX.transform.SetPositionAndRotation(character.transform.position, character.transform.rotation);
            competeStartVFX.SetActive(true);

            enemy.State.SetState(ACTION_STATE.ENEMY_STAGGER, STATE_SWITCH_BY.WEIGHT, 5f);
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
            Managers.PostProcessingManager.EnableShockWave(Constants.TIME_COMPETE);
            Managers.AudioManager.PlaySFX(Constants.Audio_Shield_Parrying);

            competeStartVFX.transform.SetPositionAndRotation(character.transform.position, character.transform.rotation);
            competeStartVFX.SetActive(true);
            competingVFX.transform.SetPositionAndRotation(character.transform.position, character.transform.rotation);
            competingVFX.SetActive(true);

            competeCooldownCoroutine = StartCoroutine(CoStartCooldown());
            competeControlCoroutine = StartCoroutine(CoCompeteControl());

            Managers.UIManager.UIFixedPanelCanvas.CompetePanel.OpenPanel();
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
            competePower -= (0.3f * Time.deltaTime);

            if (competingTime > 1f)
            {
                Managers.AudioManager.PlaySFX(Constants.Audio_Competing);
                competingTime = 0f;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                OnPressAKey?.Invoke();
                competePower += 0.06f;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                OnPressDKey?.Invoke();
                competePower += 0.06f;
            }

            if (cumulativeTime < Constants.TIME_COMPETE)
                cumulativeTime += Time.deltaTime;

            // Compete Success Condition
            if (competePower >= 1.0f)
                PlayerSuccessCompete();

            // Compete Fail Condition
            if (competePower <= 0f || cumulativeTime >= Constants.TIME_COMPETE)
                PlayerFailCompete();

            cumulativeTime = Mathf.Clamp(cumulativeTime, 0, Constants.TIME_COMPETE);
            OnChangeCompeteTime?.Invoke(cumulativeTime);

            competePower = Mathf.Clamp01(competePower);
            OnChangeCompetePower?.Invoke(competePower);

            yield return null;
        }
    }

    public void PlayerSuccessCompete()
    {
        competeSuccessVFX.transform.SetPositionAndRotation(character.transform.position, character.transform.rotation);
        competeSuccessVFX.SetActive(true);

        enemy.State.SetState(ACTION_STATE.ENEMY_STAGGER, STATE_SWITCH_BY.FORCED, 5f);
        enemy.TakeDamage(character, 5f);
        character.State.SetState(ACTION_STATE.PLAYER_COMPETE_SUCCESS, STATE_SWITCH_BY.WEIGHT);

        EndCompete();
    }

    public void PlayerFailCompete()
    {
        enemy.State.SetState(ACTION_STATE.ENEMY_COMPETE_SUCCESS, STATE_SWITCH_BY.WEIGHT);
        character.State.SetState(ACTION_STATE.PLAYER_HIT_HEAVY, STATE_SWITCH_BY.FORCED);
        character.StatusData.ReduceHP(50f, VALUE_TYPE.PERCENTAGE);

        EndCompete();
    }

    public void EndCompete()
    {
        OnEndCompete?.Invoke();
        StopCoroutine(competeControlCoroutine);

        Managers.UIManager.UIFixedPanelCanvas.CompetePanel.ClosePanel();
        Managers.PostProcessingManager.DisableShockWave();

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
    }
    #endregion
}
