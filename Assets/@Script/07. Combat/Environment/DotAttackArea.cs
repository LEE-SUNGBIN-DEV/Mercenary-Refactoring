using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotAttackArea : MonoBehaviour, IPoolObject
{
    [Header("Dot Attack Area")]
    [SerializeField] private float damageRatio;
    [SerializeField] private float damageInterval;
    [SerializeField] private float duration;
    [SerializeField] private Vector3 boxHalfScale;
    private IEnumerator autoReturnCoroutine;
    private IEnumerator dotDamageCoroutine;

    private void Awake()
    {
        autoReturnCoroutine = CoAutoReturn();
        dotDamageCoroutine = CoCastDotDamage();
    }

    public void ExecuteDotDamageProcess(Collider target)
    {
        if (target.TryGetComponent(out PlayerCharacter character))
        {
            if(character.HitState != HIT_STATE.Invincible)
            {
                character.Status.ReduceHP(damageRatio, CALCULATE_MODE.Ratio);
            }
        }
    }

    public IEnumerator CoCastDotDamage()
    {
        while(true)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, boxHalfScale);

            for (int i = 0; i < colliders.Length; i++)
                ExecuteDotDamageProcess(colliders[i]);

            yield return new WaitForSeconds(damageInterval);
        }
    }

    public IEnumerator CoAutoReturn()
    {
        yield return new WaitForSeconds(duration);
        ReturnOrDestoryObject();
    }

    #region IPoolObject Interface Fucntion
    public void ActionAfterRequest(ObjectPooler owner)
    {
        autoReturnCoroutine = CoAutoReturn();
        dotDamageCoroutine = CoCastDotDamage();
        ObjectPooler = owner;

        if (autoReturnCoroutine != null)
            StartCoroutine(autoReturnCoroutine);

        if (dotDamageCoroutine != null)
            StartCoroutine(dotDamageCoroutine);
    }

    public void ActionBeforeReturn()
    {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particles.Length; ++i)
            particles[i].Stop();

        if (autoReturnCoroutine != null)
            StopCoroutine(autoReturnCoroutine);

        if (dotDamageCoroutine != null)
            StopCoroutine(dotDamageCoroutine);
    }

    public void ReturnOrDestoryObject()
    {
        if (ObjectPooler == null)
            Destroy(gameObject);

        ObjectPooler.ReturnObject(name, gameObject);
    }

    public ObjectPooler ObjectPooler { get; set; }
    #endregion
}
