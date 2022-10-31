using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Weapon_Movement : MonoBehaviour
{
    private Rigidbody2D m_RB;
    [SerializeField] private Transform m_Player;

    private Vector2 m_InputDirection;
    private Vector2 m_GoalVelocity;
    private Vector2 m_PrevLocation;
    private float m_DistTravelled;
    private bool m_Returning;

    [Header("Weapon Limits")]
    [SerializeField][Min(0f)] private float m_MaxSpeed = 10f;
    [SerializeField][Min(0f)] private float m_MaxDistance = 4f;
    [SerializeField][Min(0f)] private float m_MaxAccelerationForce = 150f;
    [SerializeField][Min(0f)] private AnimationCurve m_MaxAccelerationCurve;

    [Header("Weapon Movement")]
    [SerializeField][Min(0f)] private float m_Acceleration = 200f;
    [SerializeField][Min(0f)] private AnimationCurve m_AccelerationCurve;
    [SerializeField][Min(0f)] private float m_ReturnSpeed = 20f;

    private void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_DistTravelled = 0;
        m_Returning = false;
    }

    private void Update()
    {
        m_InputDirection = new Vector2(Input.GetAxis("W-Horizontal"), Input.GetAxis("W-Vertical"));
    }

    private void FixedUpdate()
    {
        ApplyMovement();

        if(m_Returning)
        {
            //check if close to player
            //if so, freeze velocity and reset functionality
        }
    }

    private void ApplyMovement()
    {
        float deltaDistance = Vector2.Distance(m_PrevLocation, transform.position);
        m_DistTravelled += deltaDistance;

        Vector2 NeededAcceleration;

        if (m_DistTravelled > m_MaxDistance)
        {
            if(!m_Returning)
            {
                m_RB.velocity = new Vector2(0, 0);
                m_Returning = true;
            }

            //logic
            NeededAcceleration = m_ReturnSpeed * (m_Player.position - transform.position).normalized / Time.fixedDeltaTime;

            m_RB.AddForce(NeededAcceleration, ForceMode2D.Force);
        }
        else
        {
            float velDot = Vector2.Dot(m_InputDirection, m_GoalVelocity.normalized);
            float acceleration = m_Acceleration * m_AccelerationCurve.Evaluate(velDot);

            m_GoalVelocity = Vector2.MoveTowards(m_GoalVelocity, m_InputDirection * m_MaxSpeed, acceleration * Time.fixedDeltaTime);

            NeededAcceleration = (m_GoalVelocity - new Vector2(m_RB.velocity.x, m_RB.velocity.y)) / Time.fixedDeltaTime;

            float MaxAcceleration = m_MaxAccelerationForce * m_MaxAccelerationCurve.Evaluate(velDot);

            NeededAcceleration = Vector2.ClampMagnitude(NeededAcceleration, MaxAcceleration);

            m_PrevLocation = transform.position;
            m_RB.AddForce(NeededAcceleration, ForceMode2D.Force);
        }
    }
}
