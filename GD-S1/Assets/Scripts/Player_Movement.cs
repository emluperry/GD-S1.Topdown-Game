using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    //Components
    private Rigidbody2D m_RB;
    private Animator m_Animator;

    private Vector2 m_InputDirection;
    private Vector2 m_GoalVelocity;

    private Vector2 m_WeaponInputDirection;
    private Quaternion m_AttackArmIdleRotation;

    [Header("Player Motion")]
    [SerializeField] [Min(0f)] private float m_MaxSpeed = 1f;
    [SerializeField] [Min(0f)] private float m_Acceleration = 200f;
    [SerializeField] [Min(0f)] private float m_MaxAccelerationForce = 150f;
    [SerializeField] [Min(0f)] private AnimationCurve m_AccelerationCurve;
    [SerializeField] [Min(0f)] private AnimationCurve m_MaxAccelerationCurve;

    [Header("Weapon Animation")]
    [SerializeField] private Transform m_AttackArmJoint;
    [SerializeField] private Transform m_WeaponHead;
    [SerializeField] private SpriteRenderer m_WeaponChain;


    private void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_AttackArmIdleRotation = m_AttackArmJoint.rotation;
    }

    private void Update()
    {
        m_InputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        m_WeaponInputDirection = new Vector2(Input.GetAxis("W-Horizontal"), Input.GetAxis("W-Vertical"));
    }

    private void FixedUpdate()
    {
        UpdateAnimation();
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        float velDot = Vector2.Dot(m_InputDirection, m_GoalVelocity.normalized);
        float acceleration = m_Acceleration * m_AccelerationCurve.Evaluate(velDot);

        m_GoalVelocity = Vector2.MoveTowards(m_GoalVelocity, m_InputDirection * m_MaxSpeed, acceleration*Time.fixedDeltaTime);

        Vector2 NeededAcceleration = (m_GoalVelocity - new Vector2(m_RB.velocity.x, m_RB.velocity.y)) / Time.fixedDeltaTime;

        float MaxAcceleration = m_MaxAccelerationForce * m_MaxAccelerationCurve.Evaluate(velDot);

        NeededAcceleration = Vector3.ClampMagnitude(NeededAcceleration, MaxAcceleration);

        m_RB.AddForce(NeededAcceleration, ForceMode2D.Force);
    }

    private void UpdateAnimation()
    {
        if(m_InputDirection.x > 0)
        {
            transform.localScale = new Vector2(-1, 1);
            m_AttackArmJoint.localScale = new Vector2(-1, 1);
            m_Animator.SetBool("isWalking", true);
        }
        else if(m_InputDirection.x < 0)
        {
            transform.localScale = new Vector2(1, 1);
            m_AttackArmJoint.localScale = new Vector2(1, 1);
            m_Animator.SetBool("isWalking", true);
        }
        else
        {
            m_Animator.SetBool("isWalking", false);
        }

        if(m_WeaponInputDirection.magnitude > 0)
        {
            Vector3 WeaponVector = -m_AttackArmJoint.position + m_WeaponHead.position;
            Quaternion ArmRotation = Quaternion.identity;
            ArmRotation.SetFromToRotation((m_AttackArmJoint.rotation * Vector2.right).normalized, WeaponVector.normalized);
            m_AttackArmJoint.rotation *= ArmRotation;

            m_WeaponChain.size = new Vector2(WeaponVector.magnitude, m_WeaponChain.size.y);
            m_WeaponChain.transform.localPosition = new Vector3(0.5f * WeaponVector.magnitude, 0, 0);
        }
        else
        {
            m_AttackArmJoint.rotation = m_AttackArmIdleRotation;
            m_WeaponChain.size = new Vector2(0, m_WeaponChain.size.y);
        }
    }
}
