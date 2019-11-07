using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyActionNode : ActionNode
{
    BehaviourTreeShip _npc;

    public SmallEnemyActionNode(BehaviourTreeShip npc)
    {
        _npc = npc;
        m_action = CheckIfEnemySmall;
    }

    private NodeState CheckIfEnemySmall()
    {
        if(_npc.currentEnemy != null)
        {
            if(_npc.currentEnemy.transform.localScale.x <= 1)
            {
                m_nodeState = NodeState.SUCCESS;
                return m_nodeState;
            }
        }
        m_nodeState = NodeState.FAILURE;
        return m_nodeState;

    }
}
