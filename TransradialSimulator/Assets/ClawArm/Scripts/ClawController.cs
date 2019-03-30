using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ClawController : MonoBehaviour {
    public List<ClawArm> Claws;
    [Range(0f,1f)]
    [System.NonSerialized]
    public float CurrentPercent=0f;

    public void FixedUpdate()
    {
        foreach (ClawArm cArm in Claws)
            cArm.SetPosition(CurrentPercent);
    }

    public void SetPercent(float pos){
        CurrentPercent = pos;
        foreach (ClawArm cArm in Claws)
            cArm.SetPosition(CurrentPercent);
    }
}

[ExecuteInEditMode]
[CustomEditor(typeof(ClawController))]
public class ClawControllerEditor : Editor
{
    float moveSpeed = 0.05f; // DEGREE PER CLICK HELD
    float toPos = 0f;
    public override void OnInspectorGUI()
    {
        GameObject g = ((ClawController)target).gameObject;
        EditorGUILayout.LabelField("Current Percent: " + ((ClawController)target).CurrentPercent);
        moveSpeed = EditorGUILayout.DelayedFloatField("Turn Speed", moveSpeed);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Move Out"))
        {
            ((ClawController)target).CurrentPercent -= moveSpeed;
            ((ClawController)target).CurrentPercent = Mathf.Clamp(((ClawController)target).CurrentPercent, 0f, 1f);
        }
        else if (GUILayout.Button("Move In"))
        {
            ((ClawController)target).CurrentPercent += moveSpeed;
            ((ClawController)target).CurrentPercent = Mathf.Clamp(((ClawController)target).CurrentPercent, 0f, 1f);
        }
        else 
            ((ClawController)target).CurrentPercent=EditorGUILayout.Slider(((ClawController)target).CurrentPercent, 0f, 1f);
        GUILayout.EndHorizontal();

        toPos = EditorGUILayout.DelayedFloatField("Percent to Set: ", toPos);
        toPos = Mathf.Clamp(toPos, 0f, 1f);
        if (GUILayout.Button("Set Percent"))
        {
            g.GetComponent<ClawController>().SetPercent(toPos);
        }
        base.OnInspectorGUI();
    }
}