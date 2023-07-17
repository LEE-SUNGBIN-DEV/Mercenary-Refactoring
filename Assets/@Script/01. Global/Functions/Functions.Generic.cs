using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static partial class Functions
{
    public static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        T targetComponent = gameObject.GetComponent<T>();

        if (targetComponent == null)
        {
            targetComponent = gameObject.AddComponent<T>();
        }

        return targetComponent;
    }

    public static GameObject FindObjectFromChild(GameObject rootObject, string name, bool recursive = false)
    {
        if (rootObject == null)
            return null;

        for (int i = 0; i < rootObject.transform.childCount; i++)
        {
            Transform childTransform = rootObject.transform.GetChild(i);

            if (string.IsNullOrEmpty(name) || childTransform.name == name)
                return childTransform.gameObject;

            if(recursive)
            {
                GameObject findObject = FindObjectFromChild(childTransform.gameObject, name, recursive);
                if (findObject != null)
                    return findObject;
            }
        }
        return null;
    }

    public static T FindChild<T>(GameObject rootObject, string name = null, bool recursive = false) where T : Object
    {
        if (rootObject == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < rootObject.transform.childCount; i++)
            {
                Transform childTransform = rootObject.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || childTransform.name == name)
                {
                    T component = childTransform.GetComponent<T>();

                    if (component != null)
                    {
                        return component;
                    }
                }
            }
        }

        else
        {
            foreach (T component in rootObject.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }

        return null;
    }
}
