using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    public Action<float> DamageTaken;
    public Action<int, Vector2, Vector2> KnockbackEvent;
    public Action Killed;
    public Action Destroyable;

    [SerializeField][Min(0f)] private int m_MaximumHealth = 1;
    private int m_CurrentHealth;
    [SerializeField][Min(0f)] private float m_KillDelay = 1f;

    private void Awake()
    {
        m_CurrentHealth = m_MaximumHealth;
    }

    public void TakeDamage(int dmg, Collision2D collision)
    {
        m_CurrentHealth -= dmg;

        float dec = m_CurrentHealth / (float)m_MaximumHealth;
        DamageTaken?.Invoke(dec);
        KnockbackEvent?.Invoke(dmg, collision.GetContact(0).point, collision.GetContact(0).normal);
        

        if (m_CurrentHealth <= 0)
        {
            Killed?.Invoke();
            StartCoroutine(Dying());
        }
    }

    public IEnumerator Dying()
    {
        yield return new WaitForSecondsRealtime(m_KillDelay);
        Destroyable?.Invoke();
    }
}
