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
    private ObjectPooler objectPooler;

    private void Awake()
    {
        autoReturnCoroutine = CoAutoReturn();
        dotDamageCoroutine = CoCastDotDamage();
    }

    public void ExecuteDotDamageProcess(Collider target)
    {
        if (target.TryGetComponent(out BaseCharacter character))
        {
            if(character.IsInvincible == false)
            {
                character.StatusData.CurrentHP -= (character.StatusData.MaxHP * damageRatio * 0.01f);
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
        ReturnOrDestoryObject(objectPooler);
    }

    #region IPoolObject Interface Fucntion
    public void ActionAfterRequest(ObjectPooler owner)
    {
        autoReturnCoroutine = CoAutoReturn();
        dotDamageCoroutine = CoCastDotDamage();
        objectPooler = owner;

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

    public void ReturnOrDestoryObject(ObjectPooler owner)
    {
        if (owner == null)
            Destroy(gameObject);

        owner.ReturnObject(name, gameObject);
    }

    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    #endregion
}
