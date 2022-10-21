using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    //Components
    private Rigidbody2D m_RB;

    [SerializeField] private Transform m_Camera;
    private Vector2 m_InputDirection;
    private Vector2 m_UnitGoal;
    private Vector2 m_GoalVelocity;

    [Header("Player Motion")]
    [SerializeField] [Min(0f)] private float m_MaxSpeed = 1f;
    [SerializeField] [Min(0f)] private float m_Acceleration = 200f;
    [SerializeField] [Min(0f)] private float m_MaxAccelerationForce = 150f;
    [SerializeField] [Min(0f)] private AnimationCurve m_AccelerationCurve;
    [SerializeField] [Min(0f)] private AnimationCurve m_MaxAccelerationCurve;

    private void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        m_InputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        m_UnitGoal = m_Camera.TransformDirection(m_InputDirection);
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        //calculate goal velocity

        float velDot = Vector2.Dot(m_UnitGoal, m_GoalVelocity.normalized);
        float acceleration = m_Acceleration * m_AccelerationCurve.Evaluate(velDot);

        m_GoalVelocity = Vector2.MoveTowards(m_GoalVelocity, m_UnitGoal * m_MaxSpeed, acceleration*Time.fixedDeltaTime);

        Vector2 NeededAcceleration = (m_GoalVelocity - new Vector2(m_RB.velocity.x, m_RB.velocity.y)) / Time.fixedDeltaTime;

        float MaxAcceleration = m_MaxAccelerationForce * m_MaxAccelerationCurve.Evaluate(velDot);

        NeededAcceleration = Vector3.ClampMagnitude(NeededAcceleration, MaxAcceleration);

        m_RB.AddForce(NeededAcceleration, ForceMode2D.Force);
    }
}
