using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Functions
{
    public static Color SetColor(Color color, float alpha = 1f)
    {
        Color resultColor = color;
        resultColor.a = alpha;

        return resultColor;
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
