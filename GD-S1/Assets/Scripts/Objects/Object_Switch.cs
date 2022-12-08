using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Switch : Trigger
{
    private SpriteRenderer m_Renderer;

    [SerializeField] private Sprite m_DeactivatedSprite;
    [SerializeField] private Sprite m_ActivatedSprite;

    [SerializeField] private bool m_CanDeactivate;
    [SerializeField][Min(0f)] private float m_ActivationDuration = 1;
    private Coroutine m_DurationCoroutine;

    private void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9) //player weapon layer
        {
            ChangeState(!m_CurrentState);
        }
    }

    private IEnumerator CountdownActivation()
    {
        yield return new WaitForSeconds(m_ActivationDuration);

        ChangeState(false);
    }

    protected override void ChangeState(bool state)
    {
        base.ChangeState(state);

        if (state)
        {
            m_Renderer.sprite = m_ActivatedSprite;
            if (m_CanDeactivate)
                m_DurationCoroutine = StartCoroutine(CountdownActivation());
        }
        else
        {
            m_Renderer.sprite = m_DeactivatedSprite;
            if (m_CanDeactivate)
                StopCoroutine(m_DurationCoroutine);
        }
    }
}
