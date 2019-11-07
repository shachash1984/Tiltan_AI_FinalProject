using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimActionNode : ActionNode
{
    Transform _self;
    LayerMask _layerMask;
    BehaviourTreeShip _npc;

    public AimActionNode(Transform self, LayerMask lm, BehaviourTreeShip npc)
    {
        _self = self;
        _layerMask = lm;
        _npc = npc;
        m_action = Aim;
    }

    private NodeState Aim()
    {
        if (_npc.currentEnemy == null)
        {
            m_nodeState = NodeState.FAILURE;
            return m_nodeState;
        }
            
        if (IsEnemyInSight())
            m_nodeState = NodeState.SUCCESS;
        else
        {
            TurnTowardsEnemy();
            m_nodeState = NodeState.RUNNING;
        }
        return m_nodeState;
    }

    private bool IsEnemyInSight()
    {
        Ray ray = new Ray(_self.position, _self.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, _layerMask))
        {
            if (hit.collider.gameObject.layer == NPC.ENEMY_LAYER)
                return true;
        }
        return false;
    }

    private void TurnTowardsEnemy()
    {
        _self.rotation = Quaternion.SlerpUnclamped(_self.rotation, Quaternion.LookRotation(_npc.currentEnemy.transform.position - _self.position), 0.1f);
    }
}
