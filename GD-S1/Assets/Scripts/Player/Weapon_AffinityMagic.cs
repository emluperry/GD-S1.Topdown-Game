using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class Weapon_AffinityMagic : MonoBehaviour
{
    private bool m_QInput = false;
    private bool m_EInput = false;
    [SerializeField] private float m_SwapInputDelay = 0.1f;
    private float m_CurrentSwapDelay = 0;
    [SerializeField] private float m_ApplyInputDelay = 0.5f;
    private float m_CurrentApplyDelay = 0;

    private bool m_MagicIsActive;
    private AFFINITY_TYPE m_CurrentAffinity = AFFINITY_TYPE.FIRE;

    private Attack_Damage m_AttackDamageComponent;

    public Action<AFFINITY_TYPE> OnAffinitySwapped;
    public Action<AFFINITY_TYPE> OnAffinitySet;

    private void Awake()
    {
        m_AttackDamageComponent = GetComponent<Attack_Damage>();

        m_CurrentSwapDelay = m_SwapInputDelay;
        m_CurrentApplyDelay = m_ApplyInputDelay;
    }

    private void Update()
    {
        m_QInput = Input.GetKeyDown(KeyCode.Q);
        m_EInput = Input.GetKeyDown(KeyCode.E);
    }

    private void FixedUpdate()
    {
        if(m_QInput && m_CurrentSwapDelay >= m_SwapInputDelay)
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

            m_CurrentSwapDelay = 0;
            m_QInput = false;
        }

        if(m_EInput && m_CurrentApplyDelay >= m_ApplyInputDelay)
        {
            if(m_MagicIsActive)
            {
                ApplyAffinity(AFFINITY_TYPE.STANDARD);
                m_MagicIsActive = false;
            }
            else
            {
                ApplyAffinity(m_CurrentAffinity);
                m_MagicIsActive = true;
            }

            m_CurrentApplyDelay = 0;
            m_EInput = false;
        }

        if (m_CurrentSwapDelay < m_SwapInputDelay)
            m_CurrentSwapDelay += Time.fixedDeltaTime;

        if (m_CurrentApplyDelay < m_ApplyInputDelay)
            m_CurrentApplyDelay += Time.fixedDeltaTime;
    }

    private void ApplyAffinity(AFFINITY_TYPE type)
    {
        m_AttackDamageComponent.SetAffinity(type);

        OnAffinitySet?.Invoke(type);
    }
}
