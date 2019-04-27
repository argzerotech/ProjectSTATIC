using LSL;
using System.Collections.Generic;
using Assets.LSL4Unity.Scripts.Common;
using UnityEngine;

[RequireComponent(typeof(LiSiLityStateInlet))]
public class TransradialController : MonoBehaviour
{
    public ArmManager ArmMgr;
    public LiSiLityStateInlet LSI;
    private const string unique_source_id_suffix = "Transradial";

    private string unique_source_id;

    private liblsl.StreamOutlet outlet;
    private liblsl.StreamInfo streamInfo;

    private int channelCount = 0;

    /// <summary>
    /// Use a array to reduce allocation costs
    /// and reuse it for each sampling call
    /// </summary>
    private float[] currentSample;

    //public Transform sampleSource;

    public string StreamName = "LiSiLity.<Add_a_entity_id_here>";
    public string StreamType = "LiSiLity.Angles";

    public bool StreamFingerAngles = true;
    public bool StreamIndividualAngles = false;
    /// <summary>
    /// Due to an instable framerate we assume a irregular data rate.
    /// </summary>
    private const double dataRate = liblsl.IRREGULAR_RATE;

    void Awake()
    {
        // assigning a unique source id as a combination of a the instance ID for the case that
        // multiple LSLTransformOutlet are used and a guid identifing the script itself.
        unique_source_id = string.Format("{0}_{1}", GetInstanceID(), unique_source_id_suffix);
    }

    void Start()
    {
        var channelDefinitions = SetupChannels();

        channelCount = channelDefinitions.Count;

        // initialize the array once
        currentSample = new float[channelCount];

        streamInfo = new liblsl.StreamInfo(StreamName, StreamType, channelCount, dataRate, liblsl.channel_format_t.cf_float32, unique_source_id);

        // it's not possible to create a XMLElement before and append it.
        liblsl.XMLElement chns = streamInfo.desc().append_child("channels");
        // so this workaround has been introduced.
        foreach (var def in channelDefinitions)
        {
            chns.append_child("channel")
                .append_child_value("label", def.label)
                .append_child_value("unit", def.unit)
                .append_child_value("type", def.type);
        }

        outlet = new liblsl.StreamOutlet(streamInfo);
    }

    /// <summary>
    /// Sampling on Late Update to make sure the transform recieved all updates
    /// </summary>
    void LateUpdate()
    {
        if (outlet == null)
            return;

        sample();
    }

    private void sample()
    {
        int offset = -1;

        if (StreamFingerAngles)
        {
            List<float> FingerAngles = ArmMgr.ActiveState.Percents;
            currentSample[++offset] = FingerAngles[0];
            currentSample[++offset] = FingerAngles[1];
            currentSample[++offset] = FingerAngles[2];
            currentSample[++offset] = FingerAngles[3];
            currentSample[++offset] = FingerAngles[4];
        }
        if (StreamIndividualAngles)
        {
            //var rotation = sampleSource.rotation.eulerAngles;

            //currentSample[++offset] = rotation.x;
            //currentSample[++offset] = rotation.y;
            //currentSample[++offset] = rotation.z;
        }
        outlet.push_sample(currentSample, liblsl.local_clock());
    }

    #region workaround for channel creation
    private ICollection<ChannelDefinition> SetupChannels()
    {
        var list = new List<ChannelDefinition>();

        if (StreamFingerAngles)
        {
            string[] angleLabels = {"1st Phalange or Thumb Angle",
                                    "2nd Phalange Angle",
                                    "3rd Phalange Angle",
                                    "4th Phalange Angle",
                                    "5th Phalange Angle"};

            foreach (var item in angleLabels)
            {
                var definition = new ChannelDefinition();
                definition.label = item;
                definition.unit = "percentage of rotation position";
                definition.type = "percentage";
                list.Add(definition);
            }
        }

        if (StreamIndividualAngles)
        {
            //string[] eulerLabels = { "x", "y", "z" };

            //foreach (var item in eulerLabels)
            //{
            //    var definition = new ChannelDefinition();
            //    definition.label = item;
            //    definition.unit = "degree";
            //    definition.type = "axis angle";
            //    list.Add(definition);
            //}
        }

        return list;
    }
    #endregion
}