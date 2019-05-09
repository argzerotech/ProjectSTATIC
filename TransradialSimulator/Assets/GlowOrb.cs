using UnityEngine;

public class GlowOrb : MonoBehaviour
{
    public bool On = true;
    void OnDrawGizmos()
    {
        if (!On)
            return;
        // Draw a yellow sphere at the transform's position
        Color c = GetComponent<Light>().color;
        c.a = 0.5f;
        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position, 0.025f);
    }
}
