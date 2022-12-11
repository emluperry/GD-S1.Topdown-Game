using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_WaterCollection : MonoBehaviour
{
    private Object_Water[] m_WaterObjects;
    public Action<int> OnMagicHit;

    private void Awake()
    {
        m_WaterObjects = GetComponentsInChildren<Object_Water>();
        for(int i = 0; i < m_WaterObjects.Length; i++)
        {
            m_WaterObjects[i].OnMagicHit += LoseMagic;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < m_WaterObjects.Length; i++)
        {
            m_WaterObjects[i].OnMagicHit -= LoseMagic;
        }
    }

    private void LoseMagic(int cost)
    {
        OnMagicHit?.Invoke(cost);
    }
}
