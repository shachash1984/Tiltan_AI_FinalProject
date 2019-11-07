using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SonarSense : Sense
{
    private void OnTriggerEnter(Collider other)
    {
        if (!detectedEnemy && other.gameObject.layer == NPC.ENEMY_LAYER)
            Detected(this, other.GetComponent<Enemy>());
    }
}
