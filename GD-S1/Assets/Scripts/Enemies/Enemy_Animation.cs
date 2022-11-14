using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Animation : MonoBehaviour
{
    private Vector2 m_InputDirection = new Vector2(0, 0);

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void SetInputDirection(float x, float y)
    {
        m_InputDirection = new Vector2(x, y);
    }

    public void SetChargingBool()
    {
        m_Animator.SetBool("isCharging", true);
    }

    public void SetAttackTrigger()
    {
        m_Animator.SetBool("isCharging", false);
        m_Animator.SetTrigger("Attack");
    }

    public void SetDeadTrigger()
    {
        m_Animator.SetTrigger("Dead");
    }

    private void AdjustDirection()
    {
        if (m_InputDirection.x > 0)
        {
            m_Animator.transform.localScale = new Vector2(-1, 1);
        }
        else if (m_InputDirection.x < 0)
        {
            m_Animator.transform.localScale = new Vector2(1, 1);
        }
    }

    private void FixedUpdate()
    {
        AdjustDirection();
    }
}
