using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReturnObject : MonoBehaviour
{
    [Header("Auto Return")]
    [SerializeField] private string key;
    [SerializeField] private float returnTime;
    private float currentTime;

    protected virtual void OnEnable()
    {
        key = gameObject.name;
        currentTime = 0;
    }

    protected virtual void OnDisable()
    {
        currentTime = 0;
    }

    protected virtual void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= returnTime)
        {
            Managers.SceneManagerCS.CurrentScene.ReturnObject(key, gameObject);
        }
    }
}
