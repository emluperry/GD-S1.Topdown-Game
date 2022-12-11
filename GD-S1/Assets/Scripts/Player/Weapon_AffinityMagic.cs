using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class Weapon_AffinityMagic : Attack_Damage
{
    [Header("Affinity Input")]
    private bool m_QInput = false;
    private bool m_EInput = false;
    [SerializeField] private float m_SwapInputDelay = 0.1f;
    private float m_CurrentSwapDelay = 0;
    [SerializeField] private float m_ApplyInputDelay = 0.5f;
    private float m_CurrentApplyDelay = 0;

    [Header("Magic Values")]
    [SerializeField] private int m_MaximumMagic = 5;
    private int m_CurrentMagic;
    [SerializeField] private int m_MagicCost = 1;

    private bool m_MagicIsActive;
    private AFFINITY_TYPE m_CurrentAffinity = AFFINITY_TYPE.FIRE;

    [SerializeField] private GameObject m_WindProjectile;
    private Wind_Projectile m_ProjectileInstance;

    public Action<float> MagicUpdated;
    public Action<AFFINITY_TYPE> OnAffinitySwapped;
    public Action<AFFINITY_TYPE, bool> OnAffinitySet;

    private void Awake()
    {
        m_ProjectileInstance = Instantiate(m_WindProjectile, Vector2.zero, Quaternion.identity).GetComponent<Wind_Projectile>();
        m_ProjectileInstance.gameObject.SetActive(false);
        GetComponent<Weapon_Movement>().OnWeaponFired += FireProjectile;

        m_CurrentSwapDelay = m_SwapInputDelay;
        m_CurrentApplyDelay = m_ApplyInputDelay;

        m_CurrentMagic = m_MaximumMagic;
    }

    private void OnDestroy()
    {
        GetComponent<Weapon_Movement>().OnWeaponFired -= FireProjectile;
    }

    private void Update()
    {
        if (m_CurrentSwapDelay >= m_SwapInputDelay)
        {
            m_QInput = Input.GetKey(KeyCode.Q);
        }

        if (m_CurrentApplyDelay >= m_ApplyInputDelay)
        {
            m_EInput = Input.GetKey(KeyCode.E);
        }
    }

    private void FixedUpdate()
    {
        if(m_QInput)
        {
            m_CurrentAffinity++;
            if((int)m_CurrentAffinity > 3)
            {
                m_CurrentAffinity = AFFINITY_TYPE.FIRE;
            }

            if(m_MagicIsActive)
            {
                ApplyAffinity(m_CurrentAffinity);
            }

            OnAffinitySwapped?.Invoke(m_CurrentAffinity);

            m_QInput = false;
            m_CurrentSwapDelay = 0;
        }

        if(m_EInput)
        {
            if(m_MagicIsActive)
            {
                ApplyAffinity(AFFINITY_TYPE.STANDARD);
                m_MagicIsActive = false;
            }
            else
            {
                if(m_CurrentMagic > 0)
                {
                    ApplyAffinity(m_CurrentAffinity);
                    m_MagicIsActive = true;
                }
            }

            m_EInput = false;
            m_CurrentApplyDelay = 0;
        }

        if (m_CurrentSwapDelay < m_SwapInputDelay)
            m_CurrentSwapDelay += Time.fixedDeltaTime;

        if (m_CurrentApplyDelay < m_ApplyInputDelay)
            m_CurrentApplyDelay += Time.fixedDeltaTime;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            DealDamage(collision);

            if (m_MagicIsActive)
                LoseMagic(m_MagicCost);
        }

        OnHitObject?.Invoke();
    }

    private void FireProjectile(Vector2 direction)
    {
        if(m_MagicIsActive && m_CurrentAffinity == AFFINITY_TYPE.WIND && !m_ProjectileInstance.m_IsActive)
        {
            LoseMagic(m_MagicCost);
            m_ProjectileInstance.gameObject.SetActive(true);
            m_ProjectileInstance.transform.position = transform.position;
            m_ProjectileInstance.SetDirection(direction.normalized);
        }
    }

    private void ApplyAffinity(AFFINITY_TYPE type)
    {
        SetAffinity(type);

        if (type == AFFINITY_TYPE.STANDARD)
            OnAffinitySet?.Invoke(type, false);
        else
            OnAffinitySet?.Invoke(type, true);
    }

    public void LoseMagic(int decrement)
    {
        UpdateMagic(-decrement);
        if(m_CurrentMagic <= 0)
        {
            ApplyAffinity(AFFINITY_TYPE.STANDARD);
            m_MagicIsActive = false;
        }
    }

    public void GainMagic(int increment)
    {
        UpdateMagic(increment);
    }

    private void UpdateMagic(int amount)
    {
        m_CurrentMagic += amount;

        if (m_CurrentMagic < 0)
            m_CurrentMagic = 0;
        else if (m_CurrentMagic > m_MaximumMagic)
            m_CurrentMagic = m_MaximumMagic;

        float dec = m_CurrentMagic / (float)m_MaximumMagic;
        MagicUpdated?.Invoke(dec);
    }
}
