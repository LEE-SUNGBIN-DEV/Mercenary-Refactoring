using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PARTICLE_MODE
{
    AUTO_RETURN,
    AUTO_DISABLE,
    AUTO_DESTROY
}

public class ParticleController : MonoBehaviour, IPoolObject
{
    private ParticleSystem[] particleSystems;
    [SerializeField] private PARTICLE_MODE particleMode;
    [SerializeField] private float duration;

    private Coroutine autoModeCoroutine;

    private void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    public void Initialize(PARTICLE_MODE particleMode, float duration = 0f)
    {
        this.particleMode = particleMode;
        this.duration = duration;
    }

    private void OnEnable()
    {
        StartByAutoMode();
    }

    public void PlayAllParticles()
    {
        for (int i = 0; i < particleSystems.Length; ++i)
            particleSystems[i].Play();
    }

    public void StopAllParticles()
    {
        for (int i = 0; i < particleSystems.Length; ++i)
            particleSystems[i].Stop();
    }

    public void GenerateObjectAt(string key, Vector3 position, Vector3 rotation)
    {
        GameObject newGameObject = Managers.ResourceManager.LoadResourceSync<GameObject>(key);
        newGameObject.transform.SetPositionAndRotation(position, Quaternion.LookRotation(rotation));
    }

    public void StartByAutoMode()
    {
        if (autoModeCoroutine != null)
            StopCoroutine(autoModeCoroutine);

        autoModeCoroutine = StartCoroutine(CoAutoParticleMode());
    }

    public IEnumerator CoAutoParticleMode()
    {
        PlayAllParticles();
        yield return new WaitForSeconds(duration);
        StopAllParticles();

        switch(particleMode)
        {
            case PARTICLE_MODE.AUTO_RETURN:

                ReturnOrDestoryObject();
                break;

            case PARTICLE_MODE.AUTO_DISABLE:

                gameObject.SetActive(false);
                break;

            case PARTICLE_MODE.AUTO_DESTROY:

                Destroy(gameObject);
                break;
        }
    }

    #region IPoolObject Interface Fucntion
    public void ActionAfterRequest(ObjectPooler owner)
    {
        ObjectPooler = owner;
    }
    public void ActionBeforeReturn()
    {
    }
    public void ReturnOrDestoryObject()
    {
        if (ObjectPooler == null)
            Destroy(gameObject);

        ObjectPooler.ReturnObject(name, gameObject);
    }
    #endregion

    public ObjectPooler ObjectPooler { get; set; }
}
