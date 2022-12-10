using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_FireSwitch : Object_Switch
{
    public Action<int> OnMagicHit;
    [SerializeField] private int m_MagicCost = 1;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9) //player weapon layer
        {
            AFFINITY_TYPE magicType = collision.gameObject.GetComponent<Weapon_AffinityMagic>().GetAffinity();
            if (magicType == AFFINITY_TYPE.FIRE)
            {
                OnMagicHit?.Invoke(m_MagicCost);
                ChangeState(true);
            }
            else if(magicType == AFFINITY_TYPE.WIND)
            {
                OnMagicHit?.Invoke(m_MagicCost);
                ChangeState(false);
            }
        }
    }
}
