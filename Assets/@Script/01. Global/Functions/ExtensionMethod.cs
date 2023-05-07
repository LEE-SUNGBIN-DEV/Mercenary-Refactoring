using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public static class ExtensionMethod
{
    #region GameObject
    public static void ToggleActive(this GameObject gameObject)
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        else
        {
            gameObject.SetActive(true);
        }
    }
    public static void SetTransform(this GameObject gameObject, Transform targetTransform)
    {
        gameObject.transform.position = targetTransform.position;
        gameObject.transform.rotation = targetTransform.rotation;
    }
    public static void SetTransform(this GameObject gameObject, GameObject targetObject)
    {
        gameObject.SetTransform(targetObject.transform);
    }
    #endregion

    #region Transform
    public static Transform GetRootTransform(this Transform currentTransform)
    {
        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
        }

        return currentTransform;
    }
    #endregion

    #region Animator
    public static bool IsAnimationFrameUpTo(this Animator animator, int nameHash, int maxFrame, int targetFrame, int targetLayer = 0)
    {
        return (animator.GetCurrentAnimatorStateInfo(targetLayer).shortNameHash == nameHash
            && animator.GetCurrentAnimatorStateInfo(targetLayer).normalizedTime >= Functions.GetAnimationNormalizedTimeByFrame(maxFrame, targetFrame));
    }
    public static bool IsAnimationFrameUpTo(this Animator animator, AnimationClipInformation animationInfo, int targetFrame, int targetLayer = 0)
    {
        return IsAnimationFrameUpTo(animator, animationInfo.nameHash, animationInfo.maxFrame, targetFrame, targetLayer);
    }
    public static bool IsAnimationFrameDownTo(this Animator animator, int nameHash, int maxFrame, int targetFrame, int targetLayer = 0)
    {
        return (animator.GetCurrentAnimatorStateInfo(targetLayer).shortNameHash == nameHash
            && animator.GetCurrentAnimatorStateInfo(targetLayer).normalizedTime <= Functions.GetAnimationNormalizedTimeByFrame(maxFrame, targetFrame));
    }
    public static bool IsAnimationFrameDownTo(this Animator animator, AnimationClipInformation animationInfo, int targetFrame, int targetLayer = 0)
    {
        return IsAnimationFrameDownTo(animator, animationInfo.nameHash, animationInfo.maxFrame, targetFrame, targetLayer);
    }
    public static bool IsAnimationFrameBetweenTo(this Animator animator, int nameHash, int maxFrame, int startFrame, int endFrame, int targetLayer = 0)
    {
        return (animator.GetCurrentAnimatorStateInfo(targetLayer).shortNameHash == nameHash
            && animator.GetCurrentAnimatorStateInfo(targetLayer).normalizedTime >= Functions.GetAnimationNormalizedTimeByFrame(maxFrame, startFrame)
            && animator.GetCurrentAnimatorStateInfo(targetLayer).normalizedTime <= Functions.GetAnimationNormalizedTimeByFrame(maxFrame, endFrame));
    }
    public static bool IsAnimationFrameBetweenTo(this Animator animator, AnimationClipInformation animationInfo, int startFrame, int endFrame, int targetLayer = 0)
    {
        return IsAnimationFrameBetweenTo(animator, animationInfo.nameHash, animationInfo.maxFrame, startFrame, endFrame, targetLayer);
    }

    public static bool IsAnimationNormalizeTimeUpTo(this Animator animator, int nameHash, float normalizedTime, int targetLayer = 0)
    {
        return (animator.GetCurrentAnimatorStateInfo(targetLayer).shortNameHash == nameHash
            && animator.GetCurrentAnimatorStateInfo(targetLayer).normalizedTime >= normalizedTime);
    }
    public static bool IsAnimationNormalizeTimeDownTo(this Animator animator, int nameHash, float normalizedTime, int targetLayer = 0)
    {
        return (animator.GetCurrentAnimatorStateInfo(targetLayer).shortNameHash == nameHash
            && animator.GetCurrentAnimatorStateInfo(targetLayer).normalizedTime <= normalizedTime);
    }
    public static bool IsAnimationNormalizeTimeBetweenTo(this Animator animator, int nameHash, float minNormalizedTime, float maxNormalizedTime, int targetLayer = 0)
    {
        return (animator.GetCurrentAnimatorStateInfo(targetLayer).shortNameHash == nameHash
            && animator.GetCurrentAnimatorStateInfo(targetLayer).normalizedTime >= minNormalizedTime
            && animator.GetCurrentAnimatorStateInfo(targetLayer).normalizedTime <= maxNormalizedTime);
    }
    #endregion

    public static string GetEnumName<T>(this T targetEnum) where T : System.Enum
    {
        string enumName = System.Enum.GetName(typeof(T), targetEnum);

        return enumName;
    }

    public static bool IsBetween(this float value, float min, float max)
    {
        if (value > min && value < max)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
