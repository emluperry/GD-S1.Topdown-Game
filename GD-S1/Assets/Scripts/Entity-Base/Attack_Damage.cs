using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Damage : MonoBehaviour
{
    [SerializeField][Min(0f)] int m_BaseDamage = 1;

<<<<<<< Updated upstream
    private void OnCollisionEnter2D(Collision2D collision)
=======
    public Action OnObjectHit;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        DealDamage(collision);

        OnObjectHit?.Invoke();
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

    public AFFINITY_TYPE GetAffinity()
>>>>>>> Stashed changes
    {
        collision.gameObject.GetComponent<Entity_Health>()?.TakeDamage(m_BaseDamage, collision);
    }
}
