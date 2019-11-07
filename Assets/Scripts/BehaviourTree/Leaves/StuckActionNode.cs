using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckActionNode : ActionNode
{
    private Transform _self;
    private Transform _target;
    private float _maxDestinationTime;
    private float _destinationTimer;
    private float _distanceFromDestination;
    private float _prevDistanceFromDestination;
    private float _minDeltaDistance;

    public StuckActionNode(Transform self, Transform target)
    {
        _self = self;
        _target = target;
        _minDeltaDistance = 0.5f;
        _maxDestinationTime = 2f;
        m_action = CheckForOrbitLock;
    }

    private NodeState CheckForOrbitLock()
    {
        if (Time.time - _maxDestinationTime >= _destinationTimer)
        {
            //Debug.Log("Checking stuck");
            _destinationTimer = Time.time;
            _distanceFromDestination = Vector3.Distance(_self.position, _target.position);
            if (Mathf.Abs(_distanceFromDestination - _prevDistanceFromDestination) <= _minDeltaDistance)
            {
                m_nodeState = NodeState.SUCCESS;
            }
            else
            {
                m_nodeState = NodeState.FAILURE;
            }
            _prevDistanceFromDestination = _distanceFromDestination;
            
        }
        return m_nodeState;
    }
}
