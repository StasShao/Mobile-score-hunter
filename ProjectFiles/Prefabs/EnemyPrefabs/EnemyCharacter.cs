using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterMechanicSystems;
using UnityEngine.AI;
public class EnemyCharacter : Character,IAIControllable
{
    protected AIController aiController;
    [SerializeField] protected Animator SearcherAnimator;
    [SerializeField] protected Transform WayPoint;
    [SerializeField] protected float SearchinDistance;
    [SerializeField] protected LayerMask EnemyLayer;
    [SerializeField] protected NavMeshAgent Agent;
    [SerializeField] protected float WayPointMinDistance;
    [SerializeField] protected LayerMask GroundLayer;

    #region IAIControllable
    public Transform EnemyTransform { get; private set; }
    public bool IsEnemyDetected { get; private set; }
    public void SetEnemyDetected(bool isDetected)
    {
        IsEnemyDetected = isDetected;
    }

    public void SetEnemy(Transform enemy)
    {
        EnemyTransform = enemy;
    }
    #endregion

    public override void Begin()
    {
        Init(transform);
        aiController = new AIController(controller,SearcherAnimator,WayPoint);
    }

    public override void FixedTick()
    {
        
    }

    public override void OnPointMove()
    {
        
    }

    public override void Tick()
    {
        AIBehavior();
        OnSearching();
        WaypointRandomizePosition();
    }
    public virtual void OnSearching()
    {
        aiController.EnemySearching(this, SearchinDistance, EnemyLayer);
    }
    public virtual void AIBehavior()
    {
        
        aiController.OnMoveBehavior(IsEnemyDetected,Agent,EnemyTransform);
    }
    public virtual void WaypointRandomizePosition()
    {
        aiController.WayPointRandomizerPosition(WayPointMinDistance,GroundLayer,transform);
    }
    private void Start()
    {
        Begin();
    }
    private void FixedUpdate()
    {
        FixedTick();
    }
    private void Update()
    {
        Tick();
    }
}

