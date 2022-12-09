using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Unlockable : Trigger
{
    private SpriteRenderer m_Renderer;
    private Collider2D m_Collider;

    public int m_UnlockableID = -1;
    public Action<int> OnUnlockAttempt;

    private void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3) //player layer
        {
            OnUnlockAttempt?.Invoke(m_UnlockableID);
        }
    }

    public void ShouldUnlock(bool canBeUnlocked)
    {
        if (canBeUnlocked)
        {
            ChangeState(canBeUnlocked);
            m_Renderer.enabled = false;
            m_Collider.enabled = false;
        }
    }
}
