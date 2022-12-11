using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Handler : MonoBehaviour
{
    private Enemy_Movement m_Movement;
    private Enemy_Animation m_Animation;
    private Entity_Health m_Health;
<<<<<<< Updated upstream
    private UI_Healthbar m_UIHealthbar;
=======
    private Enemy_SFX m_EnemySFX;
    private UI_SegmentBar m_UIHealthbar;
    private Collider2D m_Collider;
>>>>>>> Stashed changes

    private bool m_IsAlive = true;

    public Action OnEnemyKilled;

    void Awake()
    {
        m_Movement = GetComponent<Enemy_Movement>();
        m_Animation = GetComponent<Enemy_Animation>();
        m_Health = GetComponent<Entity_Health>();
<<<<<<< Updated upstream
        m_UIHealthbar = GetComponentInChildren<UI_Healthbar>();
=======
        m_EnemySFX = GetComponent<Enemy_SFX>();
        m_UIHealthbar = GetComponentInChildren<UI_SegmentBar>();
        m_Collider = GetComponent<Collider2D>();
>>>>>>> Stashed changes

        m_Movement.UpdateInput += m_Animation.SetInputDirection;
        m_Movement.ChargingAttack += m_Animation.SetChargingBool;
        m_Movement.Attack += m_Animation.SetAttackTrigger;

        m_Health.Killed += SlimeKilled;
<<<<<<< Updated upstream
        m_Health.DamageTaken += m_UIHealthbar.UpdateHealth;
=======
        m_Health.DamageUpdated += TakeDamage;
>>>>>>> Stashed changes

        m_Health.Destroyable += SetInactive;
    }

    private void SetInactive()
    {
        m_Movement.UpdateInput -= m_Animation.SetInputDirection;
        m_Movement.ChargingAttack -= m_Animation.SetChargingBool;
        m_Movement.Attack -= m_Animation.SetAttackTrigger;

        m_Health.DamageTaken -= m_UIHealthbar.UpdateHealth;

        m_Health.Destroyable -= SetInactive;

        gameObject.SetActive(false);
    }

    private void TakeDamage(float dec)
    {
        m_UIHealthbar.UpdateValue(dec);
        m_EnemySFX.PlayHurtSFX();
    }

    private void SlimeKilled()
    {
        if (m_IsAlive)
        {
            m_Animation.SetDeadTrigger();
            m_Movement.SetKilled();

            OnEnemyKilled?.Invoke();
            m_IsAlive = false;
        }
    }

    public void Initialise(Player_Movement player)
    {
        m_Movement.Initialise(player);
    }

    public void SetPaused(bool paused)
    {
        m_Movement.m_IsPaused = paused;
    }
}
