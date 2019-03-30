using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ObjectScatterer : MonoBehaviour
{
    public float MinRadius;
    public float MaxRadius;
    public Vector3 Origin;
    public Transform GiveToThis;

    public void Update()
    {
        gameObject.transform.position = Origin;
        List<Transform> Children = new List<Transform>();
        Transform[] ts = GetChildrenOfObject(gameObject);
        Children.AddRange(ts);

        foreach (Transform t in Children)
        {
            float vectorMag;
            vectorMag = Random.Range(MinRadius, MaxRadius);
            float angle = Random.Range(0f, 360f);
            Vector3 offset = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right;
            offset = vectorMag * offset.normalized;
            Vector3 currentPos = transform.position;
            t.position = currentPos + offset;
            t.parent = GiveToThis;
        }
        gameObject.transform.position = Origin;
    }

    public Transform[] GetChildrenOfObject(GameObject g)
    {
        Transform t = g.transform;
        Transform[] _transforms;
        _transforms = t.GetComponentsInChildren<Transform>();
        return _transforms;
    }
}
[CustomEditor(typeof(ObjectScatterer))]
public class ObjectScattererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ((ObjectScatterer)target).gameObject.transform.position = ((ObjectScatterer)target).Origin;
        List<Transform> Children = new List<Transform>();
        Transform[] ts = GetChildrenOfObject(((ObjectScatterer)target).gameObject);
        Children.AddRange(ts);

        foreach(Transform t in Children){
            float vectorMag;
            vectorMag = Random.Range(((ObjectScatterer)target).MinRadius, ((ObjectScatterer)target).MaxRadius);

            float angle = Random.Range(0f, 360f);
            Vector3 offset = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right;
            offset = vectorMag*offset.normalized;
            Vector3 currentPos = ((ObjectScatterer)target).transform.position;
            t.position = currentPos + offset;
            t.parent = ((ObjectScatterer)target).GiveToThis;
        }
        ((ObjectScatterer)target).gameObject.transform.position = ((ObjectScatterer)target).Origin;
    }
    public Transform[] GetChildrenOfObject(GameObject g)
    {
        Transform t = g.transform;
        Transform[] _transforms;
        _transforms = t.GetComponentsInChildren<Transform>();
        return _transforms;
    }
}

