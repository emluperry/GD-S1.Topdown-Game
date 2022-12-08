using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_TriggerableDoor : TriggerableObject
{
    private SpriteRenderer m_Renderer;
    private Collider2D m_Collider;

    protected override void Awake()
    {
        base.Awake();

        m_Renderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<Collider2D>();
    }

    protected override void ChangeState(bool state)
    {
        m_Renderer.enabled = !state;
        m_Collider.enabled = !state;
    }
}
