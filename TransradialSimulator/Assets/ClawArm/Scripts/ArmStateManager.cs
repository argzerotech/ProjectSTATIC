using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmStateManager : MonoBehaviour {
    public enum CTRL_MODE{
        ANGLE,
        PWM
    }

    public List<InputField> PWMs;
    public List<InputField> Angles;
    public List<RotateOnAxis> Joints;
    public ClawController ClawCtrl;
   
    public List<float> PWMToAngleSlopes;
    public List<float> PWMToAngleOffsets;
    public List<float> AngleToPWMSlopes;
    public List<float> AngleToPWMOffsets;

    public CTRL_MODE ctrlMode = CTRL_MODE.PWM;

    public void SetCTRLMode(bool mode){
        ctrlMode = (mode) ? (CTRL_MODE.ANGLE): (CTRL_MODE.PWM);
    }

	// Update is called once per frame
	void Update () {
        switch(ctrlMode){
            case CTRL_MODE.ANGLE:
                //Debug.Log("ANGLEMODE");
                AngleUpdate();
                foreach (InputField i in PWMs)
                {
                    i.interactable = false;
                }
                foreach (InputField j in Angles)
                {
                    j.interactable = true;
                }
                break;
            case CTRL_MODE.PWM:
                //Debug.Log("PWMMODE");
                PWMUpdate();
                foreach (InputField i in PWMs)
                {
                    i.interactable = true;

                }
                foreach (InputField j in Angles)
                {
                    j.interactable = false;
                }
                break;
        }
	}

    public float Map(float x, float a1, float b1, float a2, float b2){
        return (((x - a1) / (b1 - a1)) * (b2 - a2)) + a2;
    }

    public void PWMUpdate()
    {
        //Debug.Log("PWM Update");
        for (int i = 0; i < PWMs.Count; i++)
        {
            int pwm;
            bool failure = int.TryParse(PWMs[i].text, out pwm);
            if (i < PWMs.Count - 1)
            {
                float angle = PWMToAngleSlopes[i] * pwm + PWMToAngleOffsets[i];
                Angles[i].text = "" + angle;
                Joints[i].SetAngle(angle);
            }
            else
            {
                float percent = Map(pwm, 1100, 1950, 0f, 1f);
                ClawCtrl.SetPercent(percent);
                Angles[PWMs.Count-1].text = "" + (percent*100);
            }
        }
    }

    public void AngleUpdate()
    {
        //Debug.Log("Angle Update");
        List<float> angles = new List<float>();
        for (int i = 0; i < Angles.Count; i++)
        {
            int angleOrPercent;
            bool failure = int.TryParse(Angles[i].text, out angleOrPercent);
            if (i < Angles.Count - 1)
            {
                float pwm = AngleToPWMSlopes[i] * angleOrPercent + AngleToPWMOffsets[i];
                PWMs[i].text = "" + pwm;
                Joints[i].SetAngle(angleOrPercent);
            }
            else
            {
                float pwm = Map(angleOrPercent, 0f, 100f,1100, 1950);
                ClawCtrl.SetPercent(angleOrPercent); // Really its a percent but, eh, close enough
                PWMs[Angles.Count - 1].text = "" + pwm;
            }
        }
    }

    public int AngleToPWM(int angle, int joint)
    {
        if (joint != 5)
        {
            float off = AngleToPWMOffsets[joint];
            float minAng = Joints[joint].AngleBoundMin;
            float maxAng = Joints[joint].AngleBoundMax;
            return (int)Map(angle, minAng, 
                              maxAng,
                              AngleToPWMSlopes[joint] * minAng + off, 
                              AngleToPWMSlopes[joint] * maxAng + off);
        }
        return (int)Map(angle, 0f, 100f, 1100, 1950);
    }
    public int PWMToAngle(int pwm, int joint)
    {
        if (joint != 5)
        {
            float off = AngleToPWMOffsets[joint];
            float minAng = Joints[joint].AngleBoundMin;
            float maxAng = Joints[joint].AngleBoundMax;
            return (int)Map(pwm, AngleToPWMSlopes[joint] * minAng + off,
                                 AngleToPWMSlopes[joint] * maxAng + off,
                                 minAng,
                                 maxAng);
        }
        return (int)Map(pwm, 1100, 1950, 0f, 100f);
    }
}
