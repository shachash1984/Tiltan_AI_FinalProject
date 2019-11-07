using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSelector : BehaviourNode
{
    protected List<BehaviourNode> m_nodes = new List<BehaviourNode>();

    public NodeSelector(List<BehaviourNode> nodes)
    {
        m_nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (BehaviourNode node in m_nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    continue;
                case NodeState.SUCCESS:
                    m_nodeState = NodeState.SUCCESS;
                    return m_nodeState;
                case NodeState.RUNNING:
                    m_nodeState = NodeState.RUNNING;
                    return m_nodeState;
                default:
                    continue;
            }
        }
        m_nodeState = NodeState.FAILURE;
        return m_nodeState;
    }
}
