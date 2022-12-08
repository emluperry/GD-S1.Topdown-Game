using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Platform : MonoBehaviour
{
    private Vector2 m_StartingPos;
    [SerializeField] private Vector2 m_Destination = Vector2.zero;
    [SerializeField] private float m_MoveSpeed = 1f;
    [SerializeField] private float m_DestinationOffset = 0.5f;
    private Coroutine m_MovingCoroutine;

    private Rigidbody2D m_RB;

    [SerializeField] private Object_Switch[] m_Switches;
    private int m_ActiveSwitches = 0;

    private void Awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_StartingPos = transform.position;

        foreach(Object_Switch switchObj in m_Switches)
        {
            switchObj.OnStateChange += UpdateSwitchState;
        }
    }

    private void OnDestroy()
    {
        foreach (Object_Switch switchObj in m_Switches)
        {
            switchObj.OnStateChange -= UpdateSwitchState;
        }
    }

    private void UpdateSwitchState(bool state)
    {
        if (state)
            m_ActiveSwitches++;
        else
            m_ActiveSwitches--;

        if (m_ActiveSwitches >= m_Switches.Length)
        {
            UpdateState(m_Destination);
        }
        else
        {
            UpdateState(m_StartingPos);
        }
    }

    private void UpdateState(Vector2 destination)
    {
        if (m_MovingCoroutine != null)
            StopCoroutine(m_MovingCoroutine);

        m_MovingCoroutine = StartCoroutine(MoveToDestination(destination));
    }

    private IEnumerator MoveToDestination(Vector2 destination)
    {
        Vector2 normalisedDirectionVector = (destination - (Vector2)transform.position).normalized;

        while(((Vector2)transform.position - destination).sqrMagnitude > m_DestinationOffset * m_DestinationOffset)
        {
            m_RB.MovePosition(transform.position + (Vector3)normalisedDirectionVector * m_MoveSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        m_RB.MovePosition(destination);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Entity_Health>()?.SetGrounded(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Entity_Health>()?.SetGrounded(false);
    }
}
