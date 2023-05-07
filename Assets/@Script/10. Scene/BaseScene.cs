using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    [SerializeField] protected SCENE_LIST scene;
    [SerializeField] protected SCENE_TYPE sceneType;
    [SerializeField] protected string sceneName;
    [SerializeField] protected ObjectPooler objectPooler = new ObjectPooler();

    protected virtual void Awake()
    {
        Managers.Instance.Initialize();

        // Creat Event System
        GameObject eventSystem = GameObject.Find(Constants.Prefab_Event_System);
        if (eventSystem == null)
        {
            Managers.ResourceManager.InstantiatePrefabSync(Constants.Prefab_Event_System);
        }

        sceneType = SCENE_TYPE.Unknown;

        Initialize();
    }

    public virtual void Initialize()
    {
        Managers.SceneManagerCS.CurrentScene = this;
        objectPooler.Initialize(transform);
    }

    public virtual void ExitScene()
    {
        Managers.SceneManagerCS.CurrentScene = null;
    }

    public void RegisterObject(string key, int amount)
    {
        objectPooler.RegisterObject(key, amount);
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
    public SCENE_LIST Scene { get { return scene; } }
    public SCENE_TYPE SceneType { get { return sceneType; } }
    public string SceneName { get { return sceneName; } }
    #endregion
}
