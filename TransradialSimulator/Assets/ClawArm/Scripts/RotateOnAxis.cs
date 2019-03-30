using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class RotateOnAxis : MonoBehaviour
{
    public float AngleBoundMin;
    public float AngleBoundMax;
    public float CurrentAngle;
    public Vector3 Axis;

    public void Update()
    {
        VerifyAngleValidity();
    }

    public void VerifyAngleValidity()
    {
        if(CurrentAngle<AngleBoundMin){
            transform.localRotation = Quaternion.AngleAxis(AngleBoundMin-CurrentAngle, Axis) * transform.localRotation;
            CurrentAngle += AngleBoundMin - CurrentAngle;
        }
        if (CurrentAngle > AngleBoundMax)
        {
            transform.localRotation = Quaternion.AngleAxis(-(AngleBoundMax - CurrentAngle), Axis) * transform.localRotation;
            CurrentAngle -= (AngleBoundMax - CurrentAngle);
        }
    }

    public void SetAngle(float angle)
    {
        if (angle < CurrentAngle && angle >= AngleBoundMin)
        {
            transform.localRotation = Quaternion.AngleAxis(-(CurrentAngle-angle), Axis) * transform.localRotation;
            CurrentAngle = angle;
        }
        else if (angle > CurrentAngle && angle <= AngleBoundMax)
        {
            transform.localRotation = Quaternion.AngleAxis(angle-CurrentAngle, Axis) * transform.localRotation;
            CurrentAngle = angle;
        }
        VerifyAngleValidity();
    }
}

[ExecuteInEditMode]
[CustomEditor(typeof(RotateOnAxis))]
public class ROAEditor:Editor{
    float turnSpeed = 0.5f; // DEGREE PER CLICK HELD
    float toAngle = 0f;
    public override void OnInspectorGUI()
    {
        GameObject g = ((RotateOnAxis)target).gameObject;
        ((RotateOnAxis)target).AngleBoundMin = EditorGUILayout.DelayedFloatField("Min Angle", ((RotateOnAxis)target).AngleBoundMin);
        ((RotateOnAxis)target).AngleBoundMax = EditorGUILayout.DelayedFloatField("Max Angle", ((RotateOnAxis)target).AngleBoundMax);
        EditorGUILayout.LabelField("Current Angle: " + ((RotateOnAxis)target).CurrentAngle);
        turnSpeed = EditorGUILayout.DelayedFloatField("Turn Speed", turnSpeed);
        g.GetComponent<RotateOnAxis>().Axis = EditorGUILayout.Vector3Field("Axis", g.GetComponent<RotateOnAxis>().Axis);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Turn Left"))
        {
            if (((RotateOnAxis)target).CurrentAngle - turnSpeed < ((RotateOnAxis)target).AngleBoundMin)
            {
                g.transform.localRotation = Quaternion.AngleAxis(((RotateOnAxis)target).AngleBoundMin-((RotateOnAxis)target).CurrentAngle, (((RotateOnAxis)target).Axis)) * g.transform.localRotation;
                ((RotateOnAxis)target).CurrentAngle = ((RotateOnAxis)target).AngleBoundMin;
            }
            else
            {
                g.transform.localRotation = Quaternion.AngleAxis(-turnSpeed, (((RotateOnAxis)target).Axis)) * g.transform.localRotation;
                ((RotateOnAxis)target).CurrentAngle -= turnSpeed;
            }
        }
        else if (GUILayout.Button("Turn Right")) {
            if (((RotateOnAxis)target).CurrentAngle + turnSpeed > ((RotateOnAxis)target).AngleBoundMax)
            {
                g.transform.localRotation = Quaternion.AngleAxis(((RotateOnAxis)target).AngleBoundMax - ((RotateOnAxis)target).CurrentAngle, (((RotateOnAxis)target).Axis)) * g.transform.localRotation;
                ((RotateOnAxis)target).CurrentAngle = ((RotateOnAxis)target).AngleBoundMax;
            }
            else
            {
                g.transform.localRotation = Quaternion.AngleAxis(turnSpeed, (((RotateOnAxis)target).Axis)) * g.transform.localRotation;
                ((RotateOnAxis)target).CurrentAngle += turnSpeed;
            }
            ((RotateOnAxis)target).VerifyAngleValidity();
        }
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Normalize Axis")){
            ((RotateOnAxis)target).Axis = ((RotateOnAxis)target).Axis.normalized;
        }

        toAngle = EditorGUILayout.DelayedFloatField("Angle to Set: ", toAngle);
        if (GUILayout.Button("Set Angle"))
        {
            g.GetComponent<RotateOnAxis>().SetAngle(toAngle);
        }
        else 
        {
            g.GetComponent<RotateOnAxis>().SetAngle(EditorGUILayout.Slider(((RotateOnAxis)target).CurrentAngle, ((RotateOnAxis)target).AngleBoundMin, ((RotateOnAxis)target).AngleBoundMax));
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty((RotateOnAxis)target);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}
