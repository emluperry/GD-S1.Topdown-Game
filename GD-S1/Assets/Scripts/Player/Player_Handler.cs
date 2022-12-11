using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public class Player_Handler : MonoBehaviour
{
    [SerializeField] private Player_Movement m_Player;
    private Entity_Health m_Health;
    [SerializeField] private Weapon_Movement m_Weapon;
    private Weapon_AffinityMagic m_WeaponAffinity;

    private Player_Animation m_PlayerAnim;

    public Action<AFFINITY_TYPE> UpdateCurrentAffinity;
    public Action<bool> SetCurrentAffinity;
    public Action<SEGMENT_TYPE, float> OnStatValueChange;
    public Action OnKilled;

    private void Awake()
    {
        m_Health = m_Player.GetComponent<Entity_Health>();

        m_Health.DamageUpdated += HealthChanged;
        m_Health.Killed += PlayerKilled;

        m_WeaponAffinity = m_Weapon.GetComponent<Weapon_AffinityMagic>();

        m_WeaponAffinity.MagicUpdated += MagicChanged;
        m_WeaponAffinity.OnAffinitySwapped += SwapSavedAffinity;
        m_WeaponAffinity.OnAffinitySet += SetAffinity;

        m_PlayerAnim = GetComponent<Player_Animation>();
    }

    private void OnDestroy()
    {
        m_Health.DamageUpdated -= HealthChanged;
        m_Health.Killed -= PlayerKilled;

        m_WeaponAffinity.MagicUpdated -= MagicChanged;
        m_WeaponAffinity.OnAffinitySwapped -= SwapSavedAffinity;
        m_WeaponAffinity.OnAffinitySet -= SetAffinity;
    }

    private void PlayerKilled()
    {
        OnKilled?.Invoke();
    }

    private void HealthChanged(float dec)
    {
        OnStatValueChange?.Invoke(SEGMENT_TYPE.HEALTH, dec);
    }

    private void MagicChanged(float dec)
    {
        OnStatValueChange?.Invoke(SEGMENT_TYPE.MAGIC, dec);
    }

    private void SwapSavedAffinity(AFFINITY_TYPE type)
    {
        UpdateCurrentAffinity?.Invoke(type);
    }

    private void SetAffinity(AFFINITY_TYPE type, bool wasSet)
    {
        SetCurrentAffinity?.Invoke(wasSet);
        m_PlayerAnim.UpdateAffinityColours(type);
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

    public void RecoverStat(int amount, COLLECTABLE_TYPE type)
    {
        switch(type)
        {
            case COLLECTABLE_TYPE.HEALTH:
                m_Health.HealHealth(amount);
                break;

            case COLLECTABLE_TYPE.MAGIC:
                m_WeaponAffinity.GainMagic(amount);
                break;
        }
    }

    public void LoseMagic(int cost)
    {
        m_WeaponAffinity.LoseMagic(cost);
    }
}
