using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    public const int FRIENDLY_LAYER = 8;
    public const int ENEMY_LAYER = 9;
    public const int PROJECTILE_LAYER = 10;
    public const int TARGET_LAYER = 11;
    public const int ENERGY_LAYER = 13;

    public LayerMask _layerMask;

    private void Update()
    {
        Renderer ren = GetComponent<Renderer>();
        if (ren)
        {

        }
    }

}
