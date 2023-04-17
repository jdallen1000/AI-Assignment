using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FininteStateMachine;

public class EnemyPathfinding : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] GameObject navPoint;
    
    [SerializeField] GameObject Player;
    
    [SerializeField] float stoppingDis;

    [SerializeField] float detectionDis;

    

    public StateMachine StateMachine { get; private set; }

    // Start is called before the first frame update
    public void Awake()
    {
        StateMachine = new StateMachine();

        if(!TryGetComponent<NavMeshAgent>(out agent))
        {
            Debug.LogError("this object has no nav mesh!");
        }
    }

    void Start()
    {
        agent.isStopped = true;
        StateMachine.SetState(new idleState(this));
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.OnUpdate();
    }
    public abstract class EnemyMoveState : IState
    {
        protected EnemyPathfinding instance;

        public EnemyMoveState(EnemyPathfinding _instance)
        {
            instance = _instance;
        }

        public virtual void OnEnter()
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }
    }

    public class moveState : EnemyMoveState
    {
        public moveState(EnemyPathfinding _instance) : base(_instance)
        {

        }

        public override void OnEnter()
        {
            //set agent to not stopped
            Debug.Log("entering Idle state");
            instance.agent.isStopped = false;
        }

        public override void OnUpdate()
        {
            if (Vector3.Distance(instance.transform.position, instance.Player.transform.position) < instance.detectionDis)
            {
                instance.StateMachine.SetState(new ChaseState(instance));
            }
            else if (Vector3.Distance(instance.transform.position, instance.navPoint.transform.position) > instance.stoppingDis)
            {
                instance.agent.SetDestination(instance.navPoint.transform.position);
            }
            else
            {
                // set state idle
                instance.StateMachine.SetState(new idleState(instance));
            }
        }
    }

    public class idleState : EnemyMoveState
    {
        public idleState(EnemyPathfinding _instance) : base(_instance)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("entering Mstate");
            instance.agent.isStopped = true;
        }

        public override void OnUpdate()
        {
            if (Vector3.Distance(instance.transform.position, instance.Player.transform.position) < instance.detectionDis)
            {
                instance.StateMachine.SetState(new ChaseState(instance));
            }
            else if (Vector3.Distance(instance.transform.position, instance.navPoint.transform.position) > instance.stoppingDis)
            {
                //switch move
                instance.StateMachine.SetState(new moveState(instance));
            }

        }
    }
    public class ChaseState : EnemyMoveState
    {
        public ChaseState(EnemyPathfinding _instance) : base(_instance)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("entering Cstate");
            instance.agent.isStopped = false;
        }

        public override void OnUpdate()
        {
            if (Vector3.Distance(instance.transform.position, instance.Player.transform.position) < instance.detectionDis)
            {
                instance.agent.isStopped = false;
            }
            else
            {
                instance.StateMachine.SetState(new idleState(instance));
            }
        }
    }

}
