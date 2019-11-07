using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachTargetActionNode : ActionNode
{
    Transform _self;
    Transform _target;
    float _reachTargetRange = 5f;

    public ReachTargetActionNode(Transform self, Transform target)
    {
        _self = self;
        _target = target;
        m_action = ReachDestination;
    }

    private NodeState ReachDestination()
    {
        float dist = Vector3.Distance(_self.position, _target.position);
        if (dist <= _reachTargetRange)
        {
            m_nodeState = NodeState.SUCCESS;
            return m_nodeState;
        }
        else
        {
            m_nodeState = NodeState.FAILURE;
            return m_nodeState;
        }
            
    }
}
