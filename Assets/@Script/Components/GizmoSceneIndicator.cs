using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GizmoSceneIndicator : MonoBehaviour
{
    public enum GIZMO_COLOR
    {
        black,
        blue,
        gray,
        green,
        red,
        white,
        yellow,
    }

    public enum GIZMO_SHAPE
    {
        Cube,
        Sphere,
    }

    [SerializeField] private GIZMO_COLOR color;
    [SerializeField] private GIZMO_SHAPE shape;
    [SerializeField] private float size = 1f;
    [SerializeField] private bool isDrawLine = true;
    [SerializeField] private float lineLength = 5f;

    public void SetColor()
    {
        switch (color)
        {
            case GIZMO_COLOR.black:
                Gizmos.color = Color.black;
                break;
            case GIZMO_COLOR.blue:
                Gizmos.color = Color.blue;
                break;
            case GIZMO_COLOR.gray:
                Gizmos.color = Color.gray;
                break;
            case GIZMO_COLOR.green:
                Gizmos.color = Color.green;
                break;
            case GIZMO_COLOR.red:
                Gizmos.color = Color.red;
                break;
            case GIZMO_COLOR.white:
                Gizmos.color = Color.white;
                break;
            case GIZMO_COLOR.yellow:
                Gizmos.color = Color.yellow;
                break;

            default:
                Gizmos.color = Color.white;
                break;
        }
    }

    public void DrawShape()
    {
        switch(shape)
        {
            case GIZMO_SHAPE.Cube:
                Gizmos.DrawCube(transform.position, new Vector3(size, size, size));
                break;

            case GIZMO_SHAPE.Sphere:
                Gizmos.DrawSphere(transform.position, size);
                break;

            default:
                Gizmos.DrawSphere(transform.position, size);
                break;
        }
    }

    public void DrawLine()
    {
        if(isDrawLine == true)
            Gizmos.DrawRay(transform.position, new Vector3(0, lineLength, 0));
    }

    private void OnDrawGizmos()
    {
        SetColor();
        DrawShape();
        DrawLine();
    }
}
