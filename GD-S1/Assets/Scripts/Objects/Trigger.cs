using System;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    protected bool m_CurrentState = false;
    public Action<bool> OnStateChange;

    protected SFXHandler m_SFXHandler;

    protected virtual void Awake()
    {
        m_SFXHandler = GetComponent<SFXHandler>();
    }

    protected virtual void ChangeState(bool state)
    {
        m_CurrentState = state;

        OnStateChange?.Invoke(m_CurrentState);

        if(m_SFXHandler)
            m_SFXHandler.PlaySound();
    }
}
