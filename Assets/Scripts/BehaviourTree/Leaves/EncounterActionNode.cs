using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterActionNode : ActionNode
{
    Transform _self;
    private Enemy[] _enemies;
    private float _encounterRange;
    private BehaviourTreeShip _npc;

    public EncounterActionNode(Transform self, BehaviourTreeShip npc, Enemy[] enemies)
    {
        _encounterRange = 15f;
        _self = self;
        _npc = npc;
        _enemies = enemies;
        m_action = CheckForEnemies;
    }

    public NodeState CheckForEnemies()
    {
        if (_npc.currentEnemy == null)
        {
            foreach (Enemy e in _enemies)
            {
                if (e == null)
                    continue;
                if (Vector3.Distance(e.transform.position, _self.position) <= _encounterRange)
                {
                    Debug.Log("encountered enemy");
                    _self.GetComponent<BehaviourTreeShip>().currentEnemy = e;
                    m_nodeState = NodeState.SUCCESS;
                    return m_nodeState;
                }
            }
            m_nodeState = NodeState.FAILURE;
            return m_nodeState;
        }
        else
        {
            m_nodeState = NodeState.RUNNING;
            return m_nodeState;
        }
            
        
    }
}
