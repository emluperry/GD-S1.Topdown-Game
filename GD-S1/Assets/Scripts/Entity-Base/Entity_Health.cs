using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    [SerializeField] private AFFINITY_TYPE m_HealthAffinityType = AFFINITY_TYPE.STANDARD;

    public Action<float> DamageUpdated;
    public Action<int, Vector2, Vector2> KnockbackEvent;
    public Action Killed;
    public Action Destroyable;

    public Action onLeftSolidGround;
    public Action onPitfall;

    [SerializeField][Min(0f)] private int m_MaximumHealth = 1;
    private int m_CurrentHealth;
    [SerializeField][Min(0f)] private float m_KillDelay = 1f;

    [SerializeField][Min(0f)] private float m_InvincibilityDelay = 0;
    private bool m_Invincible = false;

    private bool m_IsGrounded = false;
    [SerializeField] private int m_FallDamage = 2;
    [SerializeField] private float m_SolidGroundCheckDelay = 0.05f;
    private Coroutine m_FallingCoroutine;

    private void Awake()
    {
        m_CurrentHealth = m_MaximumHealth;
    }

    public void TakeDamage(int dmg, AFFINITY_TYPE type, Collision2D collision)
    {
        if (m_Invincible)
            return;

        UpdateHealth(-CalculateDamage(dmg, type));

        if(collision != null)
            KnockbackEvent?.Invoke(dmg, collision.GetContact(0).point, collision.GetContact(0).normal);
    }

    public void HealHealth(int recovered)
    {
        UpdateHealth(recovered);
    }

    private void UpdateHealth(int amount)
    {
        if (m_CurrentHealth < 0)
            m_CurrentHealth = 0;
        else if (m_CurrentHealth > m_MaximumHealth)
            m_CurrentHealth = m_MaximumHealth;

        m_CurrentHealth += amount;

        float dec = m_CurrentHealth / (float)m_MaximumHealth;
        DamageUpdated?.Invoke(dec);

        StartCoroutine(InvincibilityTimer());

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

        return damage;
    }

    public void SetAffinity(AFFINITY_TYPE type)
    {
        m_HealthAffinityType = type;
    }

    public IEnumerator Dying()
    {
        yield return new WaitForSecondsRealtime(m_KillDelay);
        Destroyable?.Invoke();
    }

    public IEnumerator InvincibilityTimer()
    {
        m_Invincible = true;

        yield return new WaitForSecondsRealtime(m_InvincibilityDelay);

        m_Invincible = false;
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
        while(m_IsGrounded)
        {
            yield return new WaitUntil(() => m_IsGrounded == false);
            yield return new WaitForSecondsRealtime(m_SolidGroundCheckDelay);
        }

        TakeDamage(m_FallDamage, AFFINITY_TYPE.STANDARD, null);
        onPitfall?.Invoke();
    }

    public void SetGrounded(bool isGrounded)
    {
        m_IsGrounded = isGrounded;
    }
}
