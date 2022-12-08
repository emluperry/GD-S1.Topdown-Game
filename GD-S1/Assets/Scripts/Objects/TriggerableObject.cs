using System;
using UnityEngine;

public class TriggerableObject : MonoBehaviour
{
    [SerializeField] protected Trigger[] m_Triggers;
    protected int m_ActiveTriggers = 0;

    public Action<bool> onStateUpdated;

    protected virtual void Awake()
    {
        foreach (Trigger trigger in m_Triggers)
        {
            trigger.OnStateChange += UpdateState;
        }
    }

    protected virtual void UpdateState(bool state)
    {
        if (state)
            m_ActiveTriggers++;
        else
            m_ActiveTriggers--;

        if (m_ActiveTriggers >= m_Triggers.Length)
        {
            ChangeState(true);
            onStateUpdated?.Invoke(true);
        }
        else
        {
            ChangeState(false);
            onStateUpdated?.Invoke(false);
        }
    }

    protected virtual void ChangeState(bool state)
    {

    }
}
