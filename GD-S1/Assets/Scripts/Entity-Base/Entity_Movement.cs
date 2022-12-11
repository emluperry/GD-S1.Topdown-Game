using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Entity_Movement : MonoBehaviour
{
    protected Rigidbody2D m_RB;
    protected Entity_Health m_Health;

    public bool m_IsPaused { protected get; set; } = false;

    protected Vector2 m_InputDirection;
    private Vector2 m_GoalVelocity = Vector2.zero;

    [Header("Entity Motion")]
    [SerializeField][Min(0f)] protected float m_MaxSpeed = 1f;
    [SerializeField][Min(0f)] protected float m_Acceleration = 200f;
    [SerializeField][Min(0f)] protected float m_MaxAccelerationForce = 150f;
    [SerializeField][Min(0f)] private AnimationCurve m_AccelerationCurve;
    [SerializeField][Min(0f)] private AnimationCurve m_MaxAccelerationCurve;
    [SerializeField][Min(0f)] private float m_MinKnockbackForce = 20;

    [Header("Falling Variables")]
    private Vector2 m_LastSafePosition = Vector2.zero;
    [SerializeField] private float m_SafePosOffsetDistance = 3f;
    [SerializeField] private float m_InputDelay = 1f;
    private bool m_PauseInput = false;

    protected virtual void Awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_Health = GetComponent<Entity_Health>();
    }

    protected virtual void Start()
    {
        if(m_Health)
        {
            m_Health.KnockbackEvent += ApplyKnockback;
            m_Health.onLeftSolidGround += SaveLastSafePosition;
            m_Health.onPitfall += ReturnToSolidGround;
        }
    }

    protected virtual void OnDestroy()
    {
        if (m_Health)
        {
            m_Health.KnockbackEvent -= ApplyKnockback;
            m_Health.onLeftSolidGround -= SaveLastSafePosition;
            m_Health.onPitfall -= ReturnToSolidGround;
        }
    }

    protected void ApplyMovement()
    {
        if (m_PauseInput)
            return;

        float velDot = Vector2.Dot(m_InputDirection, m_GoalVelocity.normalized);
        float acceleration = m_Acceleration * m_AccelerationCurve.Evaluate(velDot);

        m_GoalVelocity = Vector2.MoveTowards(m_GoalVelocity, m_InputDirection * m_MaxSpeed, acceleration * Time.fixedDeltaTime);

        Vector2 NeededAcceleration = (m_GoalVelocity - new Vector2(m_RB.velocity.x, m_RB.velocity.y)) / Time.fixedDeltaTime;

        float MaxAcceleration = m_MaxAccelerationForce * m_MaxAccelerationCurve.Evaluate(velDot);

        NeededAcceleration = Vector3.ClampMagnitude(NeededAcceleration, MaxAcceleration);

        m_RB.AddForce(NeededAcceleration, ForceMode2D.Force);
    }

    protected void ApplyKnockback(int dmg, Vector2 ForcePos, Vector2 ForceNormal)
    {
        m_RB.AddForceAtPosition(dmg * m_MinKnockbackForce * -ForceNormal, ForcePos);
    }

    private void SaveLastSafePosition()
    {
        m_LastSafePosition = (Vector2)transform.position + (m_SafePosOffsetDistance * -m_InputDirection.normalized);
    }

    private void ReturnToSolidGround()
    {
        m_RB.velocity = Vector2.zero;
        m_PauseInput = true;
        m_RB.MovePosition(m_LastSafePosition);

        StartCoroutine(DelayTimer());
    }

    private IEnumerator DelayTimer()
    {
        yield return new WaitForSecondsRealtime(m_InputDelay);

        m_PauseInput = false;
    }
}
