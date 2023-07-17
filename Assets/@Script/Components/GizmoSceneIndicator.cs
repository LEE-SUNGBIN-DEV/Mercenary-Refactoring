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
        white
    }

    [SerializeField] private GIZMO_COLOR color; 
    private void OnDrawGizmos()
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

            default:
                Gizmos.color = Color.green;
                break;
        }
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
        Gizmos.DrawRay(transform.position, new Vector3(0, 5, 0));
    }
}
