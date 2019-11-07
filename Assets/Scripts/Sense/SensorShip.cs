using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SensorShip : NPC
{
    private Enemy _currentEnemy;
    private float radius = 100f;
    public Bounds movementBounds;
    private float rotationDamp = 0.02f;
    private float movementSpeed = 20f;
    private Transform _target;

    private void Start()
    {
        CreateTarget();
    }

    private void OnEnable()
    {
        Sense.EnemyDetected += HandleEnemyDetected;
        Enemy.EnemyDestroyed += HandleEnemyDestroyed;
    }

    private void OnDisable()
    {
        Sense.EnemyDetected -= HandleEnemyDetected;
        Enemy.EnemyDestroyed -= HandleEnemyDestroyed;
    }

    private void HandleEnemyDestroyed(Enemy e)
    {
        _currentEnemy = null;
        CreateTarget();
        SetNewDestination();
    }

    private void CreateTarget()
    {
        _target = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        _target.gameObject.layer = TARGET_LAYER;
        _target.GetComponent<Renderer>().enabled = false;
    }

    private void HandleEnemyDetected(Sense s, Enemy e)
    {
        Debug.Log("Enemy Detected by: " + s.name);
        if (!_currentEnemy)
        {
            Destroy(_target.gameObject);
            _currentEnemy = e;
            _target = _currentEnemy.transform;
        }
            
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
        else if(other.gameObject.layer == ENEMY_LAYER)
        {
            Destroy(other.gameObject);
            HandleEnemyDestroyed(other.GetComponent<Enemy>());
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
