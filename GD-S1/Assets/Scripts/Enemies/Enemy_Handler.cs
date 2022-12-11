using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Handler : MonoBehaviour
{
    private Enemy_Movement m_Movement;
    private Enemy_Animation m_Animation;
    private Entity_Health m_Health;
    private Enemy_SFX m_SFXHandler;
    private UI_SegmentBar m_UIHealthbar;
    private Collider2D m_Collider;

    private bool m_IsAlive = true;

    public Action OnEnemyKilled;

    void Awake()
    {
        m_Movement = GetComponent<Enemy_Movement>();
        m_Animation = GetComponent<Enemy_Animation>();
        m_Health = GetComponent<Entity_Health>();
        m_SFXHandler = GetComponent<Enemy_SFX>();
        m_UIHealthbar = GetComponentInChildren<UI_SegmentBar>();
        m_Collider = GetComponent<Collider2D>();

        m_Movement.UpdateInput += m_Animation.SetInputDirection;
        m_Movement.ChargingAttack += m_Animation.SetChargingBool;
        m_Movement.Attack += m_Animation.SetAttackTrigger;

        m_Health.Killed += SlimeKilled;
        m_Health.DamageUpdated += TookDamage;

        m_Health.Destroyable += SetInactive;
    }

    private void SetInactive()
    {
        m_Movement.UpdateInput -= m_Animation.SetInputDirection;
        m_Movement.ChargingAttack -= m_Animation.SetChargingBool;
        m_Movement.Attack -= m_Animation.SetAttackTrigger;

        m_Health.DamageUpdated -= m_UIHealthbar.UpdateValue;

        m_Health.Destroyable -= SetInactive;

        gameObject.SetActive(false);
    }

    private void TookDamage(float dec)
    {
        m_UIHealthbar.UpdateValue(dec);
        m_SFXHandler.PlayHurtSFX();
    }

    private void SlimeKilled()
    {
        if (m_IsAlive)
        {
            m_Animation.SetDeadTrigger();
            m_Movement.SetKilled();
            m_Collider.enabled = false;

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
