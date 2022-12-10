using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Damage : MonoBehaviour
{
    [SerializeField] private AFFINITY_TYPE m_DamageAffinityType = AFFINITY_TYPE.STANDARD;

    [SerializeField][Min(0f)] int m_BaseDamage = 1;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        DealDamage(collision);
    }

    protected void DealDamage(Collision2D collision)
    {
        collision.gameObject.TryGetComponent<Entity_Health>(out Entity_Health healthComp);
        if (healthComp)
            healthComp.TakeDamage(m_BaseDamage, m_DamageAffinityType, collision);
    }

    public void SetAffinity(AFFINITY_TYPE type)
    {
        m_DamageAffinityType = type;
    }
}
