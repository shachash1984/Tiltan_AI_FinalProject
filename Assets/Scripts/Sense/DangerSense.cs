using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerSense : Sense
{
    public float range = 500;

    private void OnEnable()
    {
        Enemy.OpenFire += HandleShipDestroyed;
    }

    private void OnDisable()
    {
        Enemy.OpenFire -= HandleShipDestroyed;
    }

    private void HandleShipDestroyed(Enemy e)
    {
        if(Vector3.Distance(transform.position, e.transform.position) <= range)
        {
            Detected(this, e);
        }
    }
}
