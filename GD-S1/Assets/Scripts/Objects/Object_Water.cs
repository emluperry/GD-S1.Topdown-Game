using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Object_Water : Object_Platform
{
    private SpriteRenderer m_Renderer;
    private int m_WaterID;

    [SerializeField] private Sprite m_WaterSprite;
    [SerializeField] private Sprite m_FrozenSprite;
    private bool m_IsFrozen = false;

    public Action<int> OnMagicHit;
    [SerializeField] private int m_MagicCost = 1;

    public Action<int, bool> OnWaterStateChange;

    protected override void Awake()
    {
        base.Awake();

        m_Renderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9) //player weapon layer
        {
            if (!collision.gameObject.TryGetComponent<Weapon_AffinityMagic>(out Weapon_AffinityMagic affinityComp))
                return;

            AFFINITY_TYPE magicType = affinityComp.GetAffinity();
            if (!m_IsFrozen && magicType == Enums.AFFINITY_TYPE.ICE)
            {
                if(!m_IsFrozen)
                {
                    OnMagicHit?.Invoke(m_MagicCost);
                    SetFrozen(true);
                }
            }
            else if (m_IsFrozen && magicType == Enums.AFFINITY_TYPE.FIRE)
            {
                OnMagicHit?.Invoke(m_MagicCost);
                SetFrozen(false);
            }
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (m_IsFrozen)
            base.OnTriggerStay2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (m_IsFrozen)
            base.OnTriggerExit2D(collision);
    }

    private void SetFrozen(bool isFrozen)
    {
        m_IsFrozen = isFrozen;
        if(isFrozen)
        {
            m_Renderer.sprite = m_FrozenSprite;
        }
        else
        {
            m_Renderer.sprite = m_WaterSprite;
        }
        OnWaterStateChange?.Invoke(m_WaterID, isFrozen);
    }

    public void SetID(int id)
    {
        m_WaterID = id;
    }
}
