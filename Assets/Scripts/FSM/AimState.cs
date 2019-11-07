using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimState : State
{
    FSMShip _fsmNpc;
    public static event Action EnemyInSight;

    public override void Action()
    {
        Aim();
    }

    public override void OnEnterState(NPC npc)
    {
        base.OnEnterState(npc);
        _fsmNpc = npc as FSMShip;
    }

    private void Aim()
    {
        if (IsEnemyInSight())
            EnemyInSight?.Invoke();
        else
            TurnTowardsEnemy();
    }

    private bool IsEnemyInSight()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, _npc._layerMask))
        {
            if (hit.collider.gameObject.layer == NPC.ENEMY_LAYER)
                return true;
        }
        return false;
    }

    private void TurnTowardsEnemy()
    {
        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.LookRotation(_fsmNpc.currentEnemy.transform.position - transform.position), 0.1f);
    }
}
