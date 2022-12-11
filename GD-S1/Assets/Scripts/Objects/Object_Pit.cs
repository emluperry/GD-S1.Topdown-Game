using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Pit : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer != 9)
            collision.gameObject.GetComponent<Entity_Health>()?.TouchingPit(true);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 9)
            collision.gameObject.GetComponent<Entity_Health>()?.TouchingPit(false);
    }
}
