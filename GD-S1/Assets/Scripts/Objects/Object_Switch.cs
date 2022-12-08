using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Switch : MonoBehaviour
{
    private SpriteRenderer m_Renderer;

    [SerializeField] private Sprite m_DeactivatedSprite;
    [SerializeField] private Sprite m_ActivatedSprite;
    private bool m_CurrentState = false;

    [SerializeField] private bool m_CanDeactivate;
    [SerializeField][Min(0f)] private float m_ActivationDuration = 1;
    private Coroutine m_DurationCoroutine;

    public Action<bool> OnStateChange;

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

    private void ChangeState(bool state)
    {
        m_CurrentState = state;

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

        OnStateChange?.Invoke(m_CurrentState);
    }
}
