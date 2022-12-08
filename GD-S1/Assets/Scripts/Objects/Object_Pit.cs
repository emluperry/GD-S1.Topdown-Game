using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Pit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Entity_Health>()?.TouchingPit(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Entity_Health>()?.TouchingPit(false);
    }
}
