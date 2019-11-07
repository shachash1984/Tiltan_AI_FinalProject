using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TeleportActionNode : ActionNode
{
    Transform _self;
    Renderer _renderer;
    Bounds _bounds;
    BehaviourTreeShip _npc;

    public TeleportActionNode(Transform self, BehaviourTreeShip npc)
    {
        _self = self;
        _renderer = _self.GetComponent<Renderer>();
        _bounds = new Bounds(Vector3.zero, Vector3.one * 100);
        _npc = npc;
        m_action = Teleport;
    }

    public NodeState Teleport()
    {
        if(_npc.currentEnemy != null)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_renderer.material.DOFade(0, 0.5f));
            seq.Append(_self.DOMove(GetRandomPointInBounds(), 0));
            seq.Append(_renderer.material.DOFade(1, 1.5f));
            _npc.currentEnemy = null;
            m_nodeState = NodeState.SUCCESS;
            return m_nodeState;
        }
        m_nodeState = NodeState.FAILURE;
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
