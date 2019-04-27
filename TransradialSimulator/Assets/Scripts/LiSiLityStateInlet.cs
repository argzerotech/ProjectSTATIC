using UnityEngine;
using Assets.LSL4Unity.Scripts.AbstractInlets;
using Assets.LSL4Unity.Scripts;

public class LiSiLityStateInlet : InletIntSamples
{ 
    public ArmManager Arm;
    private bool pullSamplesContinuously = false;
    void Start()
    {
        // [optional] call this only if your gameobject hosting this component
        // got instantiated during runtime

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
        int sum = 0;
        foreach (int i in newSample)
            sum += i;
        sum = Mathf.RoundToInt(((float)sum) / newSample.Length);
        int state = sum + 1;
        if (!Arm.transitioning)
        {
            if (state == 1)
            {
                Arm.CurrentStateID = 0;
                Arm.transitioning = true;
            }
            if (state == 2)
            {
                Arm.CurrentStateID = 1;
                Arm.transitioning = true;
            }
            if (state == 3)
            {
                Arm.CurrentStateID = 2;
                Arm.transitioning = true;
            }
            if (state == 4)
            {
                Arm.CurrentStateID = 3;
                Arm.transitioning = true;
            }
            Debug.Log("State Changed to: " + state);
        }
    }
}