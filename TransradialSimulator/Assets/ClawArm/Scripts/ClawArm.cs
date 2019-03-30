using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ClawArm : MonoBehaviour {
    public GameObject Max;
    public GameObject Min;
    public Vector3 Current;

    public void SetPosition(float percent){
        Vector3 min, max;
        min = Min.transform.position;
        max = Max.transform.position;
        Current = percent * (max - min) + min;
    }

    public void FixedUpdate()
    {
        transform.position = Current;
    }
}
