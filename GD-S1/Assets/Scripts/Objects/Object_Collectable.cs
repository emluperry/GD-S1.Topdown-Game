using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public class Object_Collectable : MonoBehaviour
{
    public Action<COLLECTABLE_TYPE, int> OnCollected;
    [SerializeField] private COLLECTABLE_TYPE m_Type;
    [SerializeField] private int m_CollectableWorth = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3) //player layer
        {
            OnCollected?.Invoke(m_Type, m_CollectableWorth);
            gameObject.SetActive(false);
        }
    }
}
