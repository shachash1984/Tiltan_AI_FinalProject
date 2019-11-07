using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceShip : NPC
{

    public float movementSpeed = 5;
    public float rotationSpeed = 2;
    public float _force = 10;
    private Transform _target;
    private float _currentSpeed;
    public Bounds movementBounds;


    private void Update()
    {
        if (!_target)
        {
            CreateTarget();
            SetNewDestination();
        }
            
        MoveToPosition();
    }

    protected void MoveToPosition()
    {
        if (_target)
        {
            Vector3 dir = (_target.position - transform.position);
            dir.Normalize();
            AvoidObstacles(ref dir);

            _currentSpeed = movementSpeed * Time.deltaTime;
            var rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
            Vector3 wantedPos = transform.forward * _currentSpeed;
            transform.position += wantedPos;
            Debug.DrawLine(transform.position + transform.forward, transform.position + transform.forward*20 + dir);
        }
    }

    private void AvoidObstacles(ref Vector3 dir)
    {
        RaycastHit hit;
        
        if (Physics.SphereCast(transform.position,2 , transform.forward, out hit, 200, _layerMask))
        {
            Vector3 hitNormal = Vector3.Cross(hit.normal, dir);
            dir = transform.forward + hitNormal * _force * Time.deltaTime;
            Debug.Log("Avoided");
        }
    }

    private void CreateTarget()
    {
        _target = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        _target.gameObject.layer = TARGET_LAYER;
        //_target.GetComponent<Renderer>().enabled = false;
    }

    private void SetNewDestination()
    {
        _target.position = GetRandomPointInBounds();
    }

    private Vector3 GetRandomPointInBounds()
    {
        Vector3 newPos = movementBounds.center;
        newPos.x = Random.Range(movementBounds.min.x, movementBounds.max.x);
        newPos.y = Random.Range(movementBounds.min.y, movementBounds.max.y);
        newPos.z = Random.Range(movementBounds.min.z, movementBounds.max.z);

        return newPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == TARGET_LAYER)
            SetNewDestination();
    }
}
