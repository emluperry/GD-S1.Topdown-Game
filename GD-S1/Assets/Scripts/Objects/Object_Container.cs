using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public class Object_Container : MonoBehaviour
{
    [SerializeField] private GameObject m_RewardPrefab;
    private Object_Collectable[] m_RewardObjects;
    [SerializeField][Min(1)] private int m_RewardCount;
    [SerializeField] private float m_DropRadius = 3f;

    private Collider2D m_Collider;
    private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private Sprite m_EmptyChest;

    public Action<COLLECTABLE_TYPE, int> OnCollectableCollected;

    private void Start()
    {
        m_Collider = GetComponent<Collider2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_RewardObjects = new Object_Collectable[m_RewardCount];
        for (int i = 0; i < m_RewardCount; i++)
        {
            m_RewardObjects[i] = Instantiate(m_RewardPrefab, (Vector2)transform.position + (UnityEngine.Random.insideUnitCircle * m_DropRadius), Quaternion.identity, transform).GetComponent<Object_Collectable>();
            m_RewardObjects[i].OnCollected += CallCollectableCollected;
            m_RewardObjects[i].gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < m_RewardCount; i++)
        {
            m_RewardObjects[i].OnCollected -= CallCollectableCollected;
        }
    }

    public void OpenContainer()
    {
        foreach (Object_Collectable reward in m_RewardObjects)
        {
            reward.gameObject.SetActive(true);
        }

        m_Collider.enabled = false;
        m_SpriteRenderer.sprite = m_EmptyChest;
    }

    private void CallCollectableCollected(COLLECTABLE_TYPE collectable, int value)
    {
        OnCollectableCollected?.Invoke(collectable, value);
    }
}
