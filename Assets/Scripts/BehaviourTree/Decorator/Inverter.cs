using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : BehaviourNode
{
    private BehaviourNode m_node;

    public BehaviourNode node { get => m_node; }

    public Inverter(BehaviourNode node)
    {
        m_node = node;
    }

    public override NodeState Evaluate()
    {
        switch (m_node.Evaluate())
        {
            case NodeState.FAILURE:
                m_nodeState = NodeState.SUCCESS;
                return m_nodeState;
            case NodeState.SUCCESS:
                m_nodeState = NodeState.FAILURE;
                return m_nodeState;
            case NodeState.RUNNING:
                m_nodeState = NodeState.RUNNING;
                return m_nodeState;
        }
        m_nodeState = NodeState.SUCCESS;
        return m_nodeState;
    }
}
