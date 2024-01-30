using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static bool isInitialized = false;
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                if(isInitialized == true)
                {
#if UNITY_EDITOR
                    Debug.Log($"Now Destroying..");
#endif
                    return null;
                }

                GameObject root = GameObject.Find(typeof(T).Name);
                if (root == null)
                {
                    root = new GameObject(typeof(T).Name);
                }

                instance = Functions.GetOrAddComponent<T>(root);
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    public abstract void Initialize();
}
