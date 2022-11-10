using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity_Movement : MonoBehaviour
{
    protected Rigidbody2D m_RB;

    protected Vector2 m_InputDirection;
    private Vector2 m_GoalVelocity;

    [Header("Entity Motion")]
    [SerializeField][Min(0f)] private float m_MaxSpeed = 1f;
    [SerializeField][Min(0f)] private float m_Acceleration = 200f;
    [SerializeField][Min(0f)] private float m_MaxAccelerationForce = 150f;
    [SerializeField][Min(0f)] private AnimationCurve m_AccelerationCurve;
    [SerializeField][Min(0f)] private AnimationCurve m_MaxAccelerationCurve;

    private void Awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
    }

    protected void ApplyMovement()
    {
        float velDot = Vector2.Dot(m_InputDirection, m_GoalVelocity.normalized);
        float acceleration = m_Acceleration * m_AccelerationCurve.Evaluate(velDot);

        m_GoalVelocity = Vector2.MoveTowards(m_GoalVelocity, m_InputDirection * m_MaxSpeed, acceleration * Time.fixedDeltaTime);

        Vector2 NeededAcceleration = (m_GoalVelocity - new Vector2(m_RB.velocity.x, m_RB.velocity.y)) / Time.fixedDeltaTime;

        float MaxAcceleration = m_MaxAccelerationForce * m_MaxAccelerationCurve.Evaluate(velDot);

        NeededAcceleration = Vector3.ClampMagnitude(NeededAcceleration, MaxAcceleration);

        m_RB.AddForce(NeededAcceleration, ForceMode2D.Force);
    }
}
