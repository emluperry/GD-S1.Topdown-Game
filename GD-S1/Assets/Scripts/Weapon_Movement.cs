using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Movement : MonoBehaviour
{
    private Rigidbody2D m_RB;

    private Vector2 m_InputDirection;
    private Vector2 m_GoalVelocity;
    private Vector2 m_PrevLocation;
    private float m_DistTravelled;

    [Header("Weapon Limits")]
    [SerializeField][Min(0f)] private float m_MaxSpeed = 10f;
    [SerializeField][Min(0f)] private float m_MaxDistance = 4f;
    [SerializeField][Min(0f)] private float m_MaxAccelerationForce = 150f;
    [SerializeField][Min(0f)] private AnimationCurve m_MaxAccelerationCurve;

    [Header("Weapon Movement")]
    [SerializeField][Min(0f)] private float m_Acceleration = 200f;
    [SerializeField][Min(0f)] private AnimationCurve m_AccelerationCurve;

    private void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_DistTravelled = 0;
    }

    private void Update()
    {
        m_InputDirection = new Vector2(Input.GetAxis("W-Horizontal"), Input.GetAxis("W-Vertical"));
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        //has current position. determine distance moved since last called
        float deltaDistance = (m_PrevLocation - new Vector2(transform.position.x, transform.position.y)).magnitude;
        //add distance to accumulative dist
        m_DistTravelled += deltaDistance;
        //if greater than max distance, return to player. else continue
        if(m_DistTravelled > m_MaxDistance)
        {
            m_RB.velocity = new Vector2(0, 0);
        }
        else
        {
            float velDot = Vector2.Dot(m_InputDirection, m_GoalVelocity.normalized);
            float acceleration = m_Acceleration * m_AccelerationCurve.Evaluate(velDot);

            m_GoalVelocity = Vector2.MoveTowards(m_GoalVelocity, m_InputDirection * m_MaxSpeed, acceleration * Time.fixedDeltaTime);

            Vector2 NeededAcceleration = (m_GoalVelocity - new Vector2(m_RB.velocity.x, m_RB.velocity.y)) / Time.fixedDeltaTime;

            float MaxAcceleration = m_MaxAccelerationForce * m_MaxAccelerationCurve.Evaluate(velDot);

            NeededAcceleration = Vector3.ClampMagnitude(NeededAcceleration, MaxAcceleration);

            m_PrevLocation = transform.position;
            m_RB.AddForce(NeededAcceleration, ForceMode2D.Force);
        }
    }
}
