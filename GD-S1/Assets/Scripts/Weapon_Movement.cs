using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Movement : MonoBehaviour
{
    private Rigidbody2D m_RB;

    private Vector2 m_InputDirection;

    [Header("Weapon Motion")]
    [SerializeField][Min(0f)] private float m_MaxSpeed = 10f;
    [SerializeField][Min(0f)] private float m_MaxDistance = 4f;

    [SerializeField][Min(0f)] private float m_Acceleration = 200f;
    [SerializeField][Min(0f)] private float m_MaxAccelerationForce = 150f;
    [SerializeField][Min(0f)] private AnimationCurve m_AccelerationCurve;
    [SerializeField][Min(0f)] private AnimationCurve m_MaxAccelerationCurve;

    private void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
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

    }
}
