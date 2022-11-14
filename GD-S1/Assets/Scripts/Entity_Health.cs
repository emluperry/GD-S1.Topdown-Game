using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    public Action Killed;
    public Action Destroyable;

    [SerializeField][Min(0f)] private int m_MaximumHealth = 1;
    private int m_CurrentHealth;
    [SerializeField][Min(0f)] private float m_KillDelay = 1f;

    private void Awake()
    {
        m_CurrentHealth = m_MaximumHealth;
    }

    public void TakeDamage(int dmg)
    {
        m_CurrentHealth -= dmg;

        if(m_CurrentHealth <= 0)
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
