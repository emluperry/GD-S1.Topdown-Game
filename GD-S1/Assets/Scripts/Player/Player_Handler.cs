using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Handler : MonoBehaviour
{
    [SerializeField] private Player_Movement m_Player;
    private Entity_Health m_Health;
    [SerializeField] private Weapon_Movement m_Weapon;
<<<<<<< Updated upstream
=======
    private Weapon_AffinityMagic m_WeaponAffinity;
    private SFXHandler m_WeaponSFX;

    private Player_Animation m_PlayerAnim;
>>>>>>> Stashed changes

    public Action<float> OnDamageTaken;
    public Action OnKilled;

    private void Awake()
    {
        m_Health = m_Player.GetComponent<Entity_Health>();

        m_Health.DamageTaken += TookDamage;
        m_Health.Killed += PlayerKilled;
<<<<<<< Updated upstream
=======

        m_WeaponAffinity = m_Weapon.GetComponent<Weapon_AffinityMagic>();

        m_WeaponAffinity.OnObjectHit += ObjectHit;
        m_WeaponAffinity.MagicUpdated += MagicChanged;
        m_WeaponAffinity.OnAffinitySwapped += SwapSavedAffinity;
        m_WeaponAffinity.OnAffinitySet += SetAffinity;

        m_PlayerAnim = GetComponent<Player_Animation>();

        m_WeaponSFX = m_Weapon.GetComponent<SFXHandler>();
>>>>>>> Stashed changes
    }

    private void OnDestroy()
    {
        m_Health.DamageTaken -= TookDamage;
        m_Health.Killed -= PlayerKilled;
<<<<<<< Updated upstream
=======

        m_WeaponAffinity.OnObjectHit -= ObjectHit;
        m_WeaponAffinity.MagicUpdated -= MagicChanged;
        m_WeaponAffinity.OnAffinitySwapped -= SwapSavedAffinity;
        m_WeaponAffinity.OnAffinitySet -= SetAffinity;
>>>>>>> Stashed changes
    }

    private void PlayerKilled()
    {
        OnKilled?.Invoke();
    }

<<<<<<< Updated upstream
    private void TookDamage(float dec)
=======
    private void ObjectHit()
    {
        m_WeaponSFX.PlaySound();
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
>>>>>>> Stashed changes
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
