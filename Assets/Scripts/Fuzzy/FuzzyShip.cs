using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuzzyShip : NPC
{
    [Range(0f, 1f)] [SerializeField] private float _energy;
    [Range(0f, 1f)] [SerializeField] private float _danger;

    [SerializeField] private AnimationCurve _energyConsumption;
    [SerializeField] private AnimationCurve _dangerCurve;

    [SerializeField] Text _dangerText;
    [SerializeField] Text _energyText;
    

    const float MAX_ENEMY_DISTANCE = 100;
    const float NO_ENEMIES = 1;
    const float IN_BATTLE = 0;
    const float GAIN_ENERGY_MULTIPLIER = 1000;
    Enemy[] _enemies;
    private Enemy _currentEnemy;
    private float radius = 100f;
    public Bounds movementBounds;
    private float rotationDamp = 0.02f;
    private float movementSpeed = 20f;
    private Transform _target;
    private float _fireRate = 1f;
    private float _shotTimer;
    

    private void Start()
    {
        CreateTarget();
        _energy = 1;
        SetDangerLevel(NO_ENEMIES);
        _enemies = FindObjectsOfType<Enemy>();
    }

    private void OnEnable()
    {
        Enemy.EnemyDestroyed += HandleEnemyDestroyed;
    }

    private void OnDisable()
    {
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
        //_target.GetComponent<Renderer>().enabled = false;
    }

    private void HandleEnemyDetected(Sense s, Enemy e)
    {
        Debug.Log("Enemy Detected by: " + s.name);
        

    }

    private void Update()
    {
        if(_currentEnemy)
        {
            Aim();
            Shoot();
        }
        else
        {
            TurnTowardsDestination();
            Move();
            
        }
        CheckForEnemies();
        UpdateUI();
    }

    private void Move()
    {
        transform.position += transform.forward * Time.deltaTime * movementSpeed*_energy;
        ConsumeEnergy();
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
            GainEnergy();
        }
        if(other.gameObject.layer == ENEMY_LAYER)
        {
            if (!_currentEnemy)
            {
                Destroy(_target.gameObject);
                _currentEnemy = other.GetComponent<Enemy>();
                SetDangerLevel(IN_BATTLE);
            }
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

    private void Shoot()
    {
        if (IsEnemyInSight() && IsAllowedToShoot())
        {
            _shotTimer = Time.time;
            GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            projectile.layer = NPC.PROJECTILE_LAYER;
            projectile.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);
            Vector3 projRot = transform.rotation.eulerAngles;
            projRot.x += 90;
            projectile.transform.rotation = Quaternion.Euler(projRot);
            projectile.transform.position = transform.position + transform.forward * 2;
            projectile.GetComponent<Renderer>().material.color = Color.yellow;
            Rigidbody rb = projectile.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = transform.forward * 30;
            Destroy(projectile, 5);
            ConsumeEnergy();
        }
    }

    private bool IsAllowedToShoot()
    {
        return Time.time - _fireRate >= _shotTimer;
    }

    private void Aim()
    {
        if (IsEnemyInSight())
            return;
        else
            TurnTowardsEnemy();
    }

    private bool IsEnemyInSight()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, _layerMask))
        {
            if (hit.collider.gameObject.layer == NPC.ENEMY_LAYER)
                return true;
        }
        return false;
    }

    private void TurnTowardsEnemy()
    {
        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.LookRotation(_currentEnemy.transform.position - transform.position), 0.1f);
    }

    private void ConsumeEnergy()
    {
        _energy -= _energyConsumption.Evaluate(_danger);
        ClampValue(ref _energy);
    }

    private void GainEnergy()
    {
        _energy += _energyConsumption.Evaluate(_danger)*GAIN_ENERGY_MULTIPLIER;
        ClampValue(ref _energy);
    }

    public void CheckForEnemies()
    {
        if (_currentEnemy == null && _enemies.Length > 0)
        {
            float closestEnemyDistance = MAX_ENEMY_DISTANCE;
            foreach (Enemy e in _enemies)
            {
                if (e == null)
                    continue;
                float distance = Vector3.Distance(e.transform.position, transform.position);
                if (distance < closestEnemyDistance)
                {
                    closestEnemyDistance = distance;
                    closestEnemyDistance /= MAX_ENEMY_DISTANCE;
                    
                }
            }
            closestEnemyDistance = closestEnemyDistance > 1 ? 1 : closestEnemyDistance;
            SetDangerLevel(closestEnemyDistance);
        }
        else
        {
            SetDangerLevel(NO_ENEMIES);
        }
    }

    private void SetDangerLevel(float val)
    {
        _danger = _dangerCurve.Evaluate(val);
        ClampValue(ref _danger);
    }

    private void ClampValue(ref float val)
    {
        if (val < 0)
            val = 0;
        else if (val > 1)
            val = 1;
    }

    private void UpdateUI()
    {
        _dangerText.text = string.Format("Danger: {0}", _danger.ToString("f2"));
        _energyText.text = string.Format("Energy: {0}", _energy.ToString("f2"));
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
