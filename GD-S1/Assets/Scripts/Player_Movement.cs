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
            m_Animator.SetBool("isWalking", true);
        }
        else if(m_InputDirection.x < 0)
        {
            transform.localScale = new Vector2(1, 1);
            m_Animator.SetBool("isWalking", true);
        }
        else
        {
            m_Animator.SetBool("isWalking", false);
        }

        if(m_WeaponInputDirection.magnitude > 0)
        {
            //change rotation of arm
            //current rotation dir. = unit vector in current rot. direction
            //target rotation dir. = unit vector between pos and weapon pos
            Vector2 direction = transform.localScale.x == 1 ? Vector2.right : Vector2.left;
            float val = Vector2.Dot((m_AttackArmJoint.localRotation * direction).normalized, (-transform.position + m_WeaponHead.position).normalized);
            float degree = Mathf.Acos(val) * Mathf.Rad2Deg;
            if (!float.IsNaN(degree))
                m_AttackArmJoint.Rotate(0, 0, degree);
        }
        else
        {
            m_AttackArmJoint.transform.rotation = m_AttackArmIdleRotation;
        }
    }
}
