using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Damage : MonoBehaviour
{
    [SerializeField] private AFFINITY_TYPE m_DamageAffinityType = AFFINITY_TYPE.STANDARD;

    [SerializeField][Min(0f)] int m_BaseDamage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Entity_Health>()?.TakeDamage(m_BaseDamage, m_DamageAffinityType, collision);
    }
}
