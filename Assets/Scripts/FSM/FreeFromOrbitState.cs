using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFromOrbitState : State
{
    public static event Action FinishedManeuver;
    private float _startTime;
    private float _maneuverTime = 3f;

    public override void OnEnterState(NPC npc)
    {
        base.OnEnterState(npc);
        _startTime = Time.time;
    }

    public override void Action()
    {
        PerformManeuver();
        if(Time.time - _startTime >= _maneuverTime)
        {
            FinishedManeuver?.Invoke();
            _startTime = Time.time;
        }
    }

    private void PerformManeuver()
    {
        //transform.Rotate(transform.forward, transform.rotation.eulerAngles.z + Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * 20;
    }
}
