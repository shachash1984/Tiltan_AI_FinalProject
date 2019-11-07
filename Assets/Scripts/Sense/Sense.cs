using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sense : MonoBehaviour
{
    public static event Action<Sense ,Enemy> EnemyDetected;
    public Enemy detectedEnemy { get; protected set; }
    public virtual void HandleEnemyDestroyed(Enemy e)
    {
        if (e == detectedEnemy)
            detectedEnemy = null;
    }

    public static void Detected(Sense s, Enemy e)
    {
        EnemyDetected?.Invoke(s, e);
    }

    private void OnEnable()
    {
        Enemy.EnemyDestroyed += HandleEnemyDestroyed;
    }

    private void OnDisable()
    {
        Enemy.EnemyDestroyed -= HandleEnemyDestroyed;
    }
}
