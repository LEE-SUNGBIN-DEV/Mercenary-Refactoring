using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatController : BaseCombatController
{
    protected Collider weaponCollider;
    protected Character owner;
    protected IEnumerator slowMotionCoroutine;

    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        weaponCollider.enabled = false;
        slowMotionCoroutine = null;
    }

    private void OnDisable()
    {
        if (slowMotionCoroutine != null)
        {
            StopCoroutine(slowMotionCoroutine);
            Time.timeScale = 1f;
        } 
    }

    public void CallSlowMotion(float timeScale, float duration)
    {
        if (slowMotionCoroutine == null)
        {
            slowMotionCoroutine = SlowMotion(timeScale, duration);
            StartCoroutine(slowMotionCoroutine);
        }
    }

    public IEnumerator SlowMotion(float timeScale, float duration)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        slowMotionCoroutine = null;
    }

    public void StartSkill()
    {
        combatType = COMBAT_TYPE.COUNTER_SKILL;
        damageRatio = 2f;
        weaponCollider.enabled = true;
    }
    public void EndSkill()
    {
        weaponCollider.enabled = false;
    }

    #region Property
    public Character Owner
    {
        get { return owner; }
    }
    public Collider WeaponCollider
    {
        get { return weaponCollider; }
    }
    #endregion
}
