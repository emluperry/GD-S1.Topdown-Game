using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Door : MonoBehaviour
{
    private SpriteRenderer m_Renderer;
    private Collider2D m_Collider;

    protected virtual void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<Collider2D>();
    }

    protected void ChangeState(bool state)
    {
        m_Renderer.enabled = state;
        m_Collider.enabled = state;
    }
}
