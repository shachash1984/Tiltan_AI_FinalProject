using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState { FAILURE, SUCCESS, RUNNING}

[System.Serializable]
public abstract class BehaviourNode 
{
    public delegate NodeState NodeReturn();

    protected NodeState m_nodeState;

    public NodeState nodeState { get => m_nodeState; }

    public BehaviourNode() { }

    public abstract NodeState Evaluate();
}
