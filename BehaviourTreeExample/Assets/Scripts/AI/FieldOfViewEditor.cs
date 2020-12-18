using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//https://www.youtube.com/watch?v=rQG9aUWarwE
[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
    FieldOfView Fow = (FieldOfView)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(Fow.transform.position, Vector3.up, Vector3.forward, 360, Fow.viewRadius);
        Vector3 viewangleA = Fow.DirFromAngle(-Fow.viewAngle / 2, false);
        Vector3 viewangleB = Fow.DirFromAngle(Fow.viewAngle / 2, false);

        Handles.DrawLine(Fow.transform.position, Fow.transform.position + viewangleA * Fow.viewRadius);
        Handles.DrawLine(Fow.transform.position, Fow.transform.position + viewangleB * Fow.viewRadius);
        Handles.color = Color.magenta;
        foreach (Transform visabletarget in Fow.validTargets)
        {
            Handles.DrawLine(Fow.transform.position, visabletarget.position);
            
        }
    }
}
