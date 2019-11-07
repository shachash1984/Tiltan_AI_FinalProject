using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager S;

    public GameObject _boidShipPrefab;
    public Transform target;
    public int numShips = 10;
    public float maxVelocity = 25f;
    public float minVelocity = 20f;
    public float spawnRadius = 10f;
    public float spawnVelocity = 20f;
    public float nearDistance = 10f;
    public float collisionDistance = 2f;
    public float velocityMatchingAmt = 0.01f;
    public float flockCenteringAmt = 0.15f;
    public float cllisionAvoidanceAmt = -0.5f;
    public float targetAttractionAmount = 0.01f;
    public float targetAvoidanceDistance = 5f;
    public float targetAvoidanceAmt = 0.75f;
    public float velocityLerpAmt = 0.25f;


    private void Awake()
    {
        S = this;
        target = transform;
    }

    private void Start()
    {
        for (int i = 0; i < numShips; i++)
        {
            Instantiate(_boidShipPrefab);
        }
    }
}
