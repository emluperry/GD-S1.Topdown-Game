using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Weapon_Movement : Entity_Movement
{
    [Header("Weapon Variables")]
    [SerializeField] private Transform m_Player;

    private Vector2 m_PrevLocation;
    private float m_DistTravelled;
    private bool m_Returning;
    private bool m_IsActive;

    [SerializeField][Min(0f)] private float m_MaxDistance = 4f;
    [SerializeField][Min(0f)] private float m_MaxReturnDistance = 0.1f;
    [SerializeField][Min(0f)] private float m_MaxReturnDistanceOffset = 0.1f;
    [SerializeField][Min(0f)] private float m_ReturnSpeed = 20f;

    private void Start()
    {
        ToggleVisibility(false);
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
    }

    private void FixedUpdate()
    {
        if (m_InputDirection.magnitude == 0 && m_IsActive == true)
        {
            m_Returning = true;
        }
        else if (m_InputDirection.magnitude > 0 && m_IsActive == false)
        {
            m_RB.MovePosition(m_Player.position);
            ToggleVisibility(true);
        }

        ApplyWeaponMovement();

        if(m_Returning)
        {
            if(Vector2.Distance(transform.position, m_Player.position) <= m_MaxReturnDistance + m_MaxReturnDistanceOffset)
            {
                ToggleVisibility(false);
                m_RB.velocity = new Vector2(0, 0);
                m_DistTravelled = 0;
                m_RB.MovePosition(m_Player.position);
                m_PrevLocation = m_Player.position;
                m_Returning = false;
            }
        }
    }

    protected void ApplyWeaponMovement()
    {
        if(!m_Returning && m_IsActive == true)
        {
            m_DistTravelled += Vector2.Distance(m_PrevLocation, transform.position);
        }

        if (m_DistTravelled > m_MaxDistance || m_Returning || Vector2.Distance((Vector2)transform.position + m_InputDirection.normalized, m_Player.position) < Vector2.Distance(transform.position, m_Player.position) )
        {
            ReturnToPlayer();
        }
        else
        {
            m_PrevLocation = transform.position;

            ApplyMovement();
        }
    }

    private void ToggleVisibility(bool visibility)
    {
        m_IsActive = visibility;
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = visibility;
        }
        GetComponent<Collider2D>().enabled = visibility;
    }

    private void ReturnToPlayer()
    {
        if (!m_Returning)
        {
            m_RB.velocity = new Vector2(0, 0);
            m_Returning = true;
        }

        m_RB.MovePosition(Vector2.MoveTowards(transform.position, m_Player.position, m_ReturnSpeed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_Returning = true;
    }
}
