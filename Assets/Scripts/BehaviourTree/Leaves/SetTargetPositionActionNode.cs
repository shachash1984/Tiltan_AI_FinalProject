using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetPositionActionNode : ActionNode
{
    private Transform _target;
    private Bounds _bounds;

    public SetTargetPositionActionNode(Transform target, Bounds bnd)
    {
        _target = target;
        _bounds = bnd;
        m_action = SetTargetPosition;
    }

    public NodeState SetTargetPosition()
    {
        _target.position = GetRandomPointInBounds();
        m_nodeState = NodeState.SUCCESS; ;
        return m_nodeState;
    }

    private Vector3 GetRandomPointInBounds()
    {
        Vector3 newPos = _bounds.center;
        newPos.x = Random.Range(_bounds.min.x, _bounds.max.x);
        newPos.y = Random.Range(_bounds.min.y, _bounds.max.y);
        newPos.z = Random.Range(_bounds.min.z, _bounds.max.z);

        return newPos;
    }
}
