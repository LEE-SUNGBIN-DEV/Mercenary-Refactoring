using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    [SerializeField] protected SCENE_ID scene;
    [SerializeField] protected SCENE_TYPE sceneType;
    [SerializeField] protected string sceneName;
    [SerializeField] protected ObjectPooler objectPooler = new ObjectPooler();

    protected virtual void Awake()
    {
        Managers.Instance.Initialize();

        // Creat Event System
        GameObject eventSystem = GameObject.Find(Constants.PREFAB_EVENT_SYSTEM);
        if (eventSystem == null)
        {
            Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_EVENT_SYSTEM);
        }

        sceneType = SCENE_TYPE.Unknown;

        Initialize();
    }

    public virtual void Initialize()
    {
        Managers.SceneManagerEX.CurrentScene = this;
        objectPooler.Initialize(transform);
    }

    public virtual void ExitScene()
    {
        Managers.SceneManagerEX.CurrentScene = null;
    }

    public void RegistObject(string key, int amount)
    {
        objectPooler.RegistObject(key, amount);
    }

    public GameObject RequestObject(string key)
    {
        return objectPooler.RequestObject(key);
    }

    public void ReturnObject(string key, GameObject returnObject)
    {
        objectPooler.ReturnObject(key, returnObject);
    }

    #region Property
    public SCENE_ID Scene { get { return scene; } }
    public SCENE_TYPE SceneType { get { return sceneType; } }
    public string SceneName { get { return sceneName; } }
    #endregion
}
