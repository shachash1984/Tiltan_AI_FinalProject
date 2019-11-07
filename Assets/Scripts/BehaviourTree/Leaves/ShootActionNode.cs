using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootActionNode : ActionNode
{
    private float _fireRate;
    private float _shotTimer;
    Transform _self;
    BehaviourTreeShip _npc;

    public ShootActionNode(Transform self, BehaviourTreeShip npc)
    {
        _self = self;
        _fireRate = 1f;
        _npc = npc;
        m_action = Shoot;
    }

    private NodeState Shoot()
    {
        if (_npc.currentEnemy != null)
        {
            if (IsAllowedToShoot())
            {
                _shotTimer = Time.time;
                GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                projectile.layer = NPC.PROJECTILE_LAYER;
                projectile.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);
                Vector3 projRot = _self.rotation.eulerAngles;
                projRot.x += 90;
                projectile.transform.rotation = Quaternion.Euler(projRot);
                projectile.transform.position = _self.position + _self.forward * 2;
                projectile.GetComponent<Renderer>().material.color = Color.yellow;
                Rigidbody rb = projectile.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.velocity = _self.forward * 30;
                Object.Destroy(projectile, 5);
                m_nodeState = NodeState.SUCCESS;
                return m_nodeState;
            }
            else
            {
                m_nodeState = NodeState.RUNNING;
                return m_nodeState;
            }
        }
        else
        {
            m_nodeState = NodeState.FAILURE;
            return m_nodeState;
        }
            
    }



    private bool IsAllowedToShoot()
    {
        return Time.time - _fireRate >= _shotTimer;
    }
}
