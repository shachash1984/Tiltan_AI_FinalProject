using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;
using UnityEngine;
using Pada1.BBCore.Framework;

[Condition("MyCondition/IsNightCondition")]
public class IsNightCondition : ConditionBase
{
    private DayNightCycle light;

    public override bool Check()
    {
        if (searchLight())
        {
            return light.isNight;
        }
        else
        {
            return false;
        }
    }

    public override TaskStatus MonitorCompleteWhenTrue()
    {
        if (Check())
        {
            // Light is off. It's night right now.
            return TaskStatus.COMPLETED;
        }
        else
        {
            if (light != null)
            {
                light.OnChanged += OnSunset;
            }
            return TaskStatus.SUSPENDED;
            // We will never awake if light does not exist.
        }
    }

    public override TaskStatus MonitorFailWhenFalse()
    {
        if (!Check())
        {
            // Light does not exist, or is "on" (daylight). Condition is false.
            return TaskStatus.FAILED;
        }
        else
        {
            // Light exists, and is "off" (night). We suspend ourselves
            // until sunrise (when the condition will become false).
            light.OnChanged += OnSunrise;
            return TaskStatus.SUSPENDED;
        }
    }

    public void OnSunset(object sender, System.EventArgs night)
    {
        light.OnChanged -= OnSunset;
        EndMonitorWithSuccess();
    }

    public void OnSunrise(object sender, System.EventArgs e)
    {
        light.OnChanged -= OnSunrise;
        EndMonitorWithFailure();
    }

    public override void OnAbort()
    {
        if (searchLight())
        {
            light.OnChanged -= OnSunrise;
            light.OnChanged -= OnSunset;
        }
        base.OnAbort();
    }

    // Search the global light, and stores in the light field. It returns true if the light was found.
    private bool searchLight()
    {
        if (light != null)
        {
            Debug.Log("is Done1");
            return true;
        }

        GameObject lightGO = GameObject.FindGameObjectWithTag("MainLight");
        if (lightGO == null)
        {
            Debug.Log("is Done2");
            return false;
        }

        light = lightGO.GetComponent<DayNightCycle>();
        Debug.Log("is Done3");
        return light != null;
    } // searchLight


}
