using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidShip : MonoBehaviour
{
    public static List<BoidShip> ships;
    public static Transform target;

    private Vector3 _velocity;
    private Vector3 _newVelocity;
    private Vector3 _newPosition;

    private List<BoidShip> _neighbors;
    private List<BoidShip> _collisionRisks;
    private BoidShip _closest;

    private void Awake()
    {
        if (ships == null)
        {
            ships = new List<BoidShip>();
        }
        ships.Add(this);
        if (target == null)
            target = BoidManager.S.target;

        float randomX = Random.Range(BoidManager.S.transform.position.x - BoidManager.S.spawnRadius, BoidManager.S.transform.position.x + BoidManager.S.spawnRadius);
        float randomZ = Random.Range(BoidManager.S.transform.position.z - BoidManager.S.spawnRadius*4, BoidManager.S.transform.position.z - BoidManager.S.spawnRadius*2);
        Vector3 randomPos = new Vector3(randomX, BoidManager.S.transform.position.y, randomZ);
        transform.position = randomPos;
        transform.rotation = BoidManager.S.transform.rotation;
        _velocity = Random.onUnitSphere;
        _velocity *= BoidManager.S.spawnVelocity;
        _velocity.y = 1;

        _neighbors = new List<BoidShip>();
        _collisionRisks = new List<BoidShip>();

        //transform.SetParent(BoidManager.S.transform);

        Color randColor = Color.black;
        while (randColor.r + randColor.g + randColor.b < 1.0)
        {
            randColor = new Color(Random.value, Random.value, Random.value);
        }
        GetComponent<Renderer>().material.color = randColor;

    }

    private void Update()
    {
        List<BoidShip> tempNeighbors = GetNeighbors(this);
        _newVelocity = _velocity;
        _newPosition = transform.position;

        Vector3 neighborVel = GetAverageVelocity(_neighbors);
        _newVelocity += neighborVel * BoidManager.S.velocityMatchingAmt;

        Vector3 neighborCenterOffset = GetAveragePosition(_neighbors) - transform.position;
        _newPosition += neighborCenterOffset * BoidManager.S.flockCenteringAmt;

        Vector3 dist;
        if (_collisionRisks.Count > 0)
        {
            Vector3 collisionAveragePos = GetAveragePosition(_collisionRisks);
            dist = collisionAveragePos - transform.position;
            _newVelocity += dist * BoidManager.S.cllisionAvoidanceAmt;
        }

        dist = target.position - transform.position;
        if (dist.magnitude > BoidManager.S.targetAvoidanceDistance)
        {
            _newVelocity += dist * BoidManager.S.targetAttractionAmount;
        }
        else
        {
            _newVelocity = Vector3.zero;
            //_newVelocity -= dist * ZombieBoidsManager.S.targetAvoidanceDistance * ZombieBoidsManager.S.targetAvoidanceAmt;
        }
    }

    private void LateUpdate()
    {
        _velocity = (1 - BoidManager.S.velocityLerpAmt) * _velocity + BoidManager.S.velocityLerpAmt * _newVelocity;
        if (_velocity.magnitude > BoidManager.S.maxVelocity)
        {
            _velocity = _velocity.normalized * BoidManager.S.maxVelocity;
        }
        if (_velocity.magnitude < BoidManager.S.minVelocity)
        {
            _velocity = _velocity.normalized * BoidManager.S.minVelocity;
        }
        _newPosition = transform.position + _velocity * Time.deltaTime;
        //_newPosition.y = 1;

        transform.LookAt(_newPosition);
        transform.position = _newPosition;
    }

    private List<BoidShip> GetNeighbors(BoidShip boidShip)
    {
        float closestDistance = float.MaxValue;
        Vector3 delta;
        float dist;
        _neighbors.Clear();
        _collisionRisks.Clear();

        foreach (BoidShip zb in ships)
        {
            if (zb == boidShip)
                continue;
            delta = zb.transform.position - transform.position;
            dist = delta.magnitude;
            if (dist < closestDistance)
            {
                closestDistance = dist;
                _closest = zb;
            }
            if (dist < BoidManager.S.nearDistance)
            {
                _neighbors.Add(zb);
            }
            if (dist < BoidManager.S.collisionDistance)
            {
                _collisionRisks.Add(zb);
            }
        }
        if (_neighbors.Count == 0)
        {
            _neighbors.Add(_closest);
        }
        return _neighbors;
    }

    public Vector3 GetAveragePosition(List<BoidShip> zomboids)
    {
        Vector3 sum = Vector3.zero;
        if (zomboids.Count == 0)
            return sum;
        foreach (BoidShip zb in zomboids)
        {
            sum += zb.transform.position;
        }
        Vector3 center = sum / zomboids.Count;
        return center;
    }

    public Vector3 GetAverageVelocity(List<BoidShip> zomboids)
    {
        Vector3 sum = Vector3.zero;
        if (zomboids.Count == 0)
            return sum;
        foreach (BoidShip zb in zomboids)
        {
            sum += zb._velocity;
        }
        Vector3 center = sum / zomboids.Count;
        return center;
    }
}
