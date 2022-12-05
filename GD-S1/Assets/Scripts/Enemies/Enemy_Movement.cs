using System;
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
    public Action<float, float> UpdateInput;
    public Action ChargingAttack;
    public Action Attack;

    private ENEMY_STATE m_State = ENEMY_STATE.WANDERING;
    private NavMeshPath m_CurrentPath;
    private Vector3 m_Target;
    private bool m_IsAlive = true;

    [Header("Enemy AI Radius")]
    [SerializeField] private Player_Movement m_Player;
    [SerializeField][Min(0f)] private float m_DestinationOffsetRadius = 0.1f;
    [SerializeField][Min(0f)] private float m_WanderingRadius = 1f;
    [SerializeField][Min(0f)] private float m_ChaseRadius = 2f;
    [SerializeField][Min(0f)] private float m_AttackingRadius = 2f;

    [Header("Enemy Attacking")]
    [SerializeField] private float m_AttackChargeDuration = 0.5f;
    private float m_AttackChargeCurrentDuration = 0;
    [SerializeField] private float m_AttackDelay = 1f;
    private float m_CurrentAttackDelay = 0;
    [SerializeField] private float m_FlingImpulse = 5f;

    [Header("Enemy AI Alternate State Velocity")]
    [SerializeField] private float m_WanderSpeed = 0.5f;
    [SerializeField] private float m_ChasingSpeed = 0.5f;

    public void Initialise()
    {
        m_CurrentPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, NewWanderPoint(), NavMesh.AllAreas, m_CurrentPath);
        m_MaxSpeed = m_WanderSpeed;
    }

    private void FixedUpdate()
    {
        if (!m_IsAlive || m_IsPaused)
        {
            m_RB.velocity = new Vector2(0, 0);
            return;
        }

        switch (m_State)
        {
            case ENEMY_STATE.WANDERING:
                Wander();
                if (IsInRange(m_ChaseRadius))
                {
                    m_State = ENEMY_STATE.CHASING;
                    m_MaxSpeed = m_ChasingSpeed;
                }
                break;

            case ENEMY_STATE.CHASING:
                m_Target = m_Player.transform.position;
                if (!IsInRange(m_ChaseRadius))
                {
                    m_State = ENEMY_STATE.WANDERING;
                    m_MaxSpeed = m_WanderSpeed;
                }
                else if (IsInRange(m_AttackingRadius) && m_CurrentAttackDelay >= m_AttackDelay)
                {
                    m_State = ENEMY_STATE.ATTACKING;
                    m_MaxSpeed = m_FlingImpulse;
                    m_AttackChargeCurrentDuration = 0;
                    m_RB.velocity = new Vector2(0, 0);
                }
                break;

            case ENEMY_STATE.ATTACKING:
                Attacking();
                break;
        }

        if(m_State != ENEMY_STATE.ATTACKING)
        {
            if(m_CurrentAttackDelay < m_AttackDelay)
                m_CurrentAttackDelay += Time.fixedDeltaTime;

            m_InputDirection = (m_Target - transform.position).normalized;
            UpdateInput?.Invoke(m_InputDirection.x, m_InputDirection.y);

            ApplyMovement();
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

        if (m_CurrentPath.corners.Length >= 2)
            m_Target = m_CurrentPath.corners[1];
    }

    private Vector2 NewWanderPoint()
    {
        return (Vector2)transform.position + (UnityEngine.Random.insideUnitCircle * m_WanderingRadius);
    }

    private bool IsInRange(float radius)
    {
        return ((m_Player.transform.position - transform.position).sqrMagnitude < radius * radius);
    }

    private void Attacking()
    {
        ChargingAttack?.Invoke();
        m_RB.velocity = new Vector2(0, 0);
        m_AttackChargeCurrentDuration += Time.fixedDeltaTime;
        if (m_AttackChargeCurrentDuration >= m_AttackChargeDuration)
        {
            Attack?.Invoke();
            m_RB.AddForce(m_FlingImpulse * m_InputDirection, ForceMode2D.Impulse);

            m_State = ENEMY_STATE.CHASING;
            m_MaxSpeed = m_ChasingSpeed;
            m_CurrentAttackDelay = 0;
        }
    }

    public void SetKilled()
    {
        m_IsAlive = false;
        m_RB.velocity = Vector2.zero;
    }

    public void SetPlayerObject(Player_Movement player)
    {
        m_Player = player;
    }
}
