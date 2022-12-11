using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_TriggerableChest : TriggerableObject
{
    private Object_Container m_Container;
    [SerializeField] private SpriteRenderer m_Decoration;

    protected override void Awake()
    {
        m_Container = GetComponent<Object_Container>();

        base.Awake();
    }

    protected override void ChangeState(bool state)
    {
        if(state)
        {
            m_Container.OpenContainer();

            if (m_Decoration)
                m_Decoration.enabled = false;
        }
    }
}
