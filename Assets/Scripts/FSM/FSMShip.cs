using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMShip : NPC
{
    public State state { get; protected set; }
    public Enemy currentEnemy { get; protected set; }
    

    public void SetState(State newState)
    {
        if (state != null)
        {
            state.OnExitState();
            Destroy(state);
        }

        state = newState;
        newState.OnEnterState(this);
    }

    private void Start()
    {
        SetState(gameObject.AddComponent<MoveState>());
    }

    void Update()
    {
        state.Action();
    }

    private void OnEnable()
    {
        MoveState.LockedInOrbit += HandleLockedInOrbit;
        FreeFromOrbitState.FinishedManeuver += HandleFinishedManeuver;
        AimState.EnemyInSight += HandleEnemyInSight;
        Enemy.EnemyDestroyed += HandleEnemyDestroyed;
    }

    private void OnDisable()
    {
        MoveState.LockedInOrbit -= HandleLockedInOrbit;
        FreeFromOrbitState.FinishedManeuver -= HandleFinishedManeuver;
        AimState.EnemyInSight -= HandleEnemyInSight;
        Enemy.EnemyDestroyed -= HandleEnemyDestroyed;
    }

    private void HandleEnemyDestroyed(Enemy obj)
    {
        if (obj == currentEnemy)
        {
            SetState(gameObject.AddComponent<MoveState>());
            currentEnemy = null;
        }
    }

    private void HandleEnemyInSight()
    {
        SetState(gameObject.AddComponent<ShootState>());
    }

    private void HandleLockedInOrbit()
    {
        SetState(gameObject.AddComponent<FreeFromOrbitState>());
    }

    private void HandleFinishedManeuver()
    {
        SetState(gameObject.AddComponent<MoveState>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == ENEMY_LAYER)
        {
            currentEnemy = other.GetComponent<Enemy>();
            SetState(gameObject.AddComponent<AimState>());
            
        }
    }

}
