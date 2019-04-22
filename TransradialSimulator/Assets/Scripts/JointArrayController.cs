using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JointArrayController : MonoBehaviour
{
    public List<RotateOnAxis> Axes;
    public State StateA;
    public State StateB;
    public State CurrentState;

    [Range(0f,1f)]
    public float ArrayPercent = 0;

    void Update() {
        //if (Axes.Contains(null))
        //    Axes.Remove(null);
        if (StateA.Percents.Count == StateB.Percents.Count)
        {
            CurrentState = State.Interpolate(StateA, StateB, ArrayPercent);
        }
        else
        { 
            Debug.LogError("INCOMPATIBLE STATE SIZES!");
        }

        for (int i = 0; i<Axes.Count; i++)
        {
            RotateOnAxis roa = Axes[i];
            if (roa == null)
                continue;
            if (StateA.Percents.Count >= i && StateB.Percents.Count >= i)
            {
//                Debug.Log(roa);
                roa.SetAngle(State.mapFloat(CurrentState.Percents[i], 0f, 1f, roa.AngleBoundMin, roa.AngleBoundMax));
            }
        }
    }
}
[System.Serializable]
public class State
{
    [SerializeField]
    public List<float> Percents;
    public static State Interpolate (State a, State b, float p)
    {
        if (a.Percents.Count != b.Percents.Count) { 
            return new State();
        }
        else
        {
            State n = new State();
            n.Percents = new List<float>();
            int num = a.Percents.Count;
            for (int i = 0; i<num; i++) 
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