using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : BehaviourNode
{
    public delegate NodeState ActionNodeDelegate();

    protected ActionNodeDelegate m_action;

    public ActionNode(ActionNodeDelegate action)
    {
        m_action = action;
    }

    public ActionNode()
    {

    }

    public override NodeState Evaluate()
    {
        switch (m_action())
        {
            case NodeState.SUCCESS:
                m_nodeState = NodeState.SUCCESS;
                return m_nodeState;
            case NodeState.FAILURE:
                m_nodeState = NodeState.FAILURE;
                return m_nodeState;
            case NodeState.RUNNING:
                m_nodeState = NodeState.RUNNING;
                return m_nodeState;
            default:
                m_nodeState = NodeState.FAILURE;
                return m_nodeState;
        }
    }
}
