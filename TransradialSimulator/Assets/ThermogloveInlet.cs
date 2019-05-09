using UnityEngine;
using Assets.LSL4Unity.Scripts.AbstractInlets;
using Assets.LSL4Unity.Scripts;
using System.Collections.Generic;
public class ThermogloveInlet : InletIntSamples
{
    public List<Light> GlowLights;
    private bool pullSamplesContinuously = false;
    void Start()
    {
        // [optional] call this only if your gameobject hosting this component
        // got instantiated during runtime
        // 'AZThermoglove', 'ThermistorGloveData'
        // registerAndLookUpStream();
    }

    protected override bool isTheExpected(LSLStreamInfoWrapper stream)
    {
        // the base implementation just checks for stream name and type
        var predicate = base.isTheExpected(stream);
        // add a more specific description for your stream here specifying hostname etc.
        //predicate &= stream.HostName.Equals("Expected Hostname");
        return predicate;
    }

    /// <summary>
    /// Override this method to implement whatever should happen with the samples...
    /// IMPORTANT: Avoid heavy processing logic within this method, update a state and use
    /// coroutines for more complexe processing tasks to distribute processing time over
    /// several frames
    /// </summary>
    /// <param name="newSample"></param>
    /// <param name="timeStamp"></param>
    protected override void OnStreamAvailable()
    {
        pullSamplesContinuously = true;
    }

    protected override void OnStreamLost()
    {
        pullSamplesContinuously = false;
    }

    private void Update()
    {
        if (pullSamplesContinuously)
            pullSamples();
    }

    protected override void Process(int[] newSample, double timeStamp)
    {
        if (newSample.Length == 0)
            return;

        Debug.Log(newSample.Length);
        for (int i = 0; i< newSample.Length; i++)
        {
            float pct = newSample[i] / 1023.0f;
            GlowLights[i].intensity = 3.0f * pct;// Maximum is 3.0f; Minimum is 0.0f;
            GlowLights[i].color = Color.Lerp(Color.cyan, Color.red, pct);
        }

        Debug.Log("Input Thermoglove: " + newSample.ToString());
    }
}
