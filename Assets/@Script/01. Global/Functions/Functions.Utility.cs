using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static partial class Functions
{
    public static void RebuildLayout(LayoutGroup[] layoutGroups)
    {
        if (layoutGroups.IsNullOrEmpty())
            return;

        for (int i = 0; i < layoutGroups.Length; i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroups[i].transform as RectTransform);
        }
    }

    public static string GetStatusValueString(float floatNumber)
    {
        if (floatNumber % 1 == 0)
            return floatNumber.ToString("F0"); // �Ҽ��� ���� ���� ������ ǥ��
        else
            return floatNumber.ToString("F1"); // �Ҽ��� ù° �ڸ������� ǥ��
    }

    public static float GetAnimationFrameSecondWithSpeed(int startFrame, int endFrame, float frameRate, float playSpeed)
    {
        return (endFrame - startFrame) / frameRate / playSpeed;
    }

    public static float GetAnimationNormalizedTimeByFrame(int maxFrame, int targetFrame)
    {
        return targetFrame / (float)maxFrame;
    }

    public static float GetAnimationTimeByFrame(float length, int maxFrame, int targetFrame, float speed = 1f)
    {
        return GetAnimationNormalizedTimeByFrame(maxFrame, targetFrame) * length / speed;
    }

    #region Async Operation
    public static IEnumerator WaitAsyncOperation(System.Func<bool> isLoaded, UnityEngine.Events.UnityAction callback = null)
    {
        while (!isLoaded.Invoke())
        {
            yield return null;
        }

        callback?.Invoke();
    }
    #endregion
}
