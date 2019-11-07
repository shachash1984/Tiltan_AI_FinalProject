using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolActionNode : ActionNode
{
    private Transform _self;
    private Transform _target;
    private BehaviourTreeShip _npc;
    private float _rotationDamp = 0.02f;
    private float _movementSpeed = 20f;

    public PatrolActionNode(Transform self, Transform target, BehaviourTreeShip npc)
    {
        _self = self;
        _target = target;
        _npc = npc;
        m_action = Patrol;
    }

    private NodeState Patrol()
    {
        if (_npc.currentEnemy)
        {
            m_nodeState = NodeState.RUNNING;
            return m_nodeState;
        }
            

        TurnTowardsDestination();
        Move();
        m_nodeState = NodeState.SUCCESS;
        return m_nodeState;
    }

    

    private void Move()
    {
        _self.position += _self.forward * Time.deltaTime * _movementSpeed;
    }

    private void TurnTowardsDestination()
    {
        _self.rotation = Quaternion.SlerpUnclamped(_self.rotation, Quaternion.LookRotation(_target.position - _self.position), _rotationDamp);

    }

   
}
