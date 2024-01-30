using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Functions
{
    public static string GetIntCommaString(int data)
    {
        return string.Format($"{data:#,###}");
    }
    public static string GetFloatCommaString(float data)
    {
        return string.Format($"{data:#,###.#}");
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

    public static float GetAngle(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 directionVector = endPoint - startPoint;
        return Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
    }
    public static float GetXZAxisDistance(Vector3 sourcePosition, Vector3 targetPosition)
    {
        sourcePosition.y = 0;
        targetPosition.y = 0;

        return Vector3.Distance(sourcePosition, targetPosition);
    }
    public static Vector3 GetXZAxisDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        Vector3 xzAxisDirection = targetPosition - sourcePosition;
        xzAxisDirection.y = 0;

        return xzAxisDirection.normalized;
    }
    public static Vector3 GetXZAxisDirection(Vector3 targetDirection)
    {
        Vector3 xzAxisDirection = targetDirection;
        xzAxisDirection.y = 0;

        return xzAxisDirection.normalized;
    }
}
