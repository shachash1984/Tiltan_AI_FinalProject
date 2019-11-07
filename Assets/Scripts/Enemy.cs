using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    public static event Action<Enemy> EnemyDestroyed;
    public static event Action<Enemy> OpenFire;
    private int life = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == PROJECTILE_LAYER)
        {
            Destroy(other.gameObject);
            life--;
            if (life <= 0)
            {
                EnemyDestroyed?.Invoke(this);
                Destroy(gameObject);
            }
                
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OpenFire?.Invoke(this);
        }
    }
    
}
