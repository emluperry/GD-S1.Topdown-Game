using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Weapon_Movement : MonoBehaviour
{
    private Rigidbody2D m_RB;
    private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private Transform m_Player;

    private Vector2 m_InputDirection;
    private Vector2 m_GoalVelocity;
    private Vector2 m_PrevLocation;
    private float m_DistTravelled;
    private bool m_Returning;

    [Header("Weapon Limits")]
    [SerializeField][Min(0f)] private float m_MaxSpeed = 10f;
    [SerializeField][Min(0f)] private float m_MaxDistance = 4f;
    [SerializeField][Min(0f)] private float m_MaxReturnDistance = 0.1f;
    [SerializeField][Min(0f)] private float m_MaxAccelerationForce = 150f;
    [SerializeField][Min(0f)] private AnimationCurve m_MaxAccelerationCurve;
    [SerializeField][Min(0f)] private float m_MaxReturnAccelerationForce = 200f;

    [Header("Weapon Movement")]
    [SerializeField][Min(0f)] private float m_Acceleration = 200f;
    [SerializeField][Min(0f)] private AnimationCurve m_AccelerationCurve;
    [SerializeField][Min(0f)] private float m_ReturnSpeed = 20f;

    private void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.enabled = false;
        m_PrevLocation = transform.position;

        m_DistTravelled = 0;
        m_Returning = false;
    }

    private void Update()
    {
        if (!m_Returning)
            m_InputDirection = new Vector2(Input.GetAxis("W-Horizontal"), Input.GetAxis("W-Vertical"));
        else
            m_InputDirection = new Vector2(0, 0);

        if(m_InputDirection.magnitude == 0 && m_SpriteRenderer.enabled == true)
        {
            m_Returning = true;
        }
        else if(m_InputDirection.magnitude > 0 && m_SpriteRenderer.enabled == false)
        {
            m_SpriteRenderer.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        Debug.Log(m_DistTravelled);

        if(m_Returning)
        {
            if(Vector2.Distance(transform.position, m_Player.position) <= m_MaxReturnDistance)
            {
                m_SpriteRenderer.enabled = false;
                m_RB.velocity = new Vector2(0, 0);
                m_DistTravelled = 0;
                m_RB.MovePosition(m_Player.position);
                m_PrevLocation = transform.position;
                m_Returning = false;
            }
        }
    }

    private void ApplyMovement()
    {
        if(!m_Returning && m_SpriteRenderer.enabled == true)
        {
            m_DistTravelled += Vector2.Distance(m_PrevLocation, transform.position);
        }

        Vector2 NeededAcceleration;
        float MaxAcceleration;

        if (m_DistTravelled > m_MaxDistance || m_Returning)
        {
            if(!m_Returning)
            {
                m_RB.velocity = new Vector2(0, 0);
                m_Returning = true;
            }

            //logic
            NeededAcceleration = m_ReturnSpeed * (m_Player.position - transform.position).normalized / Time.fixedDeltaTime;
            MaxAcceleration = m_MaxReturnAccelerationForce;
        }
        else
        {
            float velDot = Vector2.Dot(m_InputDirection, m_GoalVelocity.normalized);
            float acceleration = m_Acceleration * m_AccelerationCurve.Evaluate(velDot);

            m_GoalVelocity = Vector2.MoveTowards(m_GoalVelocity, m_InputDirection * m_MaxSpeed, acceleration * Time.fixedDeltaTime);

            NeededAcceleration = (m_GoalVelocity - new Vector2(m_RB.velocity.x, m_RB.velocity.y)) / Time.fixedDeltaTime;

            MaxAcceleration = m_MaxAccelerationForce * m_MaxAccelerationCurve.Evaluate(velDot);

            m_PrevLocation = transform.position;
        }

        NeededAcceleration = Vector2.ClampMagnitude(NeededAcceleration, MaxAcceleration);

        m_RB.AddForce(NeededAcceleration, ForceMode2D.Force);
    }
}
