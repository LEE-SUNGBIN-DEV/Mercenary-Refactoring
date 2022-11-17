using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTestScript2 : MonoBehaviour
{
    private TestData[] copyData;

    private void Start()
    {
        Load();
    }

    public void Load()
    {
        copyData = UnitTestScript.OriginalDatas;

        for (int i = 0; i < copyData.Length; ++i)
        {
            Debug.Log($"{copyData[i]}");
            if (copyData[i] != null)
            {
                LoadData();
            }
            else
            {
                Debug.Log("Not Called");
                // !!Do Nothing
            }
        }
    }

    public void LoadData()
    {
        Debug.Log("Called");
    }
}
