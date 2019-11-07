using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShip : NPC
{
    public Bounds movementBounds;
    private float rotationDamp = 0.02f;
    private float movementSpeed = 20f;
    private Transform _target;

    private void Start()
    {
        CreateTarget();
    }

   

    

    private void CreateTarget()
    {
        _target = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        _target.gameObject.layer = TARGET_LAYER;
        //_target.GetComponent<Renderer>().enabled = false;
    }

   

    private void Update()
    {
        TurnTowardsDestination();
        Move();
    }

    private void Move()
    {
        transform.position += transform.forward * Time.deltaTime * movementSpeed;
    }

    private void TurnTowardsDestination()
    {
        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.LookRotation(_target.position - transform.position), rotationDamp);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == TARGET_LAYER)
        {
            SetNewDestination();
        }

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
}
