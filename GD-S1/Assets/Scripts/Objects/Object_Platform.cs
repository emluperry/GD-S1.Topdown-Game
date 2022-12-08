using System.Collections;
using UnityEngine;

public class Object_Platform : TriggerableObject
{
    private Vector2 m_StartingPos;
    [SerializeField] private Vector2 m_Destination = Vector2.zero;
    [SerializeField] private float m_MoveSpeed = 1f;
    [SerializeField] private float m_DestinationOffset = 0.5f;
    private Coroutine m_MovingCoroutine;

    private Rigidbody2D m_RB;

    protected override void Awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_StartingPos = transform.position;

        base.Awake();
    }

    protected override void ChangeState(bool state)
    {
        if (state)
            MoveTo(m_Destination);
        else
            MoveTo(m_StartingPos);
    }

    private void MoveTo(Vector2 destination)
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
