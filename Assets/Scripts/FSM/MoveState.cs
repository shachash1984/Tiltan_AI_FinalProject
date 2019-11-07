using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveState : State
{
    public Bounds movementBounds = new Bounds(Vector3.zero, Vector3.one * 100);
    public float destinationRadius = 10f;
    [SerializeField] private Vector3 destination;
    private float rotationDamp = 0.015f;
    private float movementSpeed = 10f;
    private GameObject _cubeDestination;
    private float _maxDestinationTime = 2f;
    private float _destinationTimer;
    private bool _visualizeDestination = true;
    public static event Action LockedInOrbit;
    private float _distanceFromDestination;
    private float _prevDistanceFromDestination;
    private float _minDeltaDistance = 0.5f;

    public override void Action()
    {
        if (ArrivedAtDestination())
        {
            SetNewDestination();
        }
        TurnTowardsDestination();
        Move();
        CheckForOrbitLock();
    }

    public override void OnEnterState(NPC npc)
    {
        base.OnEnterState(npc);
        SetNewDestination();
        _destinationTimer = Time.time;
    }

    private void Move()
    {
        transform.position += transform.forward * Time.deltaTime * movementSpeed;
    }

    private void TurnTowardsDestination()
    {
        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.LookRotation(destination - transform.position), rotationDamp);
    }

    private bool ArrivedAtDestination()
    {
        return _distanceFromDestination <= destinationRadius;
    }

    private Vector3 GetRandomPointInBounds()
    {
        Vector3 newPos = movementBounds.center;
        newPos.x = Random.Range(movementBounds.min.x, movementBounds.max.x);
        newPos.y = Random.Range(movementBounds.min.y, movementBounds.max.y);
        newPos.z = Random.Range(movementBounds.min.z, movementBounds.max.z);

        return newPos;
    }

    private void SetNewDestination()
    {
        destination = GetRandomPointInBounds();
        _destinationTimer = Time.time;
        if(_visualizeDestination)
        {
            if (_cubeDestination)
                Destroy(_cubeDestination);
            _cubeDestination = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _cubeDestination.transform.position = destination;
            
        }
        _distanceFromDestination = Vector3.Distance(transform.position, destination);
        _prevDistanceFromDestination = _distanceFromDestination;
    }

    private void CheckForOrbitLock()
    {
        if(Time.time - _maxDestinationTime >= _destinationTimer)
        {
            _destinationTimer = Time.time;
            _distanceFromDestination = Vector3.Distance(transform.position, destination);
            if (Mathf.Abs(_distanceFromDestination - _prevDistanceFromDestination) <= _minDeltaDistance)
            {
                LockedInOrbit?.Invoke();

            }
            _prevDistanceFromDestination = _distanceFromDestination;
        }
    }
}
