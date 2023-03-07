using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Functions
{
    public static float GetAnimationNormalizedTimeByFrame(int maxFrame, int targetFrame)
    {
        return targetFrame / (float)maxFrame;
    }
    public static float GetAnimationTimeByFrame(float length, int maxFrame, int targetFrame, float speed = 1f)
    {
        return GetAnimationNormalizedTimeByFrame(maxFrame, targetFrame) * length / speed;
    }
    public static float GetAnimationTimeByFrame(AnimationInfo animationInfo, int targetFrame)
    {
        return GetAnimationNormalizedTimeByFrame(animationInfo.maxFrame, targetFrame) * animationInfo.animationLength / animationInfo.animationSpeed;
    }

    public static Vector3 GetRandomRectCoordinate(float halfWidth, float halfDepth)
    {
        float pointX = Random.Range(-halfWidth, halfWidth);
        float pointZ = Random.Range(-halfDepth, halfDepth);

        return new Vector3(pointX, 0, pointZ);
    }

    public static Vector3 GetRandomCubeCoordinate(float halfWidth, float halfDepth, float halfHeight)
    {
        Vector3 randomCoordinate = GetRandomRectCoordinate(halfWidth, halfDepth);
        randomCoordinate.y = Random.Range(-halfHeight, halfHeight);

        return randomCoordinate;
    }

    public static Vector3 GetRandomCircleCoordinate(float radius)
    {
        float pointX = Random.Range(-radius, radius);
        float pointZRange = Mathf.Sqrt(radius * radius - pointX * pointX);
        float pointZ = Random.Range(-pointZRange, pointZRange);

        return new Vector3(pointX, 0, pointZ);
    }

    public static Vector3 GetRandomSphereCoordinate(float radius)
    {
        float pointX = Random.Range(-radius, radius);
        float pointZRange = Mathf.Sqrt(radius * radius - pointX * pointX);
        float pointZ = Random.Range(-pointZRange, pointZRange);
        float pointYRange = Mathf.Sqrt(radius * radius - Mathf.Sqrt(pointX * pointX + pointZ * pointZ));
        float pointY = Random.Range(-pointYRange, pointYRange);

        return new Vector3(pointX, pointY, pointZ);
    }

    public static float GetAngle(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 directionVector = endPoint - startPoint;
        return Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
    }

}
