using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArmManager : MonoBehaviour
{
    public List<JointArrayController> JointArrays;
    public List<string> StateNames;
    public List<JointArrayState> PossibleStates;

    [System.NonSerialized]
    public int LastStateID;
    [System.NonSerialized]
    public int CurrentStateID;

    public JointArrayState ActiveState;

    [System.NonSerialized]
    public bool transitioning = false;

    [System.NonSerialized]
    [Range(0f,1f)]
    public float transitionPercent = 0f;


    public float transitionRate = 0.05f;
    public float EPS = 0.000001f;

    void Update()
    {
        if(transitioning && transitionPercent < 1f)
        {
            transitionPercent += transitionRate;
            transitionPercent = Mathf.Clamp(transitionPercent, 0f, 1f);
            ActiveState = JointArrayState.Interpolate(PossibleStates[LastStateID], PossibleStates[CurrentStateID], transitionPercent);
            if (1f-transitionPercent < EPS) { 
                transitioning = false;
                transitionPercent = 0f;
                LastStateID = CurrentStateID;
            }
        }

        for (int i = 0; i < JointArrays.Count; i++)
        {
            JointArrayController jac = JointArrays[i];
            if (jac == null || i>=ActiveState.Percents.Count)
                continue;
            JointArrays[i].ArrayPercent = ActiveState.Percents[i];
        }
    }
}
[System.Serializable]
public class JointArrayState
{
    [SerializeField]
    public List<float> Percents;
    public static JointArrayState Interpolate(JointArrayState a, JointArrayState b, float p)
    {
        if (a.Percents.Count != b.Percents.Count)
        {
            return new JointArrayState();
        }
        else
        {
            JointArrayState n = new JointArrayState
            {
                Percents = new List<float>()
            };
            int num = a.Percents.Count;
            for (int i = 0; i < num; i++)
            {
                n.Percents.Add(State.mapFloat(p, 0f, 1f, a.Percents[i], b.Percents[i]));
            }
            return n;
        }
    }

    public static float mapFloat(float x, float min1, float max1, float min2, float max2)
    {
        return ((x - min1) / (max1 - min1)) * (max2 - min2) + min2;
    }
}

[ExecuteInEditMode]
[CustomEditor(typeof(ArmManager))]
public class ArmManagerEditor : Editor
{
    public bool displayDefault = false;
    public override void OnInspectorGUI()
    {
        base.DrawHeader();
        ArmManager a = ((ArmManager)target);
        if (!Application.isPlaying)
        {
            if (GUILayout.Button("Toggle Default Inspector"))
                displayDefault = !displayDefault;
            if (displayDefault)
                base.DrawDefaultInspector();
            return;
        }
        if (!a.transitioning) {
            if (GUILayout.Button("Relax")&&a.CurrentStateID!=0)
            {
                a.CurrentStateID = 0;
                a.transitioning = true;
            }
            if (GUILayout.Button("Open") && a.CurrentStateID != 1)
            {
                a.CurrentStateID = 1;
                a.transitioning = true;
            }
            if (GUILayout.Button("Fist") && a.CurrentStateID != 2)
            {
                a.CurrentStateID = 2;
                a.transitioning = true;
            }
            if (GUILayout.Button("Point") && a.CurrentStateID != 3)
            {
                a.CurrentStateID = 3;
                a.transitioning = true;
            }
        }
        else
        {
            EditorGUILayout.LabelField("Transitioning... Please Wait...");
        }
        if (GUILayout.Button("Toggle Default Inspector"))
            displayDefault = !displayDefault;
        if(displayDefault)
            base.DrawDefaultInspector();
    }
}
