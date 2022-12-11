using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Handler : MonoBehaviour
{
    [SerializeField] private Player_Movement m_Player;
    private Entity_Health m_Health;
    [SerializeField] private Weapon_Movement m_Weapon;

    public Action<float> OnDamageTaken;
    public Action OnKilled;

    private void Awake()
    {
        m_Health = m_Player.GetComponent<Entity_Health>();

        m_Health.DamageTaken += TookDamage;
        m_Health.Killed += PlayerKilled;
    }

    private void OnDestroy()
    {
        m_Health.DamageTaken -= TookDamage;
        m_Health.Killed -= PlayerKilled;
    }

    private void PlayerKilled()
    {
        OnKilled?.Invoke();
    }

    private void TookDamage(float dec)
    {
        OnDamageTaken?.Invoke(dec);
    }

    public void SetPause(bool paused)
    {
        m_Player.m_IsPaused = paused;
        m_Weapon.m_IsPaused = paused;
    }

    public Player_Movement GetPlayerMovementComponent()
    {
        return m_Player;
    }
}
