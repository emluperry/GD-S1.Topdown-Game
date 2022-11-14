using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum ENEMY_STATE
{
    WANDERING,
    CHASING,
    ATTACKING
}

public class Enemy_Movement : Entity_Movement
{
    private ENEMY_STATE m_State = ENEMY_STATE.WANDERING;
    private NavMeshPath m_CurrentPath;
    private Vector3 m_Target;

    //Coroutines
    Coroutine m_AttackingCoroutine;

    [Header("Enemy AI Radius")]
    [SerializeField] private Player_Movement m_Player;
    [SerializeField][Min(0f)] private float m_DestinationOffsetRadius = 0.1f;
    [SerializeField][Min(0f)] private float m_WanderingRadius = 1f;
    [SerializeField][Min(0f)] private float m_ChaseRadius = 2f;
    [SerializeField][Min(0f)] private float m_AttackingRadius = 2f;

    [Header("Enemy Attacking")]
    [SerializeField] private float m_AttackChargeDuration = 0.5f;

    [Header("Enemy AI Alternate State Velocity")]
    [SerializeField] private float m_WanderSpeed = 0.5f;
    [SerializeField] private float m_ChasingSpeed = 0.5f;
    [SerializeField] private float m_AttackingSpeed = 5f;

    private void Start()
    {
        m_CurrentPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, NewWanderPoint(), NavMesh.AllAreas, m_CurrentPath);
        m_MaxSpeed = m_WanderSpeed;
    }

    private void FixedUpdate()
    {
        switch (m_State)
        {
            case ENEMY_STATE.WANDERING:
                Wander();
                if (IsInRange(m_ChaseRadius))
                {
                    m_State = ENEMY_STATE.CHASING;
                    m_MaxSpeed = m_ChasingSpeed;
                }

                m_InputDirection = (m_Target - transform.position).normalized;

                ApplyMovement();
                break;

            case ENEMY_STATE.CHASING:
                m_Target = m_Player.transform.position;
                if (!IsInRange(m_ChaseRadius))
                {
                    m_State = ENEMY_STATE.WANDERING;
                    m_MaxSpeed = m_WanderSpeed;
                }
                else if (IsInRange(m_AttackingRadius))
                {
                    m_State = ENEMY_STATE.ATTACKING;
                    m_MaxSpeed = m_AttackingSpeed;
                }

                m_InputDirection = (m_Target - transform.position).normalized;

                ApplyMovement();
                break;

            case ENEMY_STATE.ATTACKING:
                if(m_AttackingCoroutine == null)
                    m_AttackingCoroutine = StartCoroutine(Attacking());

                if (!IsInRange(m_AttackingRadius))
                {
                    m_State = ENEMY_STATE.CHASING;
                    m_MaxSpeed = m_ChasingSpeed;
                }
                break;
        }
    }

    private void Wander()
    {
        Vector2 currentDestination;

        if (m_CurrentPath.corners.Length < 2 || ((currentDestination = m_CurrentPath.corners[m_CurrentPath.corners.Length - 1]) - (Vector2)transform.position).magnitude < m_DestinationOffsetRadius)
        {
            currentDestination = NewWanderPoint();
        }

        NavMesh.CalculatePath(transform.position, currentDestination, NavMesh.AllAreas, m_CurrentPath);

        m_Target = m_CurrentPath.corners[1];
    }

    private Vector2 NewWanderPoint()
    {
        return (Vector2)transform.position + (Random.insideUnitCircle * m_WanderingRadius);
    }

    private bool IsInRange(float radius)
    {
        return ((m_Player.transform.position - transform.position).sqrMagnitude < radius * radius);
    }

    private IEnumerator Attacking()
    {
        m_Target = m_Player.transform.position;
        yield return new WaitForSeconds(m_AttackChargeDuration);
        yield return new WaitForFixedUpdate();
        Debug.Log("ATTACK!!!");
        m_State = ENEMY_STATE.CHASING;
        StopCoroutine(m_AttackingCoroutine);
        m_AttackingCoroutine = null;
    }
}
