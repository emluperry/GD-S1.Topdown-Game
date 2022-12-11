using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Enums;

public class Object_Breakable : Object_Container
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9) //player weapon layer
        {
            OpenContainer();
        }
    }
}
