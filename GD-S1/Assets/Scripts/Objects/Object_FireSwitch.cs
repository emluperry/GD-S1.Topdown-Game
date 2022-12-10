using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_FireSwitch : Object_Switch
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9) //player weapon layer
        {
            if(collision.gameObject.GetComponent<Weapon_AffinityMagic>().GetAffinity() == Enums.AFFINITY_TYPE.FIRE)
            {
                ChangeState(true);
            }
            else
            {
                ChangeState(false);
            }
        }
    }
}
