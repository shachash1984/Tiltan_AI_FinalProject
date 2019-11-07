using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSequence : BehaviourNode
{
    private List<BehaviourNode> m_nodes = new List<BehaviourNode>();

    public NodeSequence(List<BehaviourNode> nodes)
    {
        m_nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        bool anyChildRunning = false;

        foreach (BehaviourNode node in m_nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    m_nodeState = NodeState.FAILURE;
                    return m_nodeState;
                case NodeState.SUCCESS:
                    continue;
                case NodeState.RUNNING:
                    anyChildRunning = true;
                    continue;
                default:
                    m_nodeState = NodeState.SUCCESS;
                    return m_nodeState;
            }
        }

        m_nodeState = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return m_nodeState;
    }
}
