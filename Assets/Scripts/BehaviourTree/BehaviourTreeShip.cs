using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// Tree Scheme
///                       mainLoop(sequence)
///                     /                   \
///                  patrol(action)        events(selector)___________________________________
///                                       /                                                   \
///                                 movement(selector)                                        enemy(sequence)______________
///                                /                  \                                      /                             \
///             destination(sequence)                  orbit(sequence)_____            encounter(action)                   handle enemy(selector)
///            /                    \                        |             \                                               /                      \
/// reched destination(action)  set destination(action)     stuck(action)   free(action)                              small enemy(sequence)       teleport(action)
///                                                                                                                  /           |          \
///                                                                                         check if enemy is small(action)  aim(action)     shoot(action)
/// 
/// 
/// 


public class BehaviourTreeShip : NPC
{
    private NodeSequence _mainLoop;
    private PatrolActionNode _patrol;
    private NodeSequence _destinationSequence;
    private ReachTargetActionNode _reachTarget;
    private SetTargetPositionActionNode _setNewDestination;
    private NodeSequence _inOrbitSequence;
    private StuckActionNode _stuck;
    private FreeFromOrbitActionNode _free;
    private NodeSelector _movementSelector;
    private EncounterActionNode _encounter;
    private NodeSelector _eventSelector;
    private NodeSequence _enemySequence;
    private NodeSelector _handleEnemySelector;
    private SmallEnemyActionNode _checkEnemySmall;
    private NodeSequence _smallEnemy;
    private AimActionNode _aim;
    private ShootActionNode _shoot;
    private TeleportActionNode _teleport;

    public Enemy currentEnemy { get; set; }
    private Transform _target;
    

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        Enemy.EnemyDestroyed += HandleEnemyDestroyed;
    }

    private void OnDisable()
    {
        Enemy.EnemyDestroyed -= HandleEnemyDestroyed;
    }

    private void Update()
    {
        _mainLoop.Evaluate();
    }

    private void Init()
    {
        CreateTarget(true);

        //patrol
        _patrol = new PatrolActionNode(transform, _target, this);

        //destination sequence
        _reachTarget = new ReachTargetActionNode(transform, _target);
        _setNewDestination = new SetTargetPositionActionNode(_target, new Bounds(Vector3.zero, Vector3.one*100));
        List<BehaviourNode> destinationNodes = new List<BehaviourNode>();
        destinationNodes.Add(_reachTarget);
        destinationNodes.Add(_setNewDestination);
        _destinationSequence = new NodeSequence(destinationNodes);

        //free from orbit sequence
        _stuck = new StuckActionNode(transform, _target);
        _free = new FreeFromOrbitActionNode(transform, _target);
        List<BehaviourNode> orbitNodes = new List<BehaviourNode>();
        orbitNodes.Add(_stuck);
        orbitNodes.Add(_free);
        _inOrbitSequence = new NodeSequence(orbitNodes);

        //movement selector
        List<BehaviourNode> movementNodes = new List<BehaviourNode>();
        movementNodes.Add(_destinationSequence);
        movementNodes.Add(_inOrbitSequence);
        _movementSelector = new NodeSelector(movementNodes);

        //enemy sequence
        //encounter
        _encounter = new EncounterActionNode(transform, this, FindObjectsOfType<Enemy>());

        //small enemy
        _checkEnemySmall = new SmallEnemyActionNode(this);
        _aim = new AimActionNode(transform, _layerMask, this);
        _shoot = new ShootActionNode(transform, this);
        List<BehaviourNode> smallEnemyNodes = new List<BehaviourNode>();
        smallEnemyNodes.Add(_checkEnemySmall);
        smallEnemyNodes.Add(_aim);
        smallEnemyNodes.Add(_shoot);
        _smallEnemy = new NodeSequence(smallEnemyNodes);

        //teleport - not small enemy
        _teleport = new TeleportActionNode(transform, this);

        //handle enemy selector
        List<BehaviourNode> handleEnemyNodes = new List<BehaviourNode>();
        handleEnemyNodes.Add(_smallEnemy);
        handleEnemyNodes.Add(_teleport);
        _handleEnemySelector = new NodeSelector(handleEnemyNodes);

        //enemy sequence
        List<BehaviourNode> enemyNodes = new List<BehaviourNode>();
        enemyNodes.Add(_encounter);
        enemyNodes.Add(_handleEnemySelector);
        _enemySequence = new NodeSequence(enemyNodes);

        //events selector
        List<BehaviourNode> eventNodes = new List<BehaviourNode>();
        eventNodes.Add(_movementSelector);
        eventNodes.Add(_enemySequence);
        _eventSelector = new NodeSelector(eventNodes);


        //initializing main sequence
        List<BehaviourNode> mainLoopNodes = new List<BehaviourNode>();
        mainLoopNodes.Add(_patrol);
        mainLoopNodes.Add(_eventSelector);
        _mainLoop = new NodeSequence(mainLoopNodes);

    }


    private void CreateTarget(bool debug = false)
    {
        _target = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        if (debug)
            _target.position = Vector3.zero;
        _target.gameObject.layer = TARGET_LAYER;
        _target.GetComponent<Renderer>().enabled = false;
    }


    private void HandleEnemyDestroyed(Enemy e)
    {
        currentEnemy = null;
    }
}
