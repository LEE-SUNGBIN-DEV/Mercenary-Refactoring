using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestData
{
    public Inner inner;
    public int id;
    public string data;
}

[System.Serializable]
public class Inner
{
    public string name;
}

public class UnitTestScript : MonoBehaviour
{
    [SerializeField] private static TestData[] originalDatas;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        originalDatas = new TestData[3];

        for (int i = 0; i < originalDatas.Length; ++i)
        {
            originalDatas[i] = null;
        }
    }

    public static TestData[] OriginalDatas { get { return originalDatas; } }
}
