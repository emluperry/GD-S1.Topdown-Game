using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_TriggerableChest : TriggerableObject
{
    private Object_Container m_Container;

    protected override void Awake()
    {
        m_Container = GetComponent<Object_Container>();

        base.Awake();
    }

    protected override void ChangeState(bool state)
    {
        m_Container.OpenContainer();
    }
}
