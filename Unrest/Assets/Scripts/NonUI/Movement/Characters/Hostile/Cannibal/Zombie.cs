using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private enum State
    {
        IDLE,
        WALK,
        CHASE,
        ATTACK
    }

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Transform player;

    private State movingState;

    private float chaseSpeed = 2.0f;
    private float attackDistance = 2f;
    private float stopNavMeshAgentDistance = 1.5f;
    public float rotationSpeed = 10f;
   
    void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;

        movingState = State.CHASE;
    }

    void Update()
    {
        switch (movingState)
        {
            case State.CHASE:
                {
                    Chase();
                    break;
                }
            case State.ATTACK:
                {
                    Attack();
                    break;
                }
        }
    }

    private void Chase()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.position);
        navMeshAgent.speed = chaseSpeed;

        //RotateTowardsPlayer();

        if (CheckAttackDistance())
        {
            movingState = State.ATTACK;
        }
    }

    private void Attack()
    {
        if (CheckNavMeshAgentDistance()) {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }

        //RotateTowardsPlayer();
        animator.SetBool("Attack", true);

        if (!CheckAttackDistance())
        {
            animator.SetBool("Attack", false);
            movingState = State.CHASE;
        }
    }

    private bool CheckAttackDistance()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        return dist <= attackDistance;
    }

    private bool CheckNavMeshAgentDistance()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        return dist <= stopNavMeshAgentDistance;
    }

    //understand
    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
