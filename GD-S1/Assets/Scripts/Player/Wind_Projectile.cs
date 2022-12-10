using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind_Projectile : Entity_Movement
{
    public bool m_IsActive = false;
    [SerializeField] private float m_Lifespan = 3f;
    private float m_CurrentLifespan = 0;

    public void SetDirection(Vector2 direction)
    {
        m_InputDirection = direction;
        m_CurrentLifespan = 0;
        m_IsActive = true;
    }

    private void FixedUpdate()
    {
        if(m_IsActive)
            ApplyMovement();

        m_CurrentLifespan += Time.fixedDeltaTime;
        if(m_CurrentLifespan >= m_Lifespan)
        {
            DisableProjectile();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6 || collision.gameObject.layer == 7 || collision.gameObject.layer == 8) //enemies, tilemap, tilemap objects
        {
            DisableProjectile();
        }
    }

    private void DisableProjectile()
    {
        m_InputDirection = Vector2.zero;
        m_IsActive = false;
        gameObject.SetActive(false);
    }
}
