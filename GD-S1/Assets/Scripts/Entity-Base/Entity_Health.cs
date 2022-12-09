using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    [SerializeField] private AFFINITY_TYPE m_HealthAffinityType = AFFINITY_TYPE.STANDARD;

    public Action<float> DamageTaken;
    public Action<int, Vector2, Vector2> KnockbackEvent;
    public Action Killed;
    public Action Destroyable;

    public Action onLeftSolidGround;
    public Action onPitfall;

    [SerializeField][Min(0f)] private int m_MaximumHealth = 1;
    private int m_CurrentHealth;
    [SerializeField][Min(0f)] private float m_KillDelay = 1f;

    private bool m_IsGrounded = false;
    [SerializeField] private int m_FallDamage = 2;
    private Coroutine m_FallingCoroutine;

    private void Awake()
    {
        m_CurrentHealth = m_MaximumHealth;
    }

    public void TakeDamage(int dmg, AFFINITY_TYPE type, Collision2D collision)
    {
        m_CurrentHealth -= CalculateDamage(dmg, type);

        float dec = m_CurrentHealth / (float)m_MaximumHealth;
        DamageTaken?.Invoke(dec);

        if(collision != null)
            KnockbackEvent?.Invoke(dmg, collision.GetContact(0).point, collision.GetContact(0).normal);
        

        if (m_CurrentHealth <= 0)
        {
            Killed?.Invoke();
            StartCoroutine(Dying());
        }
    }

    private int CalculateDamage(int dmg, AFFINITY_TYPE type)
    {
        int damage = dmg;
        if ((m_HealthAffinityType == AFFINITY_TYPE.FIRE && type == AFFINITY_TYPE.ICE) || (m_HealthAffinityType == AFFINITY_TYPE.ICE && type == AFFINITY_TYPE.WIND) || (m_HealthAffinityType == AFFINITY_TYPE.WIND && type == AFFINITY_TYPE.FIRE))
            damage /= 2;
        else if ((m_HealthAffinityType == AFFINITY_TYPE.FIRE && type == AFFINITY_TYPE.WIND) || (m_HealthAffinityType == AFFINITY_TYPE.ICE && type == AFFINITY_TYPE.FIRE) || (m_HealthAffinityType == AFFINITY_TYPE.WIND && type == AFFINITY_TYPE.ICE))
            damage *= 2;

        Debug.Log(damage);
        return damage;
    }

    public IEnumerator Dying()
    {
        yield return new WaitForSecondsRealtime(m_KillDelay);
        Destroyable?.Invoke();
    }

    public void TouchingPit(bool isTouchingPit)
    {
        if (isTouchingPit)
        {
            m_FallingCoroutine = StartCoroutine(CheckForFall());
            onLeftSolidGround?.Invoke();
        }
        else
        {
            if (m_FallingCoroutine != null)
                StopCoroutine(m_FallingCoroutine);
        }
    }

    private IEnumerator CheckForFall()
    {
        yield return new WaitUntil(() => m_IsGrounded == false);

        TakeDamage(m_FallDamage, AFFINITY_TYPE.STANDARD, null);
        onPitfall?.Invoke();
    }

    public void SetGrounded(bool isGrounded)
    {
        m_IsGrounded = isGrounded;
    }
}
