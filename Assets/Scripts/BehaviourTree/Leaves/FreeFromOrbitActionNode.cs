using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFromOrbitActionNode : ActionNode
{
    private Transform _self;
    private Transform _target;
    private float _minDeltaDistance;

    public FreeFromOrbitActionNode(Transform self, Transform target)
    {
        _self = self;
        _target = target;
        _minDeltaDistance = 5f;
        m_action = PerformManeuver;
    }

    private NodeState PerformManeuver()
    {
        m_nodeState = NodeState.FAILURE;
        _self.position += _self.forward * Time.deltaTime * 50;
        float dist = Vector3.Distance(_self.position, _target.position);
        if (dist >= _minDeltaDistance)
        {
            m_nodeState = NodeState.SUCCESS;
        }
        return m_nodeState;
    }
}
