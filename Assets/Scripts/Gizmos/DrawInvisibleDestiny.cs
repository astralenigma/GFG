using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DrawInvisibleDestiny : MonoBehaviour
{
    [SerializeField]
    Color gizmoColor = Color.blue;
    [SerializeField]
    Vector3 gizmoOffset = Vector3.zero;
    [SerializeField]
    Vector3 gizmoSize = Vector3.one;
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position+gizmoOffset, gizmoSize);
    }
}
