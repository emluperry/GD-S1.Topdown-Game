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

    [Header("Enemy Variables")]
    [SerializeField] private Player_Movement m_Player;
    [SerializeField][Min(0f)] private float m_DestinationOffsetRadius = 0.1f;
    [SerializeField][Min(0f)] private float m_WanderingRadius = 1f;
    [SerializeField][Min(0f)] private float m_ChaseRadius = 2f;

    private void Start()
    {
        m_CurrentPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, NewWanderPoint(), NavMesh.AllAreas, m_CurrentPath);
    }

    private void FixedUpdate()
    {
        switch (m_State)
        {
            case ENEMY_STATE.WANDERING:
                Wander();
                if (IsInPlayerRange())
                    m_State = ENEMY_STATE.CHASING;
                break;

            case ENEMY_STATE.CHASING:
                m_Target = m_Player.transform.position;
                if (!IsInPlayerRange())
                    m_State = ENEMY_STATE.WANDERING;
                break;

            case ENEMY_STATE.ATTACKING:
                break;
        }

        m_InputDirection = (m_Target - transform.position).normalized;

        ApplyMovement();
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

    private bool IsInPlayerRange()
    {
        return ((m_Player.transform.position - transform.position).sqrMagnitude < m_ChaseRadius * m_ChaseRadius);
    }
}
