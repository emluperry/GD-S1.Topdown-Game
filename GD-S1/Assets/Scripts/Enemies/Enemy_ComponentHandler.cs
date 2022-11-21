using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ComponentHandler : MonoBehaviour
{
    private Enemy_Movement m_Movement;
    private Enemy_Animation m_Animation;
    private Entity_Health m_Health;

    void Awake()
    {
        m_Movement = GetComponent<Enemy_Movement>();
        m_Animation = GetComponent<Enemy_Animation>();
        m_Health = GetComponent<Entity_Health>();

        m_Movement.UpdateInput += m_Animation.SetInputDirection;
        m_Movement.ChargingAttack += m_Animation.SetChargingBool;
        m_Movement.Attack += m_Animation.SetAttackTrigger;

        m_Health.Killed += m_Animation.SetDeadTrigger;
        m_Health.Killed += m_Movement.SetKilled;
        m_Health.Destroyable += SetInactive;
    }

    private void SetInactive()
    {
        gameObject.SetActive(false);
    }


}
