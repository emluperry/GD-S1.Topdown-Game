using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    [SerializeField][Min(0f)] private int m_MaximumHealth = 1;
    private int m_CurrentHealth;

    private void Awake()
    {
        m_CurrentHealth = m_MaximumHealth;
    }

    public void TakeDamage(int dmg)
    {
        m_CurrentHealth -= dmg;

        if(m_CurrentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
