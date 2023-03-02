using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PathMovement),true)]
[InitializeOnLoad]
public class PathMovementEditor : Editor
{
    
    public PathMovement pathMovementTarget
    {
        get
        {
            return target as PathMovement;
        }
    }



    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        if (pathMovementTarget.AcclerationType == PathMovement.PossibleAccelerationType.AnimationCurve)
        {
            DrawDefaultInspector();
        }
        else
        {
            DrawPropertiesExcluding(serializedObject, new string[] { "Accleration" });
        }

        serializedObject.ApplyModifiedProperties();
    }


    private void OnSceneGUI()
    {
        Handles.color = Color.green;
        PathMovement t = pathMovementTarget;
        if (!t.GetOriginalTransformPotionStatus())
        {
            return;
        }

        for (int i = 0; i < t.PathElements.Count; i++)
        {
            EditorGUI.BeginChangeCheck();

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.cyan;
            Handles.Label(t.GetOriginalTransformPostion() + t.PathElements[i].PathElementPotion + new Vector3(-0.25f, -0.25f),
                          "" + i);
            Vector3 oldPostion = t.GetOriginalTransformPostion() + t.PathElements[i].PathElementPotion;
            var fmh_58_69_638059460331603295 = Quaternion.identity; Vector3 newPostion = Handles.FreeMoveHandle(oldPostion, 1.5f, new Vector3(0.25f, 0.25f), Handles.CircleHandleCap);

            if (EditorGUI.EndChangeCheck())
            {
                 t.PathElements[i].PathElementPotion = newPostion - t.GetOriginalTransformPostion();
            }
        }

    }


}
