using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    [SerializeField][Min(0f)] private int m_maximumHealth = 1;
    private int m_currentHealth;

    private void Awake()
    {
        m_currentHealth = m_maximumHealth;
    }

    public void TakeDamage(int dmg)
    {
        m_currentHealth -= dmg;

        if(m_currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
