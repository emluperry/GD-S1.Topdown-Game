using System.Collections;
using UnityEngine;

public class Object_Platform : TriggerableObject
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Entity_Health>()?.SetGrounded(true);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Entity_Health>()?.SetGrounded(false);
    }
}
